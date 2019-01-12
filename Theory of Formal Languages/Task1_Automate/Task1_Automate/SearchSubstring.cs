using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Automate
{
    class SearchSubstring
    {
        public struct Automate
        {
            public char[] alphabet;
            public int[] setStates;
            public int[] startStates;
            public int[] finishStates;
            public Dictionary<int, List<int[]>> Table;

        }

        public struct Result
        {
            public bool res;
            public int m;
            public string number;

            public Result(bool res, int m, string number)
            {
                this.res = res;
                this.m = m;
                this.number = number;
            }
        }
        public Result maxString(Automate automate, string str, int k)
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
                if (automate.alphabet.Contains(str[i]))
                {
                    int j = Array.IndexOf(automate.alphabet, str[i]);
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
                            result.number = str.Substring(k, result.m);
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
            Automate automate = new Automate();
            StreamReader objReader = new StreamReader(".\\input.txt");
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

            string str = "-2.5e-02+10asd-3dkf8.3-e-7";
            int k = 0;
            Result result;
            for (int i = 0; i < str.Length; i++)
            {
                result = maxString(automate, str, k);
                if (result.res == true)
                {
                    Console.WriteLine("Начиная с позиции " + k + ", автомат нашел число " + result.number);
                    k += result.m;
                }
                else
                {
                    k += 1;
                }
            }
        }
    }
}
