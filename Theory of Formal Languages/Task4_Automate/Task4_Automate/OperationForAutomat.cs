using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class OperationForAutomat
    {
        public Automat Interation(Automat automat)
        {
            //Из всех заключительных состояний организуем переходы туда же, куда есть переходы из нач состояний
            Automat copyAutomat = new Automat(automat);

            foreach (var i in automat.finishStates)
            {
                foreach (var j in copyAutomat.startStates)
                {
                    foreach (var pair in copyAutomat.Table[j])
                    {
                        if (copyAutomat.Table[i].ContainsKey(pair.Key))
                        {
                            copyAutomat.Table[i][pair.Key] = pair.Value;                            
                        }
                        else
                        {
                            copyAutomat.Table[i].Add(pair.Key, pair.Value);
                        }                 
                    }
                }
            }
            //Добавляем новое состояние и делаем его заключительным и начальным
            int index = automat.Table.Keys.Max();
            index++;
            copyAutomat.startStates.Add(index);
            copyAutomat.finishStates.Add(index);
            copyAutomat.setStates.Add(index);

            Dictionary<string, HashSet<int>> dictionary = new Dictionary<string, HashSet<int>>();
            foreach (var item in automat.alphabet)
            {
                dictionary.Add(item, new HashSet<int> { - 1 });
            }
            copyAutomat.Table.Add(index, dictionary);
            return copyAutomat;
        }      
        public Automat Concat(Automat automat1, Automat automat2)
        {           
            if (automat1.alphabet!= null)
            {
                Automat resAutomat = new Automat();
                resAutomat.name = automat1.name;
                resAutomat.priority = automat1.priority;
                automat1.alphabet.UnionWith(automat2.alphabet);
                resAutomat.alphabet = automat1.alphabet;
                resAutomat.startStates = automat1.startStates;              
                resAutomat.finishStates = automat2.finishStates;
                resAutomat.setStates = automat1.setStates;
                resAutomat.setStates.UnionWith(automat2.setStates);
                resAutomat.Table = new Dictionary<int, Dictionary<string, HashSet<int>>>();
                foreach (var i in automat1.Table.Keys)
                {
                    resAutomat.Table.Add(i, automat1.Table[i]);
                }
                foreach (var i in automat2.Table.Keys)
                {
                    resAutomat.Table.Add(i, automat2.Table[i]);
                }
                foreach (var i in automat1.finishStates)
                {
                    foreach (var j in automat2.startStates)
                    {
                        foreach (var pair in automat2.Table[j])
                        {
                            if (pair.Key==".")
                            {
                                int uer = 0;
                            }
                            if (resAutomat.Table[i].ContainsKey(pair.Key))
                            {
                              //resAutomat.Table[i][pair.Key].Remove(-1);
                                HashSet<int> values = new HashSet<int>();
                                foreach (var vl in resAutomat.Table[i][pair.Key])
                                {
                                    values.Add(vl);
                                }
                                foreach (var value in pair.Value)
                                {
                                    values.Add(value);
                                }
                                resAutomat.Table[i][pair.Key] = values;
                            }
                            else
                            {
                                resAutomat.Table[i].Add(pair.Key, pair.Value);
                            }
                        }
                    }
                }

                if (automat2.finishStates.Intersect(automat2.startStates).Count() != 0)
                {
                    //если хотя бы одно из нач. сост было заключ, то объявляем заключ. все заключ. сост 1го автомата
                    resAutomat.finishStates.UnionWith(automat1.finishStates);
                }
                return resAutomat;
            }
            else
                return automat2;
        }
        
        public Automat Union(Automat automat1, Automat automat2)
        {
            Automat resAutomat = new Automat();
            resAutomat.name = automat1.name;
            resAutomat.priority = automat1.priority;
            automat1.setStates.UnionWith(automat2.setStates);
            resAutomat.setStates = automat1.setStates;
            automat1.alphabet.UnionWith(automat2.alphabet);
            resAutomat.alphabet = automat1.alphabet;
            automat1.startStates.UnionWith(automat2.startStates);
            resAutomat.startStates = automat1.startStates;
            automat1.finishStates.UnionWith(automat2.finishStates);
            resAutomat.finishStates = automat1.finishStates;
            resAutomat.Table = automat1.Table;
            foreach (var i in automat2.Table.Keys)
            {
                Dictionary<string, HashSet<int>> dic = new Dictionary<string, HashSet<int>>();
                foreach (var pair in automat2.Table[i])
                {
                    dic.Add(pair.Key, pair.Value);
                }
                resAutomat.Table.Add(i, dic);
            }
            return resAutomat;
        }
    }
}
