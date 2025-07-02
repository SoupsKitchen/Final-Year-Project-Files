using UnityEngine;
using System;
using System.IO;

public static class WavUtility
{
    const int HEADER_SIZE = 44;

    public static byte[] FromAudioClip(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();
        const int sampleCount = 0;
        const int frequency = 16000;

        byte[] header = new byte[HEADER_SIZE];

        int hz = clip.frequency;
        int channels = clip.channels;
        float[] samples = new float[clip.samples * clip.channels];

        clip.GetData(samples, 0);

        short[] intData = new short[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        int byteRate = hz * channels * 2;

        MemoryStream memoryStream = new MemoryStream();
        memoryStream.Position = 0;

        // Chunk ID "RIFF"
        memoryStream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(HEADER_SIZE + bytesData.Length - 8), 0, 4);
        memoryStream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
        memoryStream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(16), 0, 4); // Subchunk1Size (16 for PCM)
        memoryStream.Write(BitConverter.GetBytes((ushort)1), 0, 2); // AudioFormat (1 for PCM)
        memoryStream.Write(BitConverter.GetBytes((ushort)channels), 0, 2);
        memoryStream.Write(BitConverter.GetBytes(hz), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(byteRate), 0, 4);
        memoryStream.Write(BitConverter.GetBytes((ushort)(channels * 2)), 0, 2);
        memoryStream.Write(BitConverter.GetBytes((ushort)16), 0, 2); // BitsPerSample
        memoryStream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(bytesData.Length), 0, 4);
        memoryStream.Write(bytesData, 0, bytesData.Length);

        byte[] bytes = memoryStream.ToArray();

        return bytes;
    }
}