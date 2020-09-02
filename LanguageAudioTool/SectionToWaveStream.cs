using System;
using NAudio.Wave;

namespace LanguageAudioTool
{
    internal class SectionToWaveStream : WaveStream
    {
        private SectionedSampleProvider _sourceProvider;
        private float[] _floatBuffer;
        private WaveFormat _waveFormat;
        private long _floatLength;
        private long _byteLength;
        private long _totalBytesWritten = 0;

        const float kSecondsToRead = 0.2f;

        public SectionToWaveStream(SectionedSampleProvider provider)
        {
            _sourceProvider = provider;
            _waveFormat = provider.WaveFormat;

            _floatLength = (long)(_waveFormat.SampleRate * _waveFormat.Channels * kSecondsToRead);
            _byteLength = _floatLength * 4;

            _floatBuffer = new float[_floatLength];
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override long Position
        {
            get { return _totalBytesWritten; }
            set { throw new NotImplementedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int outstandingFloatsToRead = Math.Min(count / 4, (int)_floatLength); // floats 4 bytes 
            int byteIndex = 0;
            byte[] unwrapped;

            while (outstandingFloatsToRead > 0)
            {
                int floatsRead = _sourceProvider.Read(_floatBuffer, 0, outstandingFloatsToRead);
                outstandingFloatsToRead -= floatsRead;

                if (floatsRead == 0)
                {
                    _totalBytesWritten += byteIndex;
                    return byteIndex;
                }

                // Add to the byte buffer

                for (int i = 0; i < floatsRead; i++)
                {
                    unwrapped = BitConverter.GetBytes(_floatBuffer[i]);
                    buffer[offset + byteIndex++] = unwrapped[0];
                    buffer[offset + byteIndex++] = unwrapped[1];
                    buffer[offset + byteIndex++] = unwrapped[2];
                    buffer[offset + byteIndex++] = unwrapped[3];
                }
            }

            _totalBytesWritten += byteIndex;
            return byteIndex;
        }

        public override WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public override long Length
        {
            get { return _byteLength; }
        }
    }
}