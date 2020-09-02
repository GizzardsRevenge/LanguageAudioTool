namespace VarispeedDemo.SoundTouch
{
    internal class SoundTouchProfile
    {
        public bool UseTempo { get; set; }
        public bool UseAntiAliasing { get; set; }
        public bool UseQuickSeek { get; set; }
        public int SequenceMS { get; set; }

        // Note "quick seek" is not compatible with very short sequenceMS (will crash)
        public SoundTouchProfile(bool useTempo, bool useAntiAliasing, bool quickSeek, int sequenceMs)
        {
            UseTempo = useTempo;
            UseAntiAliasing = useAntiAliasing;
            UseQuickSeek = quickSeek;
            SequenceMS = sequenceMs;
        }
    }
}