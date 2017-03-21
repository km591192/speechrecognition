using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace speech_recognition
{
    class QA
    {

        SortedDictionary<string, string> ansdictionary = new SortedDictionary<string, string>();
        SortedDictionary<string, string> wwsansdictionary = new SortedDictionary<string, string>();
        SortedDictionary<string, string> fsansdictionary = new SortedDictionary<string, string>();
        public WordStemming ws = new WordStemming();
        List<string> keys = new List<string>();

        NBCClass nbcc = new NBCClass();

        private SortedDictionary<string, string> returndic()
        { return wwsansdictionary; }

        public string keystring(string s)
        {
            string ans = "";
            int i = 0, j = s.Length;
            while (i < j && s[i] != '/')
            { ans += s[i]; i++; }
            //  ans = ws.Stemword(ans);
            return ans;
        }
        public string wwskeystring(string s)
        {
            string ans = "";
            int i = 0, j = s.Length;
            while (i < j && s[i] != '/')
            { ans += s[i]; i++; }
            return ans;
        }
        public static string getfilename(string s)
        {
            string ans = "";
            int i = 0, j = s.Length;
            while (i < j && s[i] != '.')
            { ans += s[i]; i++; }
            return ans;
        }
        public static string valuestring(string s)
        {
            string ans = "";
            int i = 0, j = s.Length, k = 1;
            while (i < j && s[i] != '/')
                i++;
            k = i + 1;
            while (k < j)
            { ans += s[k]; k++; }
            return ans;
        }

        public static string keywordindictionary(string s)
        {
            string ans = "";
            int i = 0, j = s.Length, k = 1;
            while (i < j && s[i] != '_')
                i++;
            k = i + 1;
            while (k < j)
            { ans += s[k]; k++; }
            return ans;
        }

        public void filetodic(SortedDictionary<string, string> ansdictionary, string filename)
        {
            string s = "";
            string skey = "";
            string svalue = "";
            string fn = "answer" + filename;
            string fninput = getfilename(filename);
            StreamReader sr = new StreamReader(fn, Encoding.Default);
            while (sr.EndOfStream != true)
            {
                s = sr.ReadLine() + '\n';
                skey = fninput + '_' + keystring(s);
                svalue = valuestring(s);
                ansdictionary.Add(skey.Trim(), svalue.Trim());
            }
        }
        public void wwsfiletodic(SortedDictionary<string, string> ansdictionary, string filename)
        {
            string s = "";
            string skey = "";
            string svalue = "";
            string fn = "answer" + filename;
            string fninput = getfilename(filename);
            StreamReader sr = new StreamReader(fn, Encoding.Default);
            while (sr.EndOfStream != true)
            {
                s = sr.ReadLine() + '\n';
                skey = fninput + '_' + wwskeystring(s);
                svalue = valuestring(s);
                ansdictionary.Add(skey.Trim(), svalue.Trim());
            }
        }
        public static string getkey(SortedDictionary<string, string> ansdictionary, string str)
        {
            string keystr = "", sss;
            string[] words = str.Split(' ');
            SortedDictionary<string, string>.KeyCollection keyColl = ansdictionary.Keys;
            foreach (string ss in keyColl)
            {
                sss = keywordindictionary(ss.ToLower().Trim());
                if (str.Contains(sss))
                    keystr += ss + " ";
            }
            if (keystr.Length < 2)
                foreach (string ss in keyColl)
                {
                    int count = 0;
                    sss = keywordindictionary(ss.ToLower().Trim());
                    for (int i = 0; i < words.Length; i++)
                        if (sss.Contains(words[i]))
                            count++;
                    if (count >= 2)
                        keystr += ss + " ";
                }
            return keystr;
        }


        public static string getanswer(SortedDictionary<string, string> wwsansdictionary, SortedDictionary<string, string> ansdictionary, string str, string wwsstr)
        {
            string keystr = "", answer = "";
            keystr = getkey(ansdictionary, str);
            ansdictionary.TryGetValue(keystr.Trim(), out answer);
            if (answer == null)
            {
                keystr = getkey(wwsansdictionary, wwsstr);
                wwsansdictionary.TryGetValue(keystr.Trim(), out answer);
            }

            return answer;
        }

        public string answer(string[] fn, string str, string wwsstr)
        {
            string answer = String.Empty;
            
            string fnstr = String.Empty;
                for (int i = 0; i < fn.Length; i++)
                {
                    fnstr = fn[i];
                    answer += ans(wwsstr, fnstr);
                    fsansdictionary.Clear();
                }
            if (answer.Length < 3)
            for (int i = 0; i < fn.Length; i++)
            {
                filetodic(ansdictionary, fn[i]);
                wwsfiletodic(wwsansdictionary, fn[i]);
                answer += getanswer(wwsansdictionary, ansdictionary, str, wwsstr) + "\n";
                ansdictionary.Clear();
                wwsansdictionary.Clear();
            }
            return answer;
        }

        public string create_answer(string str)
        {
            string wsstr = ws.getQuestion(str);
            string[] fn = nbcc.nbcbuild_returnfn("person.txt", "location.txt", "date.txt", "definition.txt", wsstr);
            string returnstr = answer(fn, wsstr, str);
            if (returnstr.Length < 3) returnstr = "\nПопробуйте задать вопрос по-другому ( уточнив вопрос или перефразировать, возможно вы допустили ошибку ). Ответа на Ваш вопрос нет.";
            return returnstr;
        }

        public bool AnswerMethod(string zapros, string key)
        {
            string words = key.ToLower();
            string ss = zapros.ToLower();
            string[] foundsplit = words.Split(new Char[] { ' ', ',', '.', ':', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] split = ss.Split(new Char[] { ' ', ',', '.', ':', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string word = String.Empty;
            if (foundsplit[0] == "отель" || foundsplit[0] == "отели" || foundsplit[0] == "гостиница" || foundsplit[0] == "гостиницы" || foundsplit[0] == "театр" || foundsplit[0] == "музей" || foundsplit[0] == "дом")
                word = foundsplit[1];
            else
                word = foundsplit[0];
            List<string> wordList = new List<string>();
            foreach (string s in split)
            {
                if (s.Trim() != "")
                {
                    wordList.Add(ws.Stem(s.ToLower()));
                }
            }

            List<string> foundWords = SS_LDFS.FuzzySearch(word.ToLower(), wordList, 0.6);
            int fti = 0;
            foreach (string fs in foundWords)
            {
                fti = ws.Stem(zapros.ToLower()).IndexOf(fs);
            }

            if (fti > 0) return true; else return false;
        }

        public void getkeyfromdic(string filename)
        {
            string s = "";
            string skey = "";
            string svalue = "";
            string fn = "answer" + filename;
            string fninput = getfilename(filename);
            StreamReader sr = new StreamReader(fn, Encoding.Default);
            while (sr.EndOfStream != true)
            {
                s = sr.ReadLine() + '\n';
                skey = fninput + '_' + wwskeystring(s);
                svalue = valuestring(s);
                fsansdictionary.Add(skey.Trim().ToLower(), svalue.Trim().ToLower());
                keys.Add(keywordindictionary(skey).Trim());
            }
        }

        public string ans(string strVois, string fn)
        {
            string str = ws.getQuestion(strVois);
            string answerr = String.Empty;
            string answerstr = String.Empty;
            string key = String.Empty;
            getkeyfromdic(fn);
            answerr = "";
            foreach (string keyd in keys)
            {
                if (AnswerMethod(strVois.ToString(), keyd))
                {
                    key = Path.GetFileNameWithoutExtension(fn) + "_" + keyd.ToString().ToLower();
                    fsansdictionary.TryGetValue(key, out answerstr);
                    answerr += answerstr + "\n";
                }
            }
            keys.Clear();
            return answerr;
        }
    }
}

