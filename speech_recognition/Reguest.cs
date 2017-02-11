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

namespace speech_recognition
{
    class Reguest
    {
        GetTextFromRequest gtfr = new GetTextFromRequest();
        
        public string GetResult1()
        {
            string api_key = "AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw";
            string path = "Wavesaudio.flac";

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "audio/x-flac; rate=16000"); //44100
            byte[] result = client.UploadData(string.Format(
                        "https://www.google.com/speech-api/v2/recognize?client=chromium&lang=en-us&key={0}", api_key), "POST", bytes);

            string s = client.Encoding.GetString(result);
            return s;
        }

        public static bool HasConnection()
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

        public string GetResult2()
        {
            string results = "";
         try
            {
                FileStream fileStream = File.OpenRead("Wave4.wav");
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.SetLength(fileStream.Length);
                fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                byte[] BA_AudioFile = memoryStream.GetBuffer();
                HttpWebRequest _HWR_SpeechToText = (HttpWebRequest)WebRequest.Create(
                    "https://www.google.com/speech-api/v2/recognize?output=json&lang=en-us&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
                _HWR_SpeechToText.Credentials = CredentialCache.DefaultCredentials;
                _HWR_SpeechToText.Method = "POST";
                _HWR_SpeechToText.Headers.Add("Pragma", "no-cache");
                _HWR_SpeechToText.ContentType = "audio/x-wav;codec=pcm;bit=16;rate=16000"; //"audio/x-pcm;bit=16;rate=16000"   "audio/x-wav; rate=16000";  /audio/x-pcm;bit=16;rate=16000
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
                String text = "";
                foreach (String j in jsons)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject(j);
                    if (jsonObject == null || jsonObject.result.Count <= 0)
                    {
                        continue;
                    }
                    text = jsonObject.result[0].alternative[0].transcript;
                }
                MessageBox.Show(text);

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }
         return results; 
        }

        public string GetResult3()
        {
            string api_key = "AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw";
             string path = "Wave.mp3";

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "audio/mpeg; rate=16000"); //x-flac, x-mpeg-3,mpeg3, x-mpeg, x-mp3  // 44100 //codec=pcm;bit=16;
            byte[] result = client.UploadData(string.Format(
                        "https://www.google.com/speech-api/v2/recognize?client=chromium&lang=en-us&key={0}", api_key), "POST", bytes);

            string s = client.Encoding.GetString(result);
            return s;
        }

    }

}
