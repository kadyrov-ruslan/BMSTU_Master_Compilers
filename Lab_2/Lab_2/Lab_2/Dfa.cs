using System.Collections.Generic;
using System.IO;

namespace Lab_2
{
    public class Dfa
    {
        public Dfa(int startState, Set<int> acceptStates,
            IDictionary<int, IDictionary<string, int>> trans)
        {
            Start = startState;
            Accept = acceptStates;
            Trans = trans;
        }

        public int Start { get; }

        public Set<int> Accept { get; }

        public IDictionary<int, IDictionary<string, int>> Trans { get; }

        public override string ToString()
        {
            return "DFA start=" + Start + "\naccept=" + Accept;
        }

        // Write an input file for the dot program.  You can find dot at
        // http://www.research.att.com/sw/tools/graphviz/

        public void WriteDot(string filename)
        {
            var wr = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write));
            wr.WriteLine("// Format this file as a Postscript file with ");
            wr.WriteLine("//    dot " + filename + " -Tps -o out.ps\n");
            wr.WriteLine("digraph dfa {");
            wr.WriteLine("size=\"11,8.25\";");
            wr.WriteLine("rotate=90;");
            wr.WriteLine("rankdir=LR;");
            wr.WriteLine("n999999 [style=invis];"); // Invisible start node
            wr.WriteLine("n999999 -> n" + Start); // Edge into start state

            // Accept states are double circles
            foreach (var state in Trans.Keys)
                if (Accept.Contains(state))
                    wr.WriteLine("n" + state + " [peripheries=2];");

            // The transitions 
            foreach (var entry in Trans)
            {
                var s1 = entry.Key;
                foreach (var s1Trans in entry.Value)
                {
                    var lab = s1Trans.Key;
                    var s2 = s1Trans.Value;
                    wr.WriteLine("n" + s1 + " -> n" + s2 + " [label=\"" + lab + "\"];");
                }
            }

            wr.WriteLine("}");
            wr.Close();
        }
    }
}