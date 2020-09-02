using System;
using System.Collections.Generic;
using NAudio.Wave;
using VarispeedDemo.SoundTouch;

namespace LanguageAudioTool
{
    // This creates audio samples (floating point format), of an entire, fully processed file of audio and silence sections,
    // processing them as configured by the user
    internal class SectionedSampleProvider : ISampleProvider, IDisposable
    {
        private readonly AudioFileReader _reader;
        private readonly ISampleProvider _sourceProvider;

        private SoundTouchProfile _soundTouchProfile;
        private readonly SoundTouch _soundTouch;
        private readonly float[] _sourceReadBuffer;
        private readonly float[] _soundTouchReadBuffer;
        private float _playbackRate = 1.0f;
        private bool _repositionRequested;

        private readonly int _channels;
        private readonly List<SectionInfo> _sections = new List<SectionInfo>();
        private int _sectionIndex = -1;
        private int _bytesPerSecond;

        private int _percentProgress;

        public enum SectionType
        {
            kAudio,
            kSilence,
            kBeep
        };

        public int Sections
        {
            get { return _sections.Count; }
        }

        public class SectionInfo
        {
            public SectionInfo(int inStart, int inDuration)
            {
                startIndex = inStart;
                durationBytes = inDuration;
                type = SectionType.kAudio;
                speed = 1f;
            }
            public int startIndex;
            public int durationBytes;
            public SectionType type;
            public float speed;
            public int outstandingBytes; 
        };

        const int   kMillisecondsPerSecond = 1000;
        const int   kBitsPerByte = 8;
        const int   kMaxBufferSizeMultiplier = 5; // support down to 0.2 speed
        const int   kBytesPerFloat = 4;
        const int   kBeepFrequencyHz = 2000;
        const float kBeepVolume = 0.5f;

        public SectionedSampleProvider(AudioFileReader reader, int readDurationMilliseconds, SoundTouchProfile soundTouchProfile)
        {
            _soundTouch = new SoundTouch();

            SetSoundTouchProfile(soundTouchProfile);
            _reader = reader;
            _sourceProvider = reader;
            _soundTouch.SetSampleRate(WaveFormat.SampleRate);
            _channels = WaveFormat.Channels;
            _soundTouch.SetChannels(_channels);
            _bytesPerSecond = WaveFormat.SampleRate * _channels * (WaveFormat.BitsPerSample / kBitsPerByte);
            _sourceReadBuffer = new float[(WaveFormat.SampleRate * _channels * (long)readDurationMilliseconds) / kMillisecondsPerSecond];
            _soundTouchReadBuffer = new float[_sourceReadBuffer.Length * kMaxBufferSizeMultiplier]; // support down to 0.2 speed

            _reader.Seek(0, System.IO.SeekOrigin.Begin);
            _percentProgress = 0;
        }

        public void AddSectionAudio(TimeSpan start, TimeSpan duration, float speed)
        {
            if (start + duration > _reader.TotalTime)
            {
                //Auto cut to fit
                if (start >= _reader.TotalTime)
                {
                    throw new ArgumentOutOfRangeException("Section is entirely out of range for this audio file");
                }
                else
                {
                    duration = _reader.TotalTime - start;
                }
            }

            SectionInfo current = new SectionInfo(TimeSpanToOffset(start), TimeSpanToOffset(duration));
            current.type = SectionType.kAudio;
            current.speed = speed;
            current.outstandingBytes = (int)(current.durationBytes / speed);
            
            _sections.Add(current);
        }

        public void AddSectionSilence(TimeSpan duration)
        {
            SectionInfo current = new SectionInfo(0, TimeSpanToOffset(duration));
            current.type = SectionType.kSilence;
            current.outstandingBytes = current.durationBytes;
            _sections.Add(current);
        }

        public void AddSectionBeep(TimeSpan duration)
        {
            SectionInfo current = new SectionInfo(0, TimeSpanToOffset(duration));
            current.type = SectionType.kBeep;
            current.outstandingBytes = current.durationBytes;
            _sections.Add(current);
        }

        public void DropLastSection()
        {
            if (_sections.Count > 0)
            {
                _sections.RemoveAt(_sections.Count - 1);
            }
        }

        public int TimeSpanToOffset(TimeSpan ts)
        {
            int bytes = (int)(_bytesPerSecond * (ts.TotalMilliseconds / kMillisecondsPerSecond));
            bytes -= (bytes % WaveFormat.BlockAlign);
            return bytes;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int progressUpdatePerSection = 100 / _sections.Count;
            UpdateProgress(_percentProgress);

            int samplesRead = 0;
            while (samplesRead < count && _sectionIndex < _sections.Count)
            {
                if (_sectionIndex < 0)
                {
                    if (!SelectNewSection())
                        return samplesRead;
                }
                var fromThisSegment = ReadFromCurrentSection(buffer, offset + samplesRead, count - samplesRead);
                samplesRead += fromThisSegment;

                if (fromThisSegment == 0)
                {
                    bool newSection = SelectNewSection();
                    if (newSection)
                    {
                        _percentProgress += progressUpdatePerSection;
                        UpdateProgress(_percentProgress);
                    }
                    else
                        UpdateProgress(100);

                    if (!newSection || samplesRead > 0)
                        return samplesRead;
                }
            }
            return samplesRead;
        }

