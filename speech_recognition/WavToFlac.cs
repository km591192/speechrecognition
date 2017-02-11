using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using FlacBox;

using CUETools.Codecs;
using CUETools.Codecs.FLAKE;

namespace speech_recognition
{
    class WavToFlac
    {

        public void Wav_Flac()
        {
            try
            {
                String wavName = "Wave.wav";
                string flacName = "Wave.flac";
                int sampleRate = 0;

                IAudioSource audioSource = new WAVReader(wavName, null);
                AudioBuffer buff = new AudioBuffer(audioSource, 0x10000);

                FlakeWriterSettings flake = new FlakeWriterSettings();
                flake.PCM = audioSource.PCM;
                FlakeWriter flakewriter = new FlakeWriter(flacName, flake);

                sampleRate = audioSource.PCM.SampleRate;

                FlakeWriter audioDest = flakewriter;
                while (audioSource.Read(buff, -1) != 0)
                {
                    audioDest.Write(buff);
                }

                audioDest.Close();
                flakewriter.Close();
                audioSource.Close();
            }
           catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        /*
        public void _Wav2Flac()
        {
            String wavName = "Wave.wav" ;
            string flacName = "Wave.flac";

            IAudioSource audioSource = new WAVReader(wavName, null);
            AudioBuffer buff = new AudioBuffer(audioSource, 0x10000);

           FlakeWriter flakewriter = new FlakeWriter(flacName, audioSource.PCM);

            FlakeWriter audioDest = flakewriter;
            while (audioSource.Read(buff, -1) != 0)
            {
                audioDest.Write(buff);
            }

            audioDest.Close();
            audioSource.Close();
        }
        */

        public void f()
        {
            string inputFile = "Wave.wav ";
            string outputFile = "Wave.flac";

            WaveFileReader wfr = new WaveFileReader(inputFile);
            var bitrate = 0;
            bitrate = wfr.WaveFormat.AverageBytesPerSecond * 8;

            var flacStream = File.Create(outputFile);
            FlacWriter flac = new FlacWriter(flacStream);

            byte[] buffer = new byte[4];
            int bytesRead;
            do
            {
                bytesRead = wfr.Read(buffer, 0, buffer.Length);
                flacStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);
            flac.Close();
            flacStream.Dispose();
            flacStream.Close();
            wfr.Dispose();
            wfr.Close();
            
        }

        public void convertfileformat2()
        {
            string inputFile = "Wave.wav ";
            string outputFile = "Wave.flac";

            WaveFileReader wfr = new WaveFileReader(inputFile);
            var bitrate = 0;
            bitrate = wfr.WaveFormat.AverageBytesPerSecond * 8;

            var flacStream = File.Create(outputFile);
            FlacWriter flac = new FlacWriter(flacStream);

            byte[] buffer = new byte[bitrate / 8];
            int bytesRead;
            do
            {
                bytesRead = wfr.Read(buffer, 0, buffer.Length);
                flacStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);
            flac.Close();
            flacStream.Dispose();
            flacStream.Close();
            wfr.Dispose();
            wfr.Close();
        }

        public void convertfileformat1()
        {
            string inputFile = "Wave.wav ";
        string outputFile = "Wave.flac";

        WaveFileReader wfr = new WaveFileReader(inputFile);
        var bitrate = 0;
            bitrate = wfr.WaveFormat.AverageBytesPerSecond * 8;

            FileStream fs = new FileStream("Wave.flac", FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    byte[] buffer = new byte[bitrate / 8];
                    int bytesRead;
                    do
                    {
                        bytesRead = wfr.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, bytesRead);
                    }
                    while (bytesRead > 0);

                    fs.Close();
                wfr.Close();
            
        }

        public void convertfileformat()
        {
         string inputFile = "Wave.wav ";
        string outputFile = "Wave.flac";

        StreamReader strreader = new StreamReader(inputFile);
        WaveFormat wf = new WaveFormat();
        WaveFileReader wfr = new WaveFileReader(inputFile);
        var bitrate = 0;
            bitrate = wfr.WaveFormat.AverageBytesPerSecond * 8;
        using (var flacStream = File.Create(outputFile))
        {
            FlacWriter flac = new FlacWriter(flacStream);
            byte[] buffer = new byte[bitrate / 8];
            int bytesRead;
            do
            {
                bytesRead = wfr.Read(buffer, 0, buffer.Length);
                flacStream.Write(buffer, 0, bytesRead);
            } 
            while (bytesRead > 0);
            flacStream.Dispose();
            flac.Close();
            flacStream.Close();
        }
        wfr.Dispose();
        wfr.Close();
        strreader.Dispose();
        strreader.Close();

       /* WaveFormat wf = new WaveFormat();
        WaveFileReader vavreader = new WaveFileReader(inputFile);
        WaveFileReader wavreader = new WaveFileReader(inputFile);
       using (var flacStream = File.Create(outputFile))
        {
            FlacWriter flac = new FlacWriter(flacStream, wavreader.BitDepth, wavreader.Channels, wavreader.SampleRate);
            byte[] buffer = new byte[wavreader.Bitrate / 8];
            int bytesRead;
            do
            {
                bytesRead = wavreader.InputStream.Read(buffer, 0, buffer.Length);
                flac.Convert(buffer, 0, bytesRead);
            } while (bytesRead > 0);
            flac.Dispose();
            flac = null;
        }

        wavreader.Dispose();
        wavreader = null;
       */
    }
    }
}
