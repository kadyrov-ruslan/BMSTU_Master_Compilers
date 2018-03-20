using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_1_v5
{
    public class Grammar
    {
        public List<string> NonTerminals { get; set; }

        public List<string> Terminals { get; set; }

        public List<string> Rules { get; set; }

        public string FirstGrammarSymbol { get; set; }

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

        /// <summary>
        /// Получение результирующих нецикличных правил
        /// </summary>
        /// <returns></returns>
        public string[] GetNonCycleRules()
        {
            var resultNonCycleRules = new List<string>();
            var nonTermSets = PrepareSets();
            var initNonCycleRules = Rules.Except(GetCycleRules()).ToList();

            //Проходим по множествам Ri каждого i-того нетерминала
            foreach (var nonTermSet in nonTermSets)
            {
                var mainNonTerm = nonTermSet.FirstOrDefault();
                var nonTermNonCycleRules = initNonCycleRules.Where(p => p != mainNonTerm).ToList();
                for (var i = 0; i < nonTermNonCycleRules.Count; i++)
                {
                    var leftPart = GetRuleParts(nonTermNonCycleRules[i])[0];
                    var rightPart = string.Empty;
                    if (nonTermSet.Contains(leftPart))
                    {
                        rightPart = GetRuleParts(nonTermNonCycleRules[i])[1];
                        resultNonCycleRules.Add(mainNonTerm + " -> " + rightPart);
                    }
                }
            }

            //initNonCycleRules.AddRange(resultNonCycleRules);
            var result = resultNonCycleRules.Distinct();
            return result.ToArray();
        }

        /// <summary>
        /// Получение множеств N(i) для каждого нетерминала
        /// </summary>
        /// <returns></returns>
        private List<string[]> PrepareSets()
        {
            var results = new List<string[]>();
            var cycleRules = GetCycleRules();

            foreach (var nonterminal in NonTerminals)
            {
                var nonterminalSet = new List<string>
                {
                    nonterminal
                };
                var previousNonterminalSet = new List<string>();

                //Получение множества для каждого нетерминала
                var step = 0;
                while (!ListsAreEqual(nonterminalSet, previousNonterminalSet))
                {
                    previousNonterminalSet.AddRange(nonterminalSet.Except(previousNonterminalSet));

                    //Если в цикличных правилах имеется такое правило, что левая его часть равна текущему нетерминалу,
                    //то добавляем в множество этого нетерминала правую часть правила
                    if (cycleRules.Any(x => GetRuleParts(x)[0] == nonterminalSet[step]))
                    {
                        var foundRule = cycleRules.FirstOrDefault(x =>GetRuleParts(x)[0] == nonterminalSet[step]);
                        var rightPart = GetRuleParts(foundRule)[1];

                        nonterminalSet.Add(rightPart);
                        step++;
                    }
                }
                results.Add(nonterminalSet.ToArray());
            }
            return results;
        }

        /// <summary>
        /// Получение цикличных правил
        /// </summary>
        /// <returns></returns>
        private string[] GetCycleRules()
        {
            var cycleRules = new List<string>();

            foreach (var rule in Rules)
            {
                //Разделяем правило на левую и правую сторону
                var result = GetRuleParts(rule);

                //Если правая часть правила равна какому - либо нетерминалу (кроме себя), 
                //получаем цикличное правило
                if (NonTerminals.Where(p => p != result[0]).Contains(result[1]))
                    cycleRules.Add(rule);
            }

            return cycleRules.ToArray();
        }

        private static bool ListsAreEqual(List<string> firstList, List<string> secondList)
        {
            if (firstList.Count != secondList.Count)
                return false;

            for (var i = 0; i < firstList.Count; i++)
            {
                if (firstList[i] != secondList[i])
                    return false;
            }
            return true;
        }

        private static string[] GetRuleParts(string rule)
        {
            var stringSeparators = new[] { " -> " };
            //Разделяем правило на левую и правую сторону
            return rule.Split(stringSeparators, StringSplitOptions.None);
        }
    }
}
