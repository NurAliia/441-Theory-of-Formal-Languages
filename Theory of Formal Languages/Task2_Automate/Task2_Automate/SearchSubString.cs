using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task2_Automate
{
    class SearchSubstring
    {
        public Dictionary<char, string> automateSpace = new Dictionary<char, string>() {
            { '\u0009',"\\t" },
            { '\u000D',"\\r" },
            { '\u000A',"\\n" },
            { '\u0020',"\\s" },
            { '\u000B',"\\v" },

         };
        public struct Automate
        {
            public string name;
            public int priority;
            public char[] alphabet;
            public int[] setStates;
            public int[] startStates;
            public int[] finishStates;
            public Dictionary<int, List<int[]>> Table;
            public Automate(Automate a)
            {
                name = a.name;
                priority = a.priority;
                alphabet = a.alphabet;
                setStates = a.setStates;
                startStates = a.startStates;
                finishStates = a.finishStates;
                Table = a.Table;
            }
        }
        
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
        public Result MaxString(Automate automate, string str, int k)
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
                if (automate.alphabet.Contains(str[i]) || automate.name == "Identify")
                {
                    int j;
                    if (automate.name == "Identify")
                    {
                        if (automate.alphabet.Contains(str[i]))
                        {
                            j = 0;
                        }
                        else
                        {
                            if (str[i] == '|')
                            {
                                j = 1;
                            }
                            else
                                j = 2;
                        }
                    }
                    else
                    {
                        j = Array.IndexOf(automate.alphabet, str[i]);
                    }
                    foreach (var currentState in currentSet)
                    {
                        foreach (var next in automate.Table[currentState][j])
                        {
                            if (next == -1)
                            {
                                return result;
                            }
                            temp.Add(next);
                        }

                        if (automate.finishStates.Intersect(temp).Count() != 0)
                        {
                            result.res = true;
                            result.m = i - k + 1;
                            result.substring = str.Substring(k, result.m);
                        }
                    }
                    currentSet = temp;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public void Start()
        {
            List<Automate> listAutomate = new List<Automate>();            
            for (int i = 0; i < 7; i++)
            {
                string nameFile = "input" + (i+1) + ".txt";
                ReadFile(nameFile);
                listAutomate.Add(new Automate(ReadFile(nameFile)));
            }
            listAutomate[4].alphabet[0] = automateSpace.ElementAt(0).Key; //tab \t
            listAutomate[4].alphabet[1] = automateSpace.ElementAt(1).Key; //\r
            listAutomate[4].alphabet[2] = automateSpace.ElementAt(2).Key; //New Line \n
            listAutomate[4].alphabet[3] = automateSpace.ElementAt(3).Key; //Space \s
            listAutomate[4].alphabet[4] = automateSpace.ElementAt(4).Key; //tab \v
            StreamReader objReader = new StreamReader("inputText.txt");
            string str = objReader.ReadToEnd();
            int k = 0;
            Result result;
            MaxStringResult maxSubString;
            List<string> answerList = new List<string>();
            for (int i = 0; i < str.Length;)
            {
                maxSubString = new MaxStringResult(0);
                foreach (Automate a in listAutomate)
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
                if (maxSubString.maxString.Length!=0)
                {
                    k += maxSubString.maxString.Length;
                    i = k;          
                    if (maxSubString.nameAutomate=="Space")
                    {
                        string newFormat="";
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
                    answerList.Add("<Error,"+ str[k] + ">");
                    k++;
                    i++;
                }
            }
            WriteIntoFile("output.txt", answerList);
        }
        public Automate ReadFile(string nameFile)
        {
            Automate automate = new Automate();
            StreamReader objReader = new StreamReader(nameFile);
            automate.name = objReader.ReadLine();
            automate.priority = Int32.Parse(objReader.ReadLine());
            automate.alphabet = objReader.ReadLine().Split(' ').Select(x => Char.Parse(x)).ToArray();
            automate.setStates = objReader.ReadLine().Split(' ').Select(x => Int32.Parse(x)).ToArray();
            automate.startStates = objReader.ReadLine().Split(' ').Select(x => Int32.Parse(x)).ToArray();
            automate.finishStates = objReader.ReadLine().Split(' ').Select(x => Int32.Parse(x)).ToArray();

            automate.Table = new Dictionary<int, List<int[]>>();
            int[] transitions;
            for (int i = 0; i < automate.setStates.Length; i++)
            {
                List<int[]> list = new List<int[]>();
                string[] masLine = objReader.ReadLine().Split('|');
                foreach (string s in masLine)
                {
                    transitions = s.Split(' ').Select(x => Int32.Parse(x)).ToArray();
                    list.Add(transitions);
                }
                automate.Table.Add(automate.setStates[i], list);
            }
            objReader.Close();
            return automate;
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
