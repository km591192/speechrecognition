using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;
using System.Threading;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;

namespace speech_recognition
{
    class Reguest
    {
        GetTextFromRequest gtfr = new GetTextFromRequest();
        RusEnVoice rev = new RusEnVoice();
        
        public bool HasConnection()
            {
                try
                {
                    System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                    return true;
                }
                catch
                {
                    MessageBox.Show("Check your network connection or choose another mode.");
                    return false;
                }
            }


        public string GetResult(ListBox lb)
        {
            string results = "";
            String text = "";
            FileStream fileStream = File.OpenRead("Wave.flac"); 
            if (HasConnection() == true)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream();
                    memoryStream.SetLength(fileStream.Length);
                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                    byte[] BA_AudioFile = memoryStream.GetBuffer();
                    HttpWebRequest _HWR_SpeechToText = (HttpWebRequest)WebRequest.Create(
                        "https://www.google.com/speech-api/v2/recognize?output=json&lang=ru-RU&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
                        //"https://www.google.com/speech-api/v2/recognize?output=json&lang=en-us&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
                    _HWR_SpeechToText.Credentials = CredentialCache.DefaultCredentials;
                    _HWR_SpeechToText.Method = "POST";
                    _HWR_SpeechToText.ContentType = "audio/x-flac; rate=16000"; //44100
                    _HWR_SpeechToText.ContentLength = BA_AudioFile.Length;

                    Stream stream = _HWR_SpeechToText.GetRequestStream();
                    stream.Write(BA_AudioFile, 0, BA_AudioFile.Length);
                    stream.Close();

                    HttpWebResponse HWR_Response = (HttpWebResponse)_HWR_SpeechToText.GetResponse();
                    if (HWR_Response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader SR_Response = new StreamReader(HWR_Response.GetResponseStream());
                        results = SR_Response.ReadToEnd();
                    }

                    String[] jsons = results.Split('\n');
                    foreach (String j in jsons)
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(j);
                        if (jsonObject == null || jsonObject.result.Count <= 0)
                        {
                            continue;
                        }
                        text = jsonObject.result[0].alternative[0].transcript;
                    }
                    string str = gtfr.gettext(results);
                    gtfr.settext(str,lb);

                    memoryStream.Dispose();
                    memoryStream.Close();
                    fileStream.Dispose();
                    fileStream.Close();
                }
                
                catch (Exception ex)
                {

                    fileStream.Dispose();
                    fileStream.Close();
                  //  MessageBox.Show(ex.ToString());
                    MessageBox.Show("No connect to server. Try again later");
                }
            }
            fileStream.Dispose();
            fileStream.Close();
                return text;
            }
        public string tts_yandex_url(string text)
        {
            string key = "85cf18b2-3bb6-4b0b-86e4-bc1271d884d8";
            return "https://tts.voicetech.yandex.net/generate?text=" + text + "&format=mp3&lang=ru-RU&speaker=jane&key=" + key;
        }
        public string tts_google_url(string text)
        {
            return "https://translate.google.com/translate_tts?tl=en-gb&q=" + rev.rus_en_v(text.ToString()) + "&client=tw-ob";
        }

        public void text_speech(string text)
        {
            if (HasConnection() == true)
            {
                try
                {
                    if (text.Length < 2)
                        return;
                    var playThread = new Thread(() => PlayFromUrl(tts_yandex_url(text)));    
                    /*var playThread = new Thread(() => PlayFromUrl(tts_google_url(text)));*/

                    playThread.IsBackground = true;
                    playThread.Start();
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.ToString());
                }
            }
            else MessageBox.Show("No connect to server. Try again later");
        }

    AutoResetEvent stop = new AutoResetEvent(false);
    private void PlayFromUrl(string url)
    {
        try 
        {
            bool waiting;
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url).GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.PlaybackStopped += (sender, e) =>
                        {
                            waveOut.Stop();
                        };

                        waveOut.Play();
                        waiting = true;
                        stop.WaitOne(10000);
                        waiting = false;
                    }
                }
            }
        }
            catch(Exception ex)
        { MessageBox.Show(ex.ToString()); }
    }
}
    }
