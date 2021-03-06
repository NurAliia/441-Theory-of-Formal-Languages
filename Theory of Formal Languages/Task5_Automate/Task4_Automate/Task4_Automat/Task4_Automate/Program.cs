﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4_Automate;
using System.IO;
using static Task4_Automate.SearchSubstring;

namespace Task4_Automate
{
    public struct Lexeme
    {
        public string name;
        public int priority;
        public StringBuilder regex;

        public Lexeme(string name, int priority, StringBuilder regex)
        {
            this.name = name;
            this.priority = priority;
            this.regex = regex;
        }
    }
    class Program
    {       
        static void Main(string[] args)
        {
            List<Lexeme> lexemes = ReadLexemes();
            List<Automate> automates = new List<Automate>();
            CreateAutomate ca = new CreateAutomate();
            for (int i = 0; i < lexemes.Count; i++) //внешний цикл по все лексемам
            {
                Console.WriteLine(lexemes[i].regex);
                Automate automat = new Automate(lexemes[i].name, lexemes[i].priority, lexemes[i].regex);
                automat = ca.Create(automat);
                automates.Add(automat);
            }
            SearchSubstring task = new SearchSubstring();
            List<MaxStringResult> listTokens = task.Start(automates);
            RecursiveDescentParser rDP = new RecursiveDescentParser();
            int k = 0;
            List<int> regulations = rDP.RecursiveDescentParserMethod(ref k, listTokens);
            WriteIntoFile("outputRegulations.txt", regulations);
        
        }
        public static List<Lexeme> ReadLexemes()
        {
            List<Lexeme> lexemes = new List<Lexeme>();
            string[] lines = File.ReadAllLines("lexems.txt");
            for (int j = 0; j < lines.Length; j++)
            {
                string[] s = lines[j].Split(':');
                StringBuilder regex = new StringBuilder(s[2]);
                for (int i = 0; i < regex.Length - 1; i++)
                {
                    if ((regex[i] != '\\') && (regex[i] != '(') && (regex[i] != '|') &&
                        (regex[i+1] != ')') && (regex[i+1] != '|') && (regex[i + 1] != '*'))
                    {
                        regex.Insert(i+1, "$");
                        i++;
                    }                    
                }
                lexemes.Add(new Lexeme(s[0], int.Parse(s[1]), regex));
            }       
            return lexemes;
        }
        public static void WriteIntoFile(string nameFile, List<int> answerList)
        {
            using (StreamWriter sw = new StreamWriter(nameFile, false, System.Text.Encoding.Default))
            {
                foreach (int ms in answerList)
                {
                    sw.WriteLine(ms + " ");
                }
            }
        }
    }
}
