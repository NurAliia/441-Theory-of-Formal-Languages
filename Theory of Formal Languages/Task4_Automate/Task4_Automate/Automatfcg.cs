using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class Automatfcg
    {
        public string name { get; set; }
        public int priority { get; set; }
        public HashSet<char> alphabet { get; set; }
        public HashSet<int> setStates { get; set; }
        public HashSet<int> startStates { get; set; }
        public HashSet<int> finishStates { get; set; }
        public Dictionary<int, List<HashSet<int>>> Table { get; set; }
        public StringBuilder regex { get; set; }
        public Automatfcg() { }
        public Automatfcg(int set)
        {
            alphabet = new HashSet<char>();
            setStates = new HashSet<int> { set };
            startStates = new HashSet<int> { set };
            finishStates = new HashSet<int> { set + 1 };
            Table = new Dictionary<int, List<HashSet<int>>>();
        }
        public Automatfcg(string name, int priority, StringBuilder regex)
        {
            this.name = name;
            this.priority = priority;
            this.regex = regex;
            alphabet = new HashSet<char>();
            setStates = new HashSet<int>();
            startStates = new HashSet<int>();
            finishStates = new HashSet<int>();
            Table = new Dictionary<int, List<HashSet<int>>>();
            WorkWithAutomat wA = new WorkWithAutomat();
            wA.Create(regex);
        }
        public Automatfcg(Automatfcg automat)
        {
            name = automat.name;
            priority = automat.priority;
            alphabet = new HashSet<char>(automat.alphabet);
            setStates = new HashSet<int>(automat.setStates);
            startStates = new HashSet<int>(automat.startStates);
            finishStates = new HashSet<int>(automat.finishStates);
            Table = new Dictionary<int, List<HashSet<int>>>();
            foreach (var i in automat.Table.Keys)
            {
                Table.Add(i, automat.Table[i]);
            }
        }

        public Automatfcg CreateSimpleAutomat(HashSet<char> alphabet, ref int index)
        {
            Automatfcg automat = new Automatfcg();
            //автомат для 1 символа
            automat.name = name;
            automat.priority = priority;
            automat.startStates = new HashSet<int>();
            automat.setStates = new HashSet<int>();
            automat.finishStates = new HashSet<int>();
            automat.alphabet = new HashSet<char>(alphabet);
            automat.Table = new Dictionary<int, List<HashSet<int>>>();

            automat.startStates.Add(index);
            automat.setStates.UnionWith(automat.startStates);
            index++;
            automat.finishStates.Add(index);
            index++;
            automat.setStates.UnionWith(automat.finishStates);

            List<HashSet<int>> list = new List<HashSet<int>>();
            List<HashSet<int>> listEnd = new List<HashSet<int>>();
            for (int j = 0; j < automat.alphabet.Count; j++)
            {
                list.Add(new HashSet<int> { automat.finishStates.ElementAt(0) });
                listEnd.Add(new HashSet<int> { -1 });
            }
            automat.Table.Add(automat.startStates.ElementAt(0), list);
            automat.Table.Add(automat.finishStates.ElementAt(0), listEnd);

            return automat;
        }

    }
}
