using NAudio.Wave;
using System.Collections.Generic;

namespace LanguageAudioTool
{
    public struct SoundChunk
    {
        public int startIndex;
        public int endIndex;
        public bool isSilence;
        public float durationInSeconds;
    }

    public static class SilenceDetector
    {
        private const int kMicroChunksPerSecond = 20; // A microchunk is minimum size to classify as silence (e.g. half a microchunk of silence surrounded by audio doesn't count as silence)
        private const float kProportionSilenceThreshold = .6f;
        private const int kBytesPerSample = 2; // Always 16-bit for mp3

        // Classify every microchunk as containing sufficient silent bits or not
        // Then combine into longer chunks if adjacent chunks are the same type
        public static List<SoundChunk> DetectSilenceRaw(Mp3FileReader reader)
        {
            int totalBytesPerSecond = reader.WaveFormat.SampleRate * reader.WaveFormat.Channels * kBytesPerSample;
            List<SoundChunk> output = new List<SoundChunk>();

            var initial = new bool[(int)(reader.TotalTime.TotalSeconds * kMicroChunksPerSecond) + 1];
            var buffer = new byte[totalBytesPerSecond / kMicroChunksPerSecond];

            int absoluteSilenceBytesThreshold = (int)((totalBytesPerSecond / kMicroChunksPerSecond) * kProportionSilenceThreshold);

            int microChunksRead = 0;
            while (true)
            {
                int silenceBytes = 0;
                int samplesRead = reader.Read(buffer, 0, buffer.Length);
                if (samplesRead < buffer.Length)
                    break;

                for (int n = 0; n < samplesRead; n++)
                {
                    if (IsSilence(buffer[n]))
                        silenceBytes++;
                }

                initial[microChunksRead] = (silenceBytes >= absoluteSilenceBytesThreshold);
                microChunksRead++;
            }

            int bytesPerMicroChunk = buffer.Length;
            float durationPerMicroChunk = (float)buffer.Length / totalBytesPerSecond;
            SoundChunk current = new SoundChunk();
            current.startIndex = 0;
            current.endIndex = bytesPerMicroChunk;
            current.isSilence = initial[0];
            current.durationInSeconds = durationPerMicroChunk;

            for (int microChunkIndex = 1; microChunkIndex < microChunksRead; microChunkIndex++)
            {
                if (initial[microChunkIndex] == current.isSilence)
                {
                    current.endIndex += bytesPerMicroChunk;
                    current.durationInSeconds += durationPerMicroChunk;
                }
                else
                {
                    output.Add(current);
                    int lastEnd = current.endIndex;
                    current = new SoundChunk();
                    current.startIndex = lastEnd + 1;
                    current.endIndex = current.startIndex + bytesPerMicroChunk;
                    current.isSilence = initial[microChunkIndex];
                    current.durationInSeconds = (float)(current.endIndex - current.startIndex) / totalBytesPerSecond;
                }
            }

            // The very last chunk needs to be added to output
            output.Add(current);

            return output;
        }

        // Create a list of silences which are above the required threshold
        public static List<SoundChunk> DetectSilence(Mp3FileReader reader, int minimumSilenceSeconds)
        {
            List<SoundChunk> rawList = DetectSilenceRaw(reader);
            List<SoundChunk> silences = new List<SoundChunk>();

            for (int i = 0; i < rawList.Count; i++) 
            {
                SoundChunk chunk = rawList[i];
                if (chunk.isSilence)
                {
                    if (i == 0 || i == rawList.Count - 1 || chunk.durationInSeconds >= minimumSilenceSeconds)
                        silences.Add(chunk);
                }
            }

            return silences;
        }

        // Given a list of silences in a sound file, flip it to be a list of audio sections
        public static List<SoundChunk> InvertToAudio(List<SoundChunk> silences, int bytesInAudioFile, int bytesPerSecond)
        {
            List<SoundChunk> audio = new List<SoundChunk>();
            int currentStart = 0;

            foreach (SoundChunk chunk in silences)
            {
                SoundChunk audioChunk = new SoundChunk();
                audioChunk.startIndex = currentStart;
                audioChunk.endIndex = chunk.startIndex - 1;
                audioChunk.isSilence = false;

                // Sanity check
                if (audioChunk.endIndex - audioChunk.startIndex > bytesPerSecond)
                    audio.Add(audioChunk);

                currentStart = chunk.endIndex + 1;
            }

            // Now add final chunk after last silence
            SoundChunk lastSilence = silences[silences.Count - 1];

            if (bytesInAudioFile - lastSilence.endIndex > bytesPerSecond)
            {
                SoundChunk audioChunk = new SoundChunk();
                audioChunk.startIndex = lastSilence.endIndex + 1;
                audioChunk.endIndex = bytesInAudioFile - 1;
                audioChunk.isSilence = false;
                audio.Add(audioChunk);
            }

            return audio;
        }

        private static bool IsSilence(byte data)
        {
            return (data < 0x20 || data == 0xff); // 0xff is often MPEG artifact
        }
    }
}
