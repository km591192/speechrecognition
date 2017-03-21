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
    }
    }

