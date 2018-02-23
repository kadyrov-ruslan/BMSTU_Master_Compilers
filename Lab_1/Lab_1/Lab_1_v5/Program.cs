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

            Console.ReadKey();
        }
    }
}
