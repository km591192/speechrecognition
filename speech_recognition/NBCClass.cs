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
   class NBCClass
    {
        SortedDictionary<int, string> allworddictionary = new SortedDictionary<int, string>();
        SortedDictionary<string, double> verdic = new SortedDictionary<string, double>();
        WordStemming ws = new WordStemming();

        private int kolvostrfile(string filename)
        {
            int count = File.ReadAllLines(filename, Encoding.Default).Length;
            return count;
        }

        private int kolvoslovfile(string filename)
        {
            string s = "";
            string[] textMass;
            StreamReader sr = new StreamReader(filename, Encoding.Default);

            while (sr.EndOfStream != true)
            {
                s += sr.ReadLine() + '\n';
            }
            textMass = s.Split(' ', '\n');
            sr.Close();
            return textMass.Length - 1;
        }

        private int kolvostrfile1(string filename)
        {
            int count = File.ReadAllLines(filename, Encoding.Default).Length;
            return count;
        }

        private int kolvoslovfile1(string filename)
        {
            string s = "";
            string[] textMass;
            StreamReader sr = new StreamReader(filename, Encoding.Default);

            while (sr.EndOfStream != true)
            {
                s += sr.ReadLine() + '\n';
            }
            textMass = s.Split(' ', '\n');
            sr.Close();
            return textMass.Length - 1;
        }


        private void adddictionary(string fn, string fn1, string fn2, string fn3, SortedDictionary<int, string> allworddictionary)
        {
            StreamReader sr = new StreamReader(fn, Encoding.Default);
            StreamReader sr1 = new StreamReader(fn1, Encoding.Default);
            StreamReader sr2 = new StreamReader(fn2, Encoding.Default);
            StreamReader sr3 = new StreamReader(fn3, Encoding.Default);
            string s = "";
            string s1 = "";
            string s2 = "";
            string s3 = "";
            while (sr.EndOfStream != true)
            {
                s += sr.ReadLine() + ' ';
            }
            while (sr1.EndOfStream != true)
            {
                s1 += sr1.ReadLine() + ' ';
            }
            while (sr2.EndOfStream != true)
            {
                s2 += sr2.ReadLine() + ' ';
            }
            while (sr3.EndOfStream != true)
            {
                s3 += sr3.ReadLine() + ' ';
            }
            s = ws.Stem(s);
            s1 = ws.Stem(s1);
            s2 = ws.Stem(s2);
            s3 = ws.Stem(s3);

            allworddictionary.Add(2, s);
            allworddictionary.Add(3, s1);
            allworddictionary.Add(4, s2);
            allworddictionary.Add(5, s3);

            SortedDictionary<int, string>.ValueCollection valueColl = allworddictionary.Values;
            string fullstr = "";
            foreach (string ss in valueColl)
            {
                fullstr += ss + " ";
            }
            allworddictionary.Add(1, fullstr);

        }

        private int kolvoslovorazvfile(string slovo, string fn)
        {
            try
            {
                int count = 0;
                string str = "";
                StreamReader sr = new StreamReader(fn, Encoding.Default);
                while (sr.EndOfStream != true)
                {
                    str += sr.ReadLine() + '\n';
                }
                str = ws.Stem(str);

                count = new Regex(slovo).Matches(str).Count;

                return count;
            }
            catch (Exception ex) { return 0; }

        }

        private int kolvounikslovvdictionary(SortedDictionary<int, string> allworddictionary)
        {
            string str = "";
            allworddictionary.TryGetValue(1, out str);

            string[] string_array = str.Split(' ', '\n');
            ArrayList string_list = new ArrayList();

            for (int i = 0; i < string_array.Length; i++)
            {
                String word = string_array[i];
                if (string_list.Contains(word) == false)
                {
                    string_list.Add(word);
                }
            }

            return (string_list.Count - 1);
        }


        private  double veroiatnost(int D, int V, string filename, string str, SortedDictionary<string, double> ver)
        {
            double veroiatn = 0;
            string[] textMass;
            textMass = str.Split(' ', '\n');
            int kolvoslovvstr = textMass.Length;
            int Lc = kolvoslovfile1(filename);
            int Dc = kolvostrfile1(filename);
            double r = (double)Dc / D;
            double first = Math.Log(r);
            double second = 0;
            for (int i = 0; i < kolvoslovvstr; i++)
            {
                int skolkoraz = kolvoslovorazvfile(textMass[i], filename) + 1;
                int VplusLc = Math.Abs(V) + Lc;
                double res = (double)skolkoraz / VplusLc;
                second += Math.Log(res);
            }
            veroiatn = first + second;
            ver.Add(filename, veroiatn);
            return veroiatn;
        }

        private string[] getfilename(SortedDictionary<string, double> ver)
        {
            double max = (from d in ver select d.Value).Max();
            string[] keys = ver.Where(x => x.Value == max).Select(x => x.Key).ToArray();
            return keys;
        }


        public string[] nbcbuild_returnfn(string fn1, string fn2, string fn3, string fn4, string str)
        {
           int countstrper = kolvostrfile(fn1);
            int countstrloc = kolvostrfile(fn2);
            int countstrdate = kolvostrfile(fn3);
            int D = countstrper + countstrloc + countstrdate ;
            int countslovper = kolvoslovfile(fn1);
            int countslovloc = kolvoslovfile(fn2);
            int countslovdate = kolvoslovfile(fn3);
            adddictionary(fn1, fn2, fn3,fn4, allworddictionary);
            int Vdic = kolvounikslovvdictionary(allworddictionary);
            
            veroiatnost(D, Vdic, fn1, str, verdic);
            veroiatnost(D, Vdic, fn2, str, verdic);
            veroiatnost(D, Vdic, fn3, str, verdic);
            veroiatnost(D, Vdic, fn4, str, verdic);

            string[] fn = getfilename(verdic);
            for (int i = 0; i < fn.Length; i++)
            {
                string str1 = fn[i];
                StreamReader sr = new StreamReader(str1, Encoding.Default);
                string str2 = sr.ReadToEnd();
            }

            allworddictionary.Clear();
            verdic.Clear();
            return fn;
        }
    }
       
}
