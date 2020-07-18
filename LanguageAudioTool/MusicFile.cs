using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace LanguageAudioTool
{
    public class MusicFile
    {
        private List<AudioSection> _sections;
        private string _filename = string.Empty;

        public MusicFile(string filename)
        {
            _filename = filename;
        }

        public int ProcessIntoSections()
        {
            _sections = new List<AudioSection>();

            using (Mp3FileReader reader = new Mp3FileReader(_filename))
            {
                int totalBytesPerSecond = (reader.WaveFormat.SampleRate * reader.WaveFormat.Channels * 2);
                float totalBytesPerMillisecond = (float)totalBytesPerSecond / 1000f;
                List<SoundChunk> silences = SilenceDetector.DetectSilence(reader, 3);
                List<SoundChunk> audios = SilenceDetector.InvertToAudio(silences, (int)reader.Length, totalBytesPerSecond);

                foreach (SoundChunk chunk in audios)
                {
                    TimeSpan start = new TimeSpan(0, 0, 0, 0, (int)(chunk.startIndex / totalBytesPerMillisecond));
                    TimeSpan end = new TimeSpan(0, 0, 0, 0, (int)(chunk.endIndex / totalBytesPerMillisecond));
                    AudioSection section = new AudioSection(start, end);
                    _sections.Add(section);
                }
            }

            return _sections.Count;
        }

        public override string ToString()
        {
            return _filename;
        }

        public string Filename
        {
            get { return _filename; }
        }

        public List<AudioSection> Sections
        {
            get { return _sections; }
        }
    }
}
