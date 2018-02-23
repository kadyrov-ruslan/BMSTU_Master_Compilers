﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1_v5
{
    public class Grammar
    {
        public List<string> NonTerminals { get; set; }

        public List<string> Terminals { get; set; }

        public List<string> Rules { get; set; }

        public string FirstGrammarSymbol  { get; set; }

        public Grammar(List<string> paramStrings)
        {
            var nonTerminalsCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            Console.WriteLine($"нетерминалы {nonTerminalsCount}");

            NonTerminals = paramStrings.GetRange(0, nonTerminalsCount);
            paramStrings.RemoveRange(0, nonTerminalsCount);

            foreach (var nonterm in NonTerminals)
                Console.WriteLine(nonterm);

            var terminalsCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            Console.WriteLine($"терминалы {terminalsCount}");

            Terminals = paramStrings.GetRange(0, terminalsCount);
            paramStrings.RemoveRange(0, terminalsCount);

            foreach (var term in Terminals)
                Console.WriteLine(term);

            var rulesCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            Console.WriteLine($"правила {rulesCount}");

            Rules = paramStrings.GetRange(0, rulesCount);
            paramStrings.RemoveRange(0, rulesCount);

            foreach (var rule in Rules)
                Console.WriteLine(rule);

            FirstGrammarSymbol = paramStrings[0];

            Console.WriteLine($"первый элемент {FirstGrammarSymbol}");
        }
    }
}