using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class Automate
    {
        public string name { get; set; }
        public int priority { get; set; }
        public HashSet<string> alphabet { get; set; }
        public HashSet<int> setStates { get; set; }
        public HashSet<int> startStates { get; set; }
        public HashSet<int> finishStates { get; set; }
        public Dictionary<int, Dictionary<string, HashSet<int>>> Table { get; set; }
        public StringBuilder regex { get; set; }
        public Automate() { }
       
        public Automate(string name, int priority, StringBuilder regex)
        {
            this.name = name;
            this.priority = priority;
            this.regex = regex;
            alphabet = new HashSet<string>();
            setStates = new HashSet<int>();
            startStates = new HashSet<int>();
            finishStates = new HashSet<int>();
            Table = new Dictionary<int, Dictionary<string, HashSet<int>>>();         
        }
        public Automate(Automate automat)
        {
            name = automat.name;
            priority = automat.priority;
            alphabet = new HashSet<string>(automat.alphabet);
            setStates = new HashSet<int>(automat.setStates);
            startStates = new HashSet<int>(automat.startStates);
            finishStates = new HashSet<int>(automat.finishStates);
            Table = automat.Table;
        }       
    }
}
