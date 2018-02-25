using System.IO;
using System.Linq;

namespace Lab_1_v5
{
    public class FileIoHandler
    {
        public static string[] ReadTxtFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public static bool WriteTxtFile(Grammar grammar)
        {
            using (var file = new StreamWriter(@"output.txt"))
            {
                file.WriteLine(grammar.NonTerminals.Count);
                foreach (var nonterm in grammar.NonTerminals)
                    file.WriteLine(nonterm);

                file.WriteLine(grammar.Terminals.Count);
                foreach (var term in grammar.Terminals)
                    file.WriteLine(term);

                var newRules = grammar.GetNonCycleRules();
                newRules = newRules.OrderBy(q => q).ToArray();

                file.WriteLine(newRules.Length);
                foreach (var newRule in newRules)
                    file.WriteLine(newRule);

                file.WriteLine(grammar.FirstGrammarSymbol);
            }

            return true;
        }
    }
}
