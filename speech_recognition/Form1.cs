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
    public partial class Form1 : Form
    {
        Speechtofile stf;
        Reguest reg = new Reguest();
        WavToFlac wtf = new WavToFlac();
        GetTextFromRequest gtfr = new GetTextFromRequest();
        public Form1()
        {
            InitializeComponent();
            stf = new Speechtofile(this);
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            stf.start(sender, e);
            label1.Text = "Start speak.";
            label1.ForeColor = System.Drawing.Color.Green;
            listBox1.Items.Clear();

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stf.stop(sender, e);
            label1.Text = "Stop speak.";
            label1.ForeColor = System.Drawing.Color.Red;
            
           button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wtf.Wav_Flac();
            label1.Text = "";
            listBox1.Items.Clear();
            richTextBox1.AppendText("You: " + reg.GetResult(listBox1) + "\n");


            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = true;
        }

        private void writeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Enabled = true;
            button4.Enabled = true;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("You: " + textBox1.Text.ToString() + "\n");

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = false;
        }

        private void voiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > -1)
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    richTextBox1.AppendText("You: " + listBox1.SelectedItem.ToString() + "\n");
                }
                else
                    MessageBox.Show("Choose one item.");
                listBox1.Items.Clear();
            }
        }


    }
}
