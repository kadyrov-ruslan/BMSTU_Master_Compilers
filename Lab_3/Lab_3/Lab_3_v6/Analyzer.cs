using System;
using System.Collections.Generic;

namespace Lab_3_v6
{
    public class Analyzer
    {
        private enum AlgState
        {
            q,
            b,
            t
        }

        public static void ParseBottomUp(Grammar grammar, string input)
        {
            var n = input.Length;
            input = input + "$";
            var state = AlgState.q;
            var i = 0;
            var L1 = "";
            var L2 = new Stack<string>();
            L1 += "$";

            var error = false;

            while (true)
            {
                Console.WriteLine(ShowAlgConfig(state, i, L1, L2));

                if (state == AlgState.q)
                {
                    //Шаг №1 - Попытка свертки
                    var isFirstStepSuccess = false;
                    for (var j = 0; j < grammar.Rules.Count; j++)
                        if (L1.EndsWith(grammar.Rules[j].RightPart))
                        {
                            L1 = ReplaceLast(L1, grammar.Rules[j].RightPart, grammar.Rules[j].LeftPart);
                            L2.Push((j + 1).ToString());
                            isFirstStepSuccess = true;
                            break;
                        }

                    if (isFirstStepSuccess)
                        continue;

                    //Шаг №2 - Перенос
                    if (i != n)
                    {
                        i++;
                        L1 += input[i - 1].ToString();
                        L2.Push("s");
                        continue;
                    }

                    //Шаг №3 - Допускание
                    if (i == n && L1.Length == 2 && L1[L1.Length - 1].ToString() == grammar.FirstGrammarSymbol)
                    {
                        state = AlgState.t;
                        Console.WriteLine(ShowAlgConfig(state, i, L1, L2));
                        break;
                    }

                    //Шаг №4 - Переход в состояние возврата
                    if (i == n && !(L1.Length == 2 && L1[L1.Length - 1].ToString() == grammar.FirstGrammarSymbol))
                    {
                        state = AlgState.b;
                        continue;
                    }
                }

                //Шаг №5 - Возврат
                //Пункт 5.a

                if (L2.Count == 0)
                {
                    error = true;
                    break;
                }

                if (L2.Peek() != "s")
                {
                    var next = false;
                    var rj = int.Parse(L2.Peek()) - 1;
                    var curRule = grammar.Rules[rj];
                    var L1_tmp = ReplaceLast(L1, curRule.LeftPart, curRule.RightPart);
                    for (var j = rj + 1; j < grammar.Rules.Count; j++)
                        if (L1_tmp.EndsWith(grammar.Rules[j].RightPart))
                        {
                            next = true;
                            L1_tmp = ReplaceLast(L1_tmp, grammar.Rules[j].RightPart, grammar.Rules[j].LeftPart);
                            L1 = L1_tmp;
                            L2.Pop();
                            L2.Push((j + 1).ToString());
                            state = AlgState.q;
                        }

                    if (next)
                        continue;

                    //Пункт 5.b
                    if (i == n && L1.EndsWith(grammar.Rules[int.Parse(L2.Peek()) - 1].LeftPart))
                    {
                        L1 = ReplaceLast(L1, grammar.Rules[int.Parse(L2.Peek()) - 1].LeftPart,
                            grammar.Rules[int.Parse(L2.Peek()) - 1].RightPart);
                        L2.Pop();
                        continue;
                    }

                    //Пункт 5.c
                    if (i != n && L1.EndsWith(grammar.Rules[int.Parse(L2.Peek()) - 1].LeftPart))
                    {
                        L1 = ReplaceLast(L1, grammar.Rules[int.Parse(L2.Peek()) - 1].LeftPart,
                            grammar.Rules[int.Parse(L2.Peek()) - 1].RightPart);
                        L1 += input[i];
                        i++;
                        L2.Pop();
                        L2.Push("s");
                        state = AlgState.q;
                        continue;
                    }
                }

                //Пункт 5.d
                if (L2.Peek() == "s")
                {
                    i--;
                    L1 = L1.Substring(0, L1.Length - 1); //trouble. Delete symbols > 1 from right parts
                    L2.Pop();
                    continue;
                }

                error = true;
                break;
            }

            if (!error)
                Console.WriteLine("Ок");
            else
                Console.WriteLine("Ошибка при разборе");
        }

        private static string ShowAlgConfig(AlgState state, int i, string l1, Stack<string> l2)
        {
            var tmp = "";
            foreach (var s in l2)
                tmp += s;
            return $"( {state}, {i + 1}, {l1}, {tmp} )";
        }

        private static string ReplaceLast(string src, string oldstr, string newstr)
        {
            var idx = src.IndexOf(oldstr, src.Length - oldstr.Length);
            var t = src.Substring(0, idx);
            return t + newstr;
        }
    }
}
