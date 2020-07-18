using System;

namespace LanguageAudioTool
{
    public class AudioSection
    {
        private TimeSpan _start;
        private TimeSpan _end;

        public AudioSection (TimeSpan start, TimeSpan end)
        {
            _start = start;
            _end = end;
        }

        public TimeSpan Start
        {
            get { return _start; }
        }

        public TimeSpan End
        {
            get { return _end; }
        }
    }
}
