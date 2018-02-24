using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1_v5
{
    class Program
    {
        static void Main(string[] args)
        {
            var param = FileIoHandler.ReadTxtFile("input.txt").ToList();

            var grammar = new Grammar(param);

            //var cycleRules = grammar.GetCycleRules();

            //Console.WriteLine("------------------------------------");
            //Console.WriteLine("Цепные правила");
            //foreach (var rule in cycleRules)
            //    Console.WriteLine(rule);

            //foreach (var set in results)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("===============================================");
            //    for (var i = 0; i < set.Length; i++)
            //    {
            //        Console.WriteLine(set[i]);
            //    }
            //}

            var s = grammar.GetNonCycleRules();
            s = s.OrderBy(q => q).ToArray();
            //var cycleRules = grammar.GetCycleRules();

            //Console.WriteLine("------------------------------------");
            //Console.WriteLine("Цепные правила");
            foreach (var rule in s)
                Console.WriteLine(rule);
            Console.ReadKey();
        }
    }
}
