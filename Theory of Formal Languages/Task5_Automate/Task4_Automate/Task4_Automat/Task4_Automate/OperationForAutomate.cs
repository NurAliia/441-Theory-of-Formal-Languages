using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class OperationForAutomate
    {
        public Automate Interation(Automate automate)
        {
            //Из всех заключительных состояний организуем переходы туда же, куда есть переходы из нач состояний
            Automate newAutomate = new Automate(automate);

            foreach (var i in automate.finishStates)
            {
                foreach (var j in newAutomate.startStates)
                {
                    foreach (var pair in newAutomate.Table[j])
                    {
                        if (newAutomate.Table[i].ContainsKey(pair.Key))
                        {
                            //newAutomat.Table[i][pair.Key] = pair.Value;  
                            HashSet<int> values = new HashSet<int>();
                            foreach (var vl in newAutomate.Table[i][pair.Key])
                            {
                                values.Add(vl);
                            }
                            foreach (var value in pair.Value)
                            {
                                values.Add(value);
                            }
                            newAutomate.Table[i][pair.Key] = values;
                        }
                        else
                        {
                            newAutomate.Table[i].Add(pair.Key, pair.Value);
                        }                 
                    }
                }
            }
            //Добавляем новое состояние и делаем его заключительным и начальным
            int index = automate.Table.Keys.Max();
            index++;
            newAutomate.startStates.Add(index);
            newAutomate.finishStates.Add(index);
            newAutomate.setStates.Add(index);

            Dictionary<string, HashSet<int>> dictionary = new Dictionary<string, HashSet<int>>();
            foreach (var item in automate.alphabet)
            {
                dictionary.Add(item, new HashSet<int> { - 1 });
            }
            newAutomate.Table.Add(index, dictionary);
            return newAutomate;
        }      
        public Automate Concat(Automate automat1, Automate automat2)
        {           
            if (automat1.alphabet!= null)
            {
                Automate newAutomate = new Automate();
                newAutomate.name = automat1.name;
                newAutomate.priority = automat1.priority;
                automat1.alphabet.UnionWith(automat2.alphabet);
                newAutomate.alphabet = automat1.alphabet;
                newAutomate.startStates = automat1.startStates;
                newAutomate.finishStates = automat2.finishStates;
                newAutomate.setStates = automat1.setStates;
                newAutomate.setStates.UnionWith(automat2.setStates);
                newAutomate.Table = automat1.Table;
                foreach (var i in automat2.Table.Keys)
                {
                    newAutomate.Table.Add(i, automat2.Table[i]);
                }
                foreach (var i in automat1.finishStates)
                {
                    foreach (var j in automat2.startStates)
                    {
                        foreach (var pair in automat2.Table[j])
                        {
                            if (newAutomate.Table[i].ContainsKey(pair.Key))
                            {
                                HashSet<int> values = new HashSet<int>();
                                foreach (var vl in newAutomate.Table[i][pair.Key])
                                {
                                    values.Add(vl);
                                }
                                foreach (var value in pair.Value)
                                {
                                    values.Add(value);
                                }
                                newAutomate.Table[i][pair.Key] = values;
                            }
                            else
                            {
                                newAutomate.Table[i].Add(pair.Key, pair.Value);
                            }
                        }
                    }
                }

                if (automat2.finishStates.Intersect(automat2.startStates).Count() != 0)
                {
                    //если хотя бы одно из начачальное состоние было заключительным, то объявляем заключительными все заключ. сост 1го автомата
                    newAutomate.finishStates.UnionWith(automat1.finishStates);
                }
                return newAutomate;
            }
            else
                return automat2;
        }
        
        public Automate Union(Automate automat1, Automate automat2)
        {
            Automate newAutomate = new Automate();
            newAutomate.name = automat1.name;
            newAutomate.priority = automat1.priority;
            automat1.setStates.UnionWith(automat2.setStates);
            newAutomate.setStates = automat1.setStates;
            automat1.alphabet.UnionWith(automat2.alphabet);
            newAutomate.alphabet = automat1.alphabet;
            automat1.startStates.UnionWith(automat2.startStates);
            newAutomate.startStates = automat1.startStates;
            automat1.finishStates.UnionWith(automat2.finishStates);
            newAutomate.finishStates = automat1.finishStates;
            newAutomate.Table = automat1.Table;
            foreach (var i in automat2.Table.Keys)
            {
                newAutomate.Table.Add(i, automat2.Table[i]);
            }
            return newAutomate;
        }
    }
}