        private void UpdateProgress(int progressPercent)
        {
            ConfigureInput.InvokeIfRequired(Copying.Instance, delegate
            {
                Copying.Instance.ProgressBarValue = progressPercent;
            });
        }

        private int ReadFromCurrentSection(float[] buffer, int offset, int count)
        {
            SectionInfo current = _sections[_sectionIndex];

            if (current.type == SectionType.kAudio)
            {
                var samplesRequired = Math.Min(current.outstandingBytes / kBytesPerFloat, count);

                int samplesRead = ReadAudio(buffer, offset, samplesRequired);
                current.outstandingBytes -= samplesRead * kBytesPerFloat;

                return samplesRead;
            }
            else if (current.type == SectionType.kBeep)
            {
                double currentSineWavePoint = 0.0;
                double sineWaveIncrement = (1.0 / WaveFormat.SampleRate) * kBeepFrequencyHz;

                int readSamples = 0;
                for (int i = 0; i < count && current.outstandingBytes > 0; i++)
                {
                    buffer[i] = (float)Math.Sin(currentSineWavePoint) * kBeepVolume;
                    currentSineWavePoint += sineWaveIncrement;
                    readSamples++;
                    current.outstandingBytes -= kBytesPerFloat;
                }

                return readSamples;
            }
            else // (current.type == SectionType.kSilence)
            {
                int readSamples = 0;
                for (int i = 0; i < count && current.outstandingBytes > 0; i++)
                {
                    buffer[i] = 0f;
                    readSamples++;
                    current.outstandingBytes -= kBytesPerFloat;
                }

                return readSamples;
            }
        }

        public int ReadAudio(float[] buffer, int offset, int count)
        {
            if (_playbackRate == 0) // play silence
            {
                for (int n = 0; n < count; n++)
                {
                    buffer[offset++] = 0;
                }
                return count;
            }

            if (_repositionRequested)
            {
                _soundTouch.Clear();
                _repositionRequested = false;
            }

            int samplesRead = 0;
            bool reachedEndOfSource = false;
            while (samplesRead < count)
            {
                if (_soundTouch.NumberOfSamplesAvailable == 0)
                {
                    var readFromSource = _sourceProvider.Read(_sourceReadBuffer, 0, _sourceReadBuffer.Length);
                    if (readFromSource > 0)
                        _soundTouch.PutSamples(_sourceReadBuffer, readFromSource / _channels);
                    else
                    {
                        reachedEndOfSource = true;
                        // we've reached the end, tell SoundTouch we're done
                        _soundTouch.Flush();
                    }
                }
               
                var desiredSampleFrames = (count - samplesRead) / _channels;

                if (desiredSampleFrames == 0) // Sometimes we have off by 1 issues
                    return count;
                    //desiredSampleFrames = count - samplesRead;

                var received = _soundTouch.ReceiveSamples(_soundTouchReadBuffer, desiredSampleFrames) * _channels;
                // use loop instead of Array.Copy due to WaveBuffer
                for (int n = 0; n < received; n++)
                {
                    buffer[offset + samplesRead++] = _soundTouchReadBuffer[n];
                }
                if (received == 0 && reachedEndOfSource) break;
            }

            if (reachedEndOfSource)
                return 0;
            else
                return samplesRead;
        }

        public WaveFormat WaveFormat
        {
            get { return _sourceProvider.WaveFormat; }
        }

        public float PlaybackRate
        {
            get
            {
                return _playbackRate;
            }
            set
            {
                if (_playbackRate != value)
                {
                    UpdatePlaybackRate(value);
                    _playbackRate = value;
                }
            }
        }

        private void UpdatePlaybackRate(float value)
        {
            if (value != 0)
            {
                if (_soundTouchProfile.UseTempo)
                {
                    _soundTouch.SetTempo(value);
                }
                else
                {
                    _soundTouch.SetRate(value);
                }
            }
        }

        public void Dispose()
        {
            _soundTouch.Dispose();
        }

        public void SetSoundTouchProfile(SoundTouchProfile soundTouchProfile)
        {
            if (_soundTouchProfile != null &&
                _playbackRate != 1.0f &&
                soundTouchProfile.UseTempo != _soundTouchProfile.UseTempo)
            {
                if (soundTouchProfile.UseTempo)
                {
                    _soundTouch.SetRate(1.0f);
                    _soundTouch.SetPitchOctaves(0f);
                    _soundTouch.SetTempo(_playbackRate);
                }
                else
                {
                    _soundTouch.SetTempo(1.0f);
                    _soundTouch.SetRate(_playbackRate);
                }
            }
            _soundTouchProfile = soundTouchProfile;
            _soundTouch.SetUseAntiAliasing(soundTouchProfile.UseAntiAliasing);
            _soundTouch.SetUseQuickSeek(soundTouchProfile.UseQuickSeek);
            _soundTouch.SetSequenceMS(soundTouchProfile.SequenceMS);
        }

        public void Reposition()
        {
            _repositionRequested = true;
        }

        private bool SelectNewSection()
        {
            _sectionIndex++;

            if (_sectionIndex < _sections.Count)
            {
                var current = _sections[_sectionIndex];

                if (current.type == SectionType.kAudio)
                {
                    _reader.Position = current.startIndex;
                    Reposition();
                    UpdatePlaybackRate(current.speed);
                }
                return true;
            }

            return false;
        }
    }
}