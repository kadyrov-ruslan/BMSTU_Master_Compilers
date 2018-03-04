using System;
using System.Linq;

namespace Lab_3_v6
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var param = FileIoHandler.ReadTxtFile("input.txt").ToList();
            var grammar = new Grammar(param);

            Console.WriteLine("Введите входное выражение");
            var input = Console.ReadLine();
            Console.WriteLine("Выполнение восходящего разбора с возвратами . . .");

            Analyzer.ParseBottomUp(grammar, input);
            Console.ReadKey();
        }
    }
}