using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
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

        public void DoWork(bool multiFiles)
        {
            if (multiFiles)
                DoWorkMultiFiles();
            else
                DoWorkSingleFile();
        }

        private void DoWorkMultiFiles()
        {
            int sectionID = 1;
            string filenameFirstBit = Path.Combine(Path.GetDirectoryName(_outputPath), Path.GetFileNameWithoutExtension(_outputPath));

            foreach (AudioSection section in _sections)
            {
                TimeSpan duration = section.End - section.Start;

                using (AudioFileReader reader = new AudioFileReader(_file.Filename))
                {
                    using (SectionedSampleProvider provider = new SectionedSampleProvider(reader, 100, new SoundTouchProfile(true, false, false, 50)))
                    {
                        foreach (Job job in _jobs)
                        {
                            if (job.JobType == Job.Type.AddSection)
                                provider.AddSectionAudio(section.Start, duration, (float)job.Parameter / 100f);
                            else if (job.JobType == Job.Type.AddBeep)
                            {
                                provider.AddSectionSilence(new TimeSpan(0, 0, 0, 0, 500));
                                provider.AddSectionBeep(new TimeSpan(0, 0, 0, 0, job.Parameter));
                                provider.AddSectionSilence(new TimeSpan(0, 0, 0, 0, 500));
                            }
                            else // silence
                                provider.AddSectionSilence(new TimeSpan(0, 0, job.Parameter));
                        }

                        // Drop the last segment, if it is silence
                        if (_jobs[_jobs.Count - 1].JobType == Job.Type.AddSilence)
                        {
                            provider.DropLastSection();
                        }
                        else if (_jobs[_jobs.Count - 1].JobType == Job.Type.AddBeep)
                        {
                            provider.DropLastSection(); // drop silence buffer
                            provider.DropLastSection(); // drop the beep
                            provider.DropLastSection(); // drop silence buffer
                        }

                        // Sanity check
                        if (provider.Sections == 0)
                        {
                            string message = String.Format("This file will be ignored, as it could not be processed into audio sections:{0}{1}", Environment.NewLine, _file.Filename);
                            MessageBox.Show(message, "Bad file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        // now encode to mp3
                        string generatedFilename = filenameFirstBit + sectionID.ToString().PadLeft(3, '0') + ".mp3";
                        sectionID++;

                        if (_useLameEncoding)
                            DoEncodeWindows7(provider, generatedFilename);
                        else
                            DoEncodeWindows8(provider, 44100, generatedFilename);
                    }
                }
            }
        }

        private void DoWorkSingleFile()
        { 
            // Load reader and sample provider
            using (AudioFileReader reader = new AudioFileReader(_file.Filename))
            {
                using (SectionedSampleProvider provider = new SectionedSampleProvider(reader, 100, new SoundTouchProfile(true, false, false, 50)))
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
                            else if (job.JobType == Job.Type.AddBeep)
                            {
                                provider.AddSectionSilence(new TimeSpan(0, 0, 0, 0, 500));
                                provider.AddSectionBeep(new TimeSpan(0, 0, 0, 0, job.Parameter));
                                provider.AddSectionSilence(new TimeSpan(0, 0, 0, 0, 500));
                            }
                            else // silence
                                provider.AddSectionSilence(new TimeSpan(0, 0, job.Parameter));
                        }
                    }

                    // Drop the last segment, if it is silence
                    if (_jobs[_jobs.Count - 1].JobType == Job.Type.AddSilence)
                    {
                        provider.DropLastSection();
                    }
                    else if (_jobs[_jobs.Count - 1].JobType == Job.Type.AddBeep)
                    {
                        provider.DropLastSection(); // drop silence buffer
                        provider.DropLastSection(); // drop the beep
                        provider.DropLastSection(); // drop silence buffer
                    }

                    // Sanity check
                    if (provider.Sections == 0)
                    {
                        string message = String.Format("This file will be ignored, as it could not be processed into audio sections:{0}{1}", Environment.NewLine, _file.Filename);
                        MessageBox.Show(message, "Bad file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // now encode to mp3
                    if (_useLameEncoding)
                        DoEncodeWindows7(provider, _outputPath);
                    else
                        DoEncodeWindows8(provider, 44100, _outputPath);

                }
            }
        }

        public static bool UseLameMp3Encoding
        {
            get { return _useLameEncoding; }
            set { _useLameEncoding = value; }
        }

        internal void DoEncodeWindows8 (SectionedSampleProvider provider, int sampleRate, string path)
        {
            MediaFoundationEncoder.EncodeToMp3(provider.ToWaveProvider(), path, sampleRate);
        }

        internal void DoEncodeWindows7(SectionedSampleProvider provider, string path)
        {
            using (var writer = new LameMP3FileWriter(path, provider.WaveFormat, LAMEPreset.STANDARD))
            {
                using (var waveStream = new SectionToWaveStream(provider))
                {
                    waveStream.CopyTo(writer);
                }
            }
        }
    }
}
