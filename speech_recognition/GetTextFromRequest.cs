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

namespace speech_recognition
{
    class GetTextFromRequest
    {
        public string gettext(string resultstr)
        {
            string newwords = "";
           resultstr.Split('"');
            char[] cm = {'"','{',':','}',',','[',']','\n'};
           string[] words = resultstr.Split(cm,System.StringSplitOptions.RemoveEmptyEntries);

            string news = "";
           string p = words.Length.ToString();
           foreach (string s in words)
               news += s + "\n";            
          
            int wordslength = words.Length;
            if (wordslength - 4 != 5)
            {
                for (int i = 8; i <= wordslength - 4; i += 4)
                {
                   // MessageBox.Show(words[i]);
                    newwords += words[i] + "\n";
                }
                 
            }
           // MessageBox.Show(news);
           // MessageBox.Show(newwords);
           return newwords;
        }

        public void settext(string newwords,ListBox lb)
        {
           string[] words= newwords.Split('\n');
           if (newwords != null)
           {
               foreach (string s in words)
                   lb.Items.Add(s);
           }

      }


        private string str;
        public string getstr()
        {
            return this.str;
        }
        public void setstr(string thestr)
        {
            this.str = thestr;
        }
    }
}
