using System;

namespace Lab_2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Regex a = new Sym("A");
            Regex b = new Sym("B");
            Regex c = new Sym("C");
            Regex abStar = new Star(new Alt(a, b));
            Regex bb = new Seq(b, b);
            Regex r = new Seq(abStar, new Seq(a, b));
            // The regular expression (a|b)*ab
            BuildAndShow("dfa1.dot", r);
            // The regular expression ((a|b)*ab)*
            BuildAndShow("dfa2.dot", new Star(r));
            // The regular expression ((a|b)*ab)((a|b)*ab)
            BuildAndShow("dfa3.dot", new Seq(r, r));
            // The regular expression (a|b)*abb, from ASU 1986 p 136
            BuildAndShow("dfa4.dot", new Seq(abStar, new Seq(a, bb)));
            // SML reals: sign?((digit+(\.digit+)?))([eE]sign?digit+)?
            Regex d = new Sym("digit");
            Regex dPlus = new Seq(d, new Star(d));
            Regex s = new Sym("sign");
            Regex sOpt = new Alt(s, new Eps());
            Regex dot = new Sym(".");
            Regex dotDigOpt = new Alt(new Eps(), new Seq(dot, dPlus));
            Regex mant = new Seq(sOpt, new Seq(dPlus, dotDigOpt));
            Regex e = new Sym("e");
            Regex exp = new Alt(new Eps(), new Seq(e, new Seq(sOpt, dPlus)));
            Regex smlReal = new Seq(mant, exp);
            BuildAndShow("dfa5.dot", smlReal);

            Console.ReadKey();
        }

        public static void BuildAndShow(string filename, Regex r)
        {
            var nfa = r.MkNfa(new Nfa.NameSource());
            Console.WriteLine(nfa);
            Console.WriteLine("---");
            var dfa = nfa.ToDfa();
            Console.WriteLine(dfa);
            Console.WriteLine("Writing DFA graph to file " + filename);
            dfa.WriteDot(filename);
            Console.WriteLine();
        }
    }
}