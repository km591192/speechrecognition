using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speech_recognition
{
    class RusEnVoice
    {
        public string rus_en_v(string str)
        {
            string strans = String.Empty;
            str = str.ToLower();
            var letters = str.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                strans += letters_voice(letters[i]);
            }
            return strans;
        }

        private string letters_voice (char c)
        {
            string str = "";
            if (c == '(') str = " ";
            if (c == ')') str = " ";
            if (c == ';') str = " ";
            if (c == '.') str = " ";
            if (c == ',') str = " ";
            if (c == ':') str = " ";
            if (c == ' ') str = " ";
            if (c == 'а') str = "a";
            if (c == 'ц') str = "tc";
            if (c == 'у') str = "u";
            if (c == 'к') str = "k";
            if (c == 'е') str = "e";
            if (c == 'н') str = "n";
            if (c == 'г') str = "g";
            if (c == 'щ') str = "shch";
            if (c == 'ш') str = "sh";
            if (c == 'з') str = "z";
            if (c == 'х') str = "h";
            if (c == 'ф') str = "ph";
            if (c == 'ы') str = "y";
            if (c == 'в') str = "v";
            if (c == 'п') str = "p";
            if (c == 'р') str = "r";
            if (c == 'о') str = "o";
            if (c == 'л') str = "l";
            if (c == 'д') str = "d";
            if (c == 'ж') str = "zh";
            if (c == 'э') str = "e";
            if (c == 'я') str = "ya";
            if (c == 'ч') str = "ch";
            if (c == 'с') str = "s";
            if (c == 'м') str = "m";
            if (c == 'й') str = "y";
            if (c == 'и') str = "i";
            if (c == 'т') str = "t";
            if (c == 'ь') str = "'";
            if (c == 'б') str = "b";
            if (c == 'ю') str = "u";
            if (c == '0') str = "nol'";
            if (c == '1') str = "odin";
            if (c == '2') str = "dva";
            if (c == '3') str = "tri'";
            if (c == '4') str = "chetyre'";
            if (c == '5') str = "piat";
            if (c == '6') str = "shest";
            if (c == '7') str = "sem'";
            if (c == '8') str = "vosem";
            if (c == '9') str = "devyat'";
            return str;
        }
    }
}
