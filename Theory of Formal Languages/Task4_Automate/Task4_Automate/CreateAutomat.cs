using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class CreateAutomat
    {
        private Stack<Automat> automats { get; set; }
        private Stack<char> operators { get; set; }
        private int index = 0;
        OperationForAutomat op = new OperationForAutomat();

        public Automat CreateSimpleAutomat(string name, int priority,HashSet<string> alphabet,HashSet<string> newChar, ref int index)
        {
            Automat automat = new Automat();
            //автомат для 1 символа
            automat.name = name;
            automat.priority = priority;
            automat.startStates = new HashSet<int>();
            automat.setStates = new HashSet<int>();
            automat.finishStates = new HashSet<int>();
            automat.alphabet = new HashSet<string>(alphabet);
            automat.Table = new Dictionary<int, Dictionary<string, HashSet<int>>>();

            automat.startStates.Add(index);
            automat.setStates.UnionWith(automat.startStates);
            index++;
            automat.finishStates.Add(index);
            index++;
            automat.setStates.UnionWith(automat.finishStates);

            Dictionary<string, HashSet<int>> list = new Dictionary<string, HashSet<int>>();
            Dictionary<string, HashSet<int>> listEnd = new Dictionary<string, HashSet<int>>();            
            for (int j = 0; j < automat.alphabet.Count; j++)
            {
                list.Add(automat.alphabet.ElementAt(j), new HashSet<int> { -1} );
                listEnd.Add( automat.alphabet.ElementAt(j), new HashSet<int> { -1 });
            }
            Dictionary<string, HashSet<int>> listCopy = new Dictionary<string, HashSet<int>>(list);

            for (int j = 0; j < newChar.Count; j++)
            {
                foreach (var pair in list)
                {
                    if (newChar.ElementAt(j).Contains(pair.Key))
                    {
                        listCopy[pair.Key] = new HashSet<int>(automat.finishStates);
                    }
                }
            }
            automat.Table.Add(automat.startStates.ElementAt(0), listCopy);
            automat.Table.Add(automat.finishStates.ElementAt(0), listEnd);

            return automat;
        }

        private int GetPriorityOperation(char operation)
        {
            switch (operation)
            {
                case '|':
                    return 0;
                case '$':
                    return 1;
                case '*':
                    return 2;
                default:
                    return -1;
            }
        }

        public Automat Create(Automat automat)
        {
            HashSet<string> alph = new HashSet<string> { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
            HashSet<string> digits = new HashSet<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            HashSet<string> allSymbols = new HashSet<string> {"!", "@","\"", "#","№", "$", ";",":","%","^","&","?","*","(",")","-","+","=",",",".", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            automats = new Stack<Automat>();
            operators = new Stack<char>();
            index = 0;
            StringBuilder regex = automat.regex;
            for (int i = 0; i < regex.Length; i++)
            {
                switch (regex[i])
                {
                    case '(':
                        operators.Push('(');
                        break;
                    case ')':
                        while (operators.Peek() != '(')
                        {
                            ExecuteOperation(operators.Pop());
                        }
                        operators.Pop();
                        break;
                    case '*':
                    case '$':
                    case '|':
                        char currentOperator = regex[i];
                        while (operators.Count != 0 && automats.Count != 0 && GetPriorityOperation(operators.Peek()) >= GetPriorityOperation(currentOperator))
                        {
                            ExecuteOperation(operators.Pop());
                        }
                        operators.Push(currentOperator);
                        break;
                    case '\\':
                        i++;
                        switch (regex[i])
                        {
                            case ('w'):
                                automat.alphabet.UnionWith(alph);
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet,alph, ref index));
                                break;
                            case ('d'):
                                automat.alphabet.UnionWith(digits);
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, digits, ref index));
                                break;
                            case ('t'):
                                automat.alphabet.UnionWith(new HashSet<string> { "\t" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "\t" }, ref index));
                                break;
                            case ('r'):
                                automat.alphabet.UnionWith(new HashSet<string> { "\r" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "\r" }, ref index));
                                break;
                            case ('n'):
                                automat.alphabet.UnionWith(new HashSet<string> { "\n" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "\n" }, ref index));
                                break;
                            case ('s'):
                                automat.alphabet.UnionWith(new HashSet<string> { "\\s" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "\\s" }, ref index));
                                break;
                            case (' '):
                                automat.alphabet.UnionWith(new HashSet<string> { "\\s" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "\\s" }, ref index));
                                break;
                            case ('@'):
                                automat.alphabet.UnionWith(new HashSet<string> { "" });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { "" }, ref index));
                                break;
                            case ('.'):
                                automat.alphabet.UnionWith(allSymbols);
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, allSymbols, ref index));
                                break;
                            default:
                                automat.alphabet.UnionWith(new HashSet<string> { regex[i].ToString() });
                                automats.Push(CreateSimpleAutomat(automat.name, automat.priority, automat.alphabet, new HashSet<string> { regex[i].ToString() }, ref index));
                                break;
                        }
                        break;
                    default:
                        if (regex[i]=='-')
                        {
                            int er = 0;
                        }
                        automat.alphabet.UnionWith(new HashSet<string> { regex[i].ToString() });
                        automats.Push(CreateSimpleAutomat(automat.name, automat.priority,automat.alphabet, new HashSet<string> { regex[i].ToString() }, ref index));
                        break;
                }
            }
            while (operators.Count() != 0)
            {
                ExecuteOperation(operators.Pop());
            }
            if (automats.Count() == 1)
                return automats.Pop();
            return null;
        }

        private void ExecuteOperation(char operation)
        {
            switch (operation)
            {
                case '*':
                    {
                        Automat automat = automats.Pop();                      
                        automat = op.Interation(automat);
                        index++;
                        automats.Push(automat);
                    }
                    break;
                case '|':
                    {
                        Automat automat1 = automats.Pop();
                        Automat automat2 = automats.Pop();
                        automat1 = op.Union(automat1, automat2);
                        automats.Push(automat1);
                    }
                    break;
                case '$':
                    {
                        Automat automat2 = automats.Pop();
                        Automat automat1 = automats.Pop();
                        automat1 = op.Concat(automat1, automat2);
                        automats.Push(automat1);
                    }
                    break;
            }
        }
        //объединить 2 списка без дубляжей
        private HashSet<string> unionAlphabetWithOutDuble(HashSet<string> list1, HashSet<string> list2)
        {
            
            HashSet<string> resList = list1;

            foreach (var item in list2)
            {
                if (!resList.Contains(item))
                {
                    resList.Add(item);
                }
            }
            return resList;
        }

    }
}
