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

        public enum SectionDefinition
        {
            kSeparatedBySilence = 0,
            kByDuration,
            kWholeFile
        }

        public MusicFile(string filename)
        {
            _filename = filename;
        }

        public int ProcessIntoSections(SectionDefinition def, int sectionDuration)
        {
            switch (def)
            {
                case SectionDefinition.kSeparatedBySilence: ProcessIntoSectionsSilence(); break;
                case SectionDefinition.kByDuration: ProcessIntoSectionDuration(sectionDuration); break;
                case SectionDefinition.kWholeFile: ProcessIntoSectionWholeFile(); break;
            }

            return _sections.Count;
        }

        private void ProcessIntoSectionDuration(int secondsDurationPerSection)
        {
            _sections = new List<AudioSection>();

            using (Mp3FileReader reader = new Mp3FileReader(_filename))
            {
                int totalSeconds = (int)reader.TotalTime.TotalSeconds;

                for (int i = 0; i <= totalSeconds; i += secondsDurationPerSection)
                {
                    AudioSection section = new AudioSection(new TimeSpan(0, 0, i), new TimeSpan(0, 0, i + secondsDurationPerSection));
                    _sections.Add(section);
                }
            }
        }

        private void ProcessIntoSectionWholeFile()
        {
            _sections = new List<AudioSection>();

            using (Mp3FileReader reader = new Mp3FileReader(_filename))
            {
                AudioSection section = new AudioSection(new TimeSpan(0, 0, 0), reader.TotalTime);
                _sections.Add(section);
            }
        }

        private void ProcessIntoSectionsSilence()
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

        public bool CheckIsValid()
        {
            if (!String.IsNullOrWhiteSpace(_filename))
            {
                try
                {
                    // Just try to open it with Mp3FileReader
                    Mp3FileReader reader = new Mp3FileReader(_filename);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
