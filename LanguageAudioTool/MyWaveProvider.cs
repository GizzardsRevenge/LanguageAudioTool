using System;
using System.Collections.Generic;
using NAudio.Wave;
using VarispeedDemo.SoundTouch;

namespace LanguageAudioTool
{
    class MyWaveProvider : ISampleProvider, IDisposable
    {
        private readonly AudioFileReader _reader;
        private readonly ISampleProvider _sourceProvider;
        private readonly SoundTouch _soundTouch;
        private readonly float[] _sourceReadBuffer;
        private readonly float[] _soundTouchReadBuffer;
        private readonly int _channelCount;
        private float _playbackRate = 1.0f;
        private SoundTouchProfile _currentSoundTouchProfile;
        private bool _repositionRequested;
        private readonly List<SegmentInfo> _segments = new List<SegmentInfo>();
        private int _segmentIndex = -1;
        private int _bytesPerSecond;

        private System.IO.StreamWriter _debugFile; // DEBUG

        public enum SegmentType
        {
            kAudio,
            kSilence,
            kDEBUG
        };

        public class SegmentInfo
        {
            public SegmentInfo(int inStart, int inDuration)
            {
                startIndex = inStart;
                durationBytes = inDuration;
                type = SegmentType.kAudio;
                speed = 1f;
            }
            public int startIndex;
            public int durationBytes;
            public SegmentType type;
            public float speed;
            public int outstanding; //DEBUG?
        };

        public MyWaveProvider(AudioFileReader reader, int readDurationMilliseconds, SoundTouchProfile soundTouchProfile)
        {
            _soundTouch = new SoundTouch();

            SetSoundTouchProfile(soundTouchProfile);
            _reader = reader;
            _sourceProvider = reader;
            _soundTouch.SetSampleRate(WaveFormat.SampleRate);
            _channelCount = WaveFormat.Channels;
            _soundTouch.SetChannels(_channelCount);
            _bytesPerSecond = WaveFormat.SampleRate * _channelCount * (WaveFormat.BitsPerSample / 8);
            _sourceReadBuffer = new float[(WaveFormat.SampleRate * _channelCount * (long)readDurationMilliseconds) / 1000];
            _soundTouchReadBuffer = new float[_sourceReadBuffer.Length * 5]; // support down to 0.2 speed

            _reader.Seek(0, System.IO.SeekOrigin.Begin);

            _debugFile = new System.IO.StreamWriter(@"d:\SCRATCH\debugWavePro.txt", false);
        }

        public void AddSegmentAudio(TimeSpan start, TimeSpan duration, float speed)
        {
            SegmentInfo current = new SegmentInfo(TimeSpanToOffset(start), TimeSpanToOffset(duration));
            current.type = SegmentType.kAudio;
            current.speed = speed;
            //current.outstanding = current.durationBytes;

            //DEBUG
            current.outstanding = (int)(current.durationBytes / speed);

            if (start + duration > _reader.TotalTime)
                throw new ArgumentOutOfRangeException("Segment goes beyond end of input");
            _segments.Add(current);

            //AddSegmentDEBUG();
        }

        public void AddSegmentSilence(TimeSpan duration)
        {
            SegmentInfo current = new SegmentInfo(0, TimeSpanToOffset(duration));
            current.type = SegmentType.kSilence;
            current.outstanding = current.durationBytes;
            _segments.Add(current);

            //AddSegmentDEBUG();
        }

        public void DropLastSegment()
        {
            if (_segments.Count > 0)
            {
                _segments.RemoveAt(_segments.Count - 1);
            }
        }

        private void AddSegmentDEBUG()
        {
            SegmentInfo current = new SegmentInfo(0, TimeSpanToOffset(new TimeSpan(0, 0, 1)));
            current.type = SegmentType.kDEBUG;
            current.outstanding = current.durationBytes;
            _segments.Add(current);
        }

