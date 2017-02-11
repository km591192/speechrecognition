using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace speech_recognition
{
    class Speechtofile
    {

        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "Wave.wav";

       Form1 f11;
       public Speechtofile(Form1 f1)
      {
         f11 = f1;
      }
 

        public void start(object sender, EventArgs e)
        {
            try
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = 0;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
                // new EventArgs(waveIn_RecordingStopped);
               // waveIn.WaveFormat = new WaveFormat(/*44100*/16000, 1);
                waveIn.WaveFormat = new WaveFormat(16000, 1); //16bit mono pcm
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                waveIn.StartRecording();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        public void stop(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                StopRecording();
            }
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (f11.InvokeRequired)
            {
                f11.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
        }
        void StopRecording()
        {
            waveIn.StopRecording();
        }


        private void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if (f11.InvokeRequired)
            {
                f11.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }

    }
}
