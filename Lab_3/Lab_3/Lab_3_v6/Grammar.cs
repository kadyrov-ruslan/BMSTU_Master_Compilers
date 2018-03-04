using System;
using System.Collections.Generic;

namespace Lab_3_v6
{
    public class GrammarRule
    {
        public string LeftPart { get; set; }

        public string RightPart { get; set; }

        public GrammarRule(string leftPart, string rightPart)
        {
            LeftPart = leftPart;
            RightPart = rightPart;
        }
    }

    public class Grammar
    {
        public List<string> NonTerminals { get; set; }

        public List<string> Terminals { get; set; }

        public List<GrammarRule> Rules { get; set; }

        public string FirstGrammarSymbol { get; set; }

        public Grammar(List<string> paramStrings)
        {
            var nonTerminalsCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            Console.WriteLine("Считывание грамматики из файла . . .");

            NonTerminals = paramStrings.GetRange(0, nonTerminalsCount);
            paramStrings.RemoveRange(0, nonTerminalsCount);

            var terminalsCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            Terminals = paramStrings.GetRange(0, terminalsCount);
            paramStrings.RemoveRange(0, terminalsCount);

            var rulesCount = int.Parse(paramStrings[0]);
            paramStrings.RemoveAt(0);

            var rules = paramStrings.GetRange(0, rulesCount);
            Rules = new List<GrammarRule>();
            for (var i = 0; i < rulesCount; i++)
            {
                char[] stringSeparators = {'-', '>', ' '};
                var sp = rules[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                for (var j = 1; j < sp.Length; j++) 
                    Rules.Add(new GrammarRule(sp[0], sp[1]));
            }
            paramStrings.RemoveRange(0, rulesCount);

            FirstGrammarSymbol = paramStrings[0];

            Console.WriteLine("------------------------------------");
        }
    }
}
