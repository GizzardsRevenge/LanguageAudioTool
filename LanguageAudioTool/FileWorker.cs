
using NAudio.Lame;
using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using VarispeedDemo.SoundTouch;

namespace LanguageAudioTool
{
    public class FileWorker
    {
        private List<AudioSection> _sections;
        private List<Job> _jobs; 
        private MusicFile _file;
        private FirstSectionBehaviour _firstSection;
        private string _outputPath;

        private static bool _useLameEncoding = false;

        public enum FirstSectionBehaviour
        {
            kHandleAsNormal,
            kOnceAtFullSpeed,
            kDelete
        }

        public FileWorker(List<AudioSection> sections, List<Job> jobs, MusicFile file, FirstSectionBehaviour firstSectionBehaviour, string newPath)
        {
            _sections = sections;
            _jobs = jobs;
            _file = file;
            _firstSection = firstSectionBehaviour;

            string filename = Path.GetFileName(file.Filename);
            _outputPath = Path.Combine(newPath, filename);
        }

        public void DoWork()
        {   
            // Load reader and sample provider
            using (AudioFileReader reader = new AudioFileReader(_file.Filename))
            {
                using (SectionedSampleProvider provider = new SectionedSampleProvider(reader, 100, new SoundTouchProfile(true, false)))
                {
                    bool firstSection = true;

                    // create the actual sections
                    foreach (AudioSection section in _sections)
                    {
                        TimeSpan duration = section.End - section.Start;

                        if (firstSection)
                        {
                            firstSection = false;
                            if (_firstSection == FirstSectionBehaviour.kDelete)
                                continue; // Ignore this section
                            else if (_firstSection == FirstSectionBehaviour.kOnceAtFullSpeed)
                            {
                                provider.AddSectionAudio(section.Start, duration, 1f);
                                provider.AddSectionSilence(new TimeSpan(0, 0, 1)); // Add 1 second silence
                                continue;
                            }
                            // Otherwise, drop down to treat as normal section...
                        }

                        foreach (Job job in _jobs)
                        {
                            if (job.JobType == Job.Type.AddSection)
                                provider.AddSectionAudio(section.Start, duration, (float)job.Parameter / 100f);
                            else // silence
                                provider.AddSectionSilence(new TimeSpan(0, 0, job.Parameter));
                        }
                    }

                    // Drop the last segment, if it is silence
                    if (_jobs[_jobs.Count - 1].JobType == Job.Type.AddSilence)
                    {
                        provider.DropLastSection();
                    }

                    // now encode to mp3
                    if (_useLameEncoding)
                        DoEncodeWindows7(provider);
                    else
                        DoEncodeWindows8(provider, 44100);

                }
            }
        }

        public static bool UseLameMp3Encoding
        {
            get { return _useLameEncoding; }
            set { _useLameEncoding = value; }
        }

        internal void DoEncodeWindows8 (SectionedSampleProvider provider, int sampleRate)
        {
            MediaFoundationEncoder.EncodeToMp3(provider.ToWaveProvider(), _outputPath, sampleRate);
        }

        internal void DoEncodeWindows7(SectionedSampleProvider provider)
        {
            using (var writer = new LameMP3FileWriter(_outputPath, provider.WaveFormat, LAMEPreset.STANDARD))
            {
                using (var waveStream = new SectionToWaveStream(provider))
                {
                    waveStream.CopyTo(writer);
                }
            }
        }
    }
}
