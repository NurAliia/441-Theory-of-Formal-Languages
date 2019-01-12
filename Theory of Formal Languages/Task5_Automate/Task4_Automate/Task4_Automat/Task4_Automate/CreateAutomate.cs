using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_Automate
{
    class CreateAutomate
    {
        private Stack<Automate> automats { get; set; }
        private Stack<char> operators { get; set; }
        private int index = 0;
        OperationForAutomate op = new OperationForAutomate();

        public Automate CreateSimpleAutomat(string name, int priority,HashSet<string> alphabet,HashSet<string> newChar, ref int index)
        {
            Automate automate = new Automate();
            //автомат для 1 символа
            automate.name = name;
            automate.priority = priority;
            automate.startStates = new HashSet<int>();
            automate.setStates = new HashSet<int>();
            automate.finishStates = new HashSet<int>();
            automate.alphabet = new HashSet<string>(alphabet);
            automate.Table = new Dictionary<int, Dictionary<string, HashSet<int>>>();

            automate.startStates.Add(index);
            automate.setStates.UnionWith(automate.startStates);
            index++;
            automate.finishStates.Add(index);
            index++;
            automate.setStates.UnionWith(automate.finishStates);

            Dictionary<string, HashSet<int>> list = new Dictionary<string, HashSet<int>>();
            Dictionary<string, HashSet<int>> listEnd = new Dictionary<string, HashSet<int>>();            
            for (int j = 0; j < automate.alphabet.Count; j++)
            {
                list.Add(automate.alphabet.ElementAt(j), new HashSet<int> { -1} );
                listEnd.Add( automate.alphabet.ElementAt(j), new HashSet<int> { -1 });
            }
            Dictionary<string, HashSet<int>> listCopy = new Dictionary<string, HashSet<int>>(list);

            for (int j = 0; j < newChar.Count; j++)
            {
                foreach (var pair in list)
                {
                    if (newChar.ElementAt(j).Contains(pair.Key))
                    {
                        listCopy[pair.Key] = new HashSet<int>(automate.finishStates);
                    }
                }
            }
            automate.Table.Add(automate.startStates.ElementAt(0), listCopy);
            automate.Table.Add(automate.finishStates.ElementAt(0), listEnd);

            return automate;
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

        public Automate Create(Automate automate)
        {
            HashSet<string> alph = new HashSet<string> { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
            HashSet<string> digits = new HashSet<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            HashSet<string> allSymbols = new HashSet<string> {"!", "@","\"", "#","№", "$", ";",":","%","^","&","?","*","(",")","-","+","=",",",".", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            automats = new Stack<Automate>();
            operators = new Stack<char>();
            index = 0;
            StringBuilder regex = automate.regex;
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
                    case '|':
                    case '$':
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
                                automate.alphabet.UnionWith(alph);
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet,alph, ref index));
                                break;
                            case ('d'):
                                automate.alphabet.UnionWith(digits);
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, digits, ref index));
                                break;
                            case ('s'):
                                automate.alphabet.UnionWith(new HashSet<string> { "\\s" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "\\s" }, ref index));
                                break;
                            case (' '):
                                automate.alphabet.UnionWith(new HashSet<string> { "\\s" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "\\s" }, ref index));
                                break;
                            case ('t'):
                                automate.alphabet.UnionWith(new HashSet<string> { "\t" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "\t" }, ref index));
                                break;
                            case ('r'):
                                automate.alphabet.UnionWith(new HashSet<string> { "\r" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "\r" }, ref index));
                                break;
                            case ('n'):
                                automate.alphabet.UnionWith(new HashSet<string> { "\n" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "\n" }, ref index));
                                break;                           
                            case ('@'):
                                automate.alphabet.UnionWith(new HashSet<string> { "" });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { "" }, ref index));
                                break;
                            case ('.'):
                                automate.alphabet.UnionWith(allSymbols);
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, allSymbols, ref index));
                                break;
                            default:
                                automate.alphabet.UnionWith(new HashSet<string> { regex[i].ToString() });
                                automats.Push(CreateSimpleAutomat(automate.name, automate.priority, automate.alphabet, new HashSet<string> { regex[i].ToString() }, ref index));
                                break;
                        }
                        break;
                    default:
                        automate.alphabet.UnionWith(new HashSet<string> { regex[i].ToString() });
                        automats.Push(CreateSimpleAutomat(automate.name, automate.priority,automate.alphabet, new HashSet<string> { regex[i].ToString() }, ref index));
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
                        Automate automate = automats.Pop();                      
                        automate = op.Interation(automate);
                        index++;
                        automats.Push(automate);
                    }
                    break;
                case '|':
                    {
                        Automate automat1 = automats.Pop();
                        Automate automat2 = automats.Pop();
                        automat1 = op.Union(automat1, automat2);
                        automats.Push(automat1);
                    }
                    break;
                case '$':
                    {
                        Automate automat2 = automats.Pop();
                        Automate automat1 = automats.Pop();
                        automat1 = op.Concat(automat1, automat2);
                        automats.Push(automat1);
                    }
                    break;
            }
        }
    }
}
