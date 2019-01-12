using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class SearchSubstring
    {
        public Dictionary<char, string> automateSpace = new Dictionary<char, string>() {
            { '\u0009',"\\t" },
            { '\u000D',"\\r" },
            { '\u000A',"\\n" },
            { '\u0020',"\\s" },
            { '\u000B',"\\v" },
            { '\u0032',"\\s" }

         };
        public struct Result
        {
            public bool res;
            public int m;
            public string substring;

            public Result(bool res, int m, string substring)
            {
                this.res = res;
                this.m = m;
                this.substring = substring;
            }
        }
        public struct MaxStringResult
        {
            public int m;
            public int priority;
            public string nameAutomate;
            public string maxString;

            public MaxStringResult(int m, int priority, string nameAutomate, string maxString)
            {
                this.m = m;
                this.priority = priority;
                this.nameAutomate = nameAutomate;
                this.maxString = maxString;
            }
            public MaxStringResult(int m)
            {
                this.m = m;
                this.priority = 0;
                nameAutomate = "";
                maxString = "";
            }
        }
        public Result MaxString(Automat automate, string str, int k)
        {
            HashSet<int> currentSet = new HashSet<int>(automate.startStates);
            Result result = new Result(false, 0, "");
            if (automate.finishStates.Intersect(currentSet).Count() != 0)
            {
                result.res = true;
            }
            HashSet<int> temp;
            for (int i = k; i < str.Length; i++)
            {
                temp = new HashSet<int>();
                if (automate.alphabet.Contains(str[i].ToString()))
                {             
                    foreach (var currentState in currentSet)
                    {
                        foreach (var pair in automate.Table[currentState])
                        {
                            if(pair.Key == str[i].ToString())
                            {
                                foreach (var p in pair.Value)
                                {
                                    if (p != -1 || pair.Value.Count > 1)
                                    {
                                        temp.Add(p);
                                    }
                                }
                            }
                        }                      
                        if (automate.finishStates.Intersect(temp).Count() != 0)
                        {
                            result.res = true;
                            result.m = i - k + 1;
                            result.substring = str.Substring(k, result.m);
                            break;
                        }
                    }
                    if (temp.Count == 0)
                    {
                        return result;
                    }
                    temp.Remove(-1);
                    currentSet = temp;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public void Start(List<Automat> listAutomate)
        {                     
            StreamReader objReader = new StreamReader("inputText.txt");
            string str = objReader.ReadToEnd();
            int k = 0;
            Result result;
            MaxStringResult maxSubString;
            List<string> answerList = new List<string>();
            for (int i = 0; i < str.Length;)
            {
                maxSubString = new MaxStringResult(0);
                foreach (Automat a in listAutomate)
                {
                    result = MaxString(a, str, k);
                    if (result.res == true && (result.m > maxSubString.m || (result.m == maxSubString.m && a.priority < maxSubString.priority)))
                    {
                        maxSubString.maxString = result.substring;
                        maxSubString.nameAutomate = a.name;
                        maxSubString.m = result.m;
                        maxSubString.priority = a.priority;
                    }
                }
                if (maxSubString.maxString.Length != 0)
                {
                    k += maxSubString.maxString.Length;
                    i = k;
                    if (maxSubString.nameAutomate == "Space")
                    {
                        string newFormat = "";
                        foreach (var c in maxSubString.maxString)
                        {
                            newFormat += automateSpace[c];
                        }
                        maxSubString.maxString = newFormat;
                    }
                    answerList.Add("<" + maxSubString.nameAutomate + "," + maxSubString.maxString + ">");
                }
                else
                {
                    answerList.Add("<Error," + str[k] + ">");
                    k++;
                    i++;
                }
            }
            WriteIntoFile("output.txt", answerList);
        }    
        public void WriteIntoFile(string nameFile, List<String> answerList)
        {
            using (StreamWriter sw = new StreamWriter(nameFile, false, System.Text.Encoding.Default))
            {
                foreach (string ms in answerList)
                {
                    sw.WriteLine(ms);
                }
            }
        }
    }
}
