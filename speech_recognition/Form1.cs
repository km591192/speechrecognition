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
        QA qa = new QA();
        SpeechSynthesizer synth = new SpeechSynthesizer();
        RusEnVoice rev = new RusEnVoice();

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
            string strreg = reg.GetResult(listBox1);
            richTextBox1.AppendText("You: " + strreg + "\n");
            string strans = String.Empty;
            if (strreg != null)
            strans = qa.create_answer(strreg);
            richTextBox1.AppendText("Me: " + strans + "\n");

            tts1(strans);

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
            button4.Enabled = false;
            richTextBox1.AppendText("You: " + textBox1.Text.ToString() + "\n");
            string answers = qa.create_answer( textBox1.Text.Trim().ToString());
            richTextBox1.AppendText("Me: " + answers + "\n");

          //  tts1(answers);

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
                    richTextBox1.AppendText("Me: " + qa.create_answer(listBox1.SelectedItem.ToString()) + "\n");
                    string str = rev.rus_en_v(qa.create_answer(listBox1.SelectedItem.ToString())).ToString();
                    tts(str);
                }
                else
                    MessageBox.Show("Choose one item.");
                listBox1.Items.Clear();
            }
        }

        private void tts(string str)
        {
            if (reg.HasConnection())
            {
                string str1 = " ";
                for (int i = 0; i < str.Length; i += 70)
                {
                    if (str.Length - i < 70)
                        str1 = str.Substring(i, str.Length - i);
                    else
                        str1 = str.Substring(i, 70);
                    reg.text_speech(str1.ToString());
                    System.Threading.Thread.Sleep(7500);
                }
            }
        }
        private void tts1(string str)
        {
            if (reg.HasConnection())
            {
                string str1 = " ";
                for (int i = 0; i < str.Length; i += 150)
                {
                    if (str.Length - i < 150)
                        str1 = str.Substring(i, str.Length - i);
                    else
                        str1 = str.Substring(i, 150);
                    reg.text_speech(str1.ToString());
                    System.Threading.Thread.Sleep(10500);
                }
            }
        }


    }
}