        public int TimeSpanToOffset(TimeSpan ts)
        {
            int bytes = (int)(_bytesPerSecond * (ts.TotalMilliseconds / 1000f));
            //var bytes = (int)(WaveFormat.AverageBytesPerSecond * ts.TotalSeconds);
            bytes -= (bytes % WaveFormat.BlockAlign);
            return bytes;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int bytesRead = 0;
            while (bytesRead < count && _segmentIndex < _segments.Count)
            {
                if (_segmentIndex < 0)
                {
                    if (!SelectNewSegment())
                        return bytesRead;
                }
                var fromThisSegment = ReadFromCurrentSegment(buffer, offset + bytesRead, count - bytesRead);
                bytesRead += fromThisSegment;

                if (fromThisSegment == 0)
                {
                    if (!SelectNewSegment() || bytesRead > 0)
                        return bytesRead;
                }
            }
            return bytesRead;
        }

        private int ReadFromCurrentSegment(float[] buffer, int offset, int count)
        {
            SegmentInfo current = _segments[_segmentIndex];

            if (current.type == SegmentType.kAudio)
            {
                //var bytesAvailable = (int)(current.startIndex + current.durationBytes - _reader.Position); // start + length - position
                //var bytesRequired = Math.Min(bytesAvailable, count);
                //var bytesRequired = Math.Min(bytesAvailable, count);

                var bytesRequired = Math.Min(current.outstanding / 4, count);

                int bytesRead = ReadAudio(buffer, offset, bytesRequired);
                current.outstanding -= bytesRead * 4;

                _debugFile.WriteLine("A" + _segmentIndex + ": " + bytesRead + " bytes");

                return bytesRead;
            }
            else if (current.type == SegmentType.kSilence)
            {
                int readBytes = 0;
                for (int i = 0; i < count && _segments[_segmentIndex].durationBytes > 0; i++)
                {
                    buffer[i] = 0f; 
                    readBytes++;
                    current.durationBytes -= 4; // 4 bytes per float
                    current.outstanding -= 4;
                }

                _debugFile.WriteLine("S" + _segmentIndex + ": " + readBytes + " bytes");

                return readBytes;
            }
            else // DEBUG
            {
                int readBytes = 0;
                for (int i = 0; i < count && _segments[_segmentIndex].durationBytes > 0; i++)
                {
                    if (i % 100 < 50)
                        buffer[i] = .25f;
                    else
                        buffer[i] = -.25f;
                    readBytes++;
                    current.durationBytes--;
                    current.outstanding--;
                }
                return readBytes;
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
                        _soundTouch.PutSamples(_sourceReadBuffer, readFromSource / _channelCount);
                    else
                    {
                        reachedEndOfSource = true;
                        // we've reached the end, tell SoundTouch we're done
                        _soundTouch.Flush();
                    }
                }
               
                var desiredSampleFrames = (count - samplesRead) / _channelCount;

                var received = _soundTouch.ReceiveSamples(_soundTouchReadBuffer, desiredSampleFrames) * _channelCount;
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
                if (_currentSoundTouchProfile.UseTempo)
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
            _debugFile.Flush();
            _debugFile.Close();
            _debugFile.Dispose();
            _soundTouch.Dispose();
        }

        public void SetSoundTouchProfile(SoundTouchProfile soundTouchProfile)
        {
            if (_currentSoundTouchProfile != null &&
                _playbackRate != 1.0f &&
                soundTouchProfile.UseTempo != _currentSoundTouchProfile.UseTempo)
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
            _currentSoundTouchProfile = soundTouchProfile;
            _soundTouch.SetUseAntiAliasing(soundTouchProfile.UseAntiAliasing);
            _soundTouch.SetUseQuickSeek(soundTouchProfile.UseQuickSeek);
        }

        public void Reposition()
        {
            _repositionRequested = true;
        }

        private bool SelectNewSegment()
        {
            _segmentIndex++;

            //if (_segmentIndex == -1)
            //    _segmentIndex = 0;
            //else
            //{
            //    return false;
            //}

            if (_segmentIndex < _segments.Count)
            {
                var current = _segments[_segmentIndex];

                _debugFile.WriteLine(String.Format("Grabbing new segment, type {0}, duration {1}", current.type, current.durationBytes));
                //_soundTouch.Clear();
                //_soundTouch.Flush();

                if (current.type == SegmentType.kAudio)
                {
                    _reader.Position = current.startIndex;
                    Reposition();
                    //_soundTouch.Clear();
                    UpdatePlaybackRate(current.speed);
                }
                return true;
            }

            //int seg = 0;
            //foreach (SegmentInfo info in _segments)
            //{
            //    seg++;
            //}

            return false;
        }
    }
}