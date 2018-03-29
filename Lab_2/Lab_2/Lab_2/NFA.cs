using System.Collections.Generic;

namespace Lab_2
{
    public class Nfa
    {
        public Nfa(int startState, int exitState)
        {
            Start = startState;
            Exit = exitState;
            Trans = new Dictionary<int, List<Transition>>();
            if (!startState.Equals(exitState))
                Trans.Add(exitState, new List<Transition>());
        }

        public int Start { get; }

        public int Exit { get; }

        public IDictionary<int, List<Transition>> Trans { get; }

        public void AddTrans(int s1, string lab, int s2)
        {
            List<Transition> s1Trans;
            if (Trans.ContainsKey(s1))
                s1Trans = Trans[s1];
            else
            {
                s1Trans = new List<Transition>();
                Trans.Add(s1, s1Trans);
            }

            s1Trans.Add(new Transition(lab, s2));
        }

        public void AddTrans(KeyValuePair<int, List<Transition>> tr)
        {
            // Assumption: if tr is in trans, it maps to an empty list (end state)
            Trans.Remove(tr.Key);
            Trans.Add(tr.Key, tr.Value);
        }

        public override string ToString()
        {
            return "NFA start=" + Start + " exit=" + Exit;
        }

        // Construct the transition relation of a composite-state DFA
        // from an NFA with start state s0 and transition relation
        // trans (a Map from int to List of Transition).  The start
        // state of the constructed DFA is the epsilon closure of s0,
        // and its transition relation is a Map from a composite state
        // (a Set of ints) to a Map from label (a String) to a
        // composite state (a Set of ints).
        private static IDictionary<Set<int>, IDictionary<string, Set<int>>> CompositeDfaTrans(int s0, IDictionary<int, List<Transition>> trans)
        {
            var S0 = EpsilonClose(new Set<int>(s0), trans);
            var worklist = new Queue<Set<int>>();
            worklist.Enqueue(S0);
            // The transition relation of the DFA
           var res = new Dictionary<Set<int>, IDictionary<string, Set<int>>>();
            while (worklist.Count != 0)
            {
                var S = worklist.Dequeue();
                if (!res.ContainsKey(S))
                {
                    // The S -lab-> T transition relation being constructed for a given S
                    var STrans = new Dictionary<string, Set<int>>();
                    // For all s in S, consider all transitions s -lab-> t
                    foreach (var s in S)
                        // For all non-epsilon transitions s -lab-> t, add t to T
                        foreach (var tr in trans[s])
                            if (tr.lab != null)
                            {
                                // Already a transition on lab
                                Set<int> toState;
                                if (STrans.ContainsKey(tr.lab))
                                    toState = STrans[tr.lab];
                                else
                                {
                                    // No transitions on lab yet
                                    toState = new Set<int>();
                                    STrans.Add(tr.lab, toState);
                                }

                                toState.Add(tr.target);
                            }

                    // Epsilon-close all T such that S -lab-> T, and put on worklist
                    var STransClosed = new Dictionary<string, Set<int>>();
                    foreach (var entry in STrans)
                    {
                        var Tclose = EpsilonClose(entry.Value, trans);
                        STransClosed.Add(entry.Key, Tclose);
                        worklist.Enqueue(Tclose);
                    }

                    res.Add(S, STransClosed);
                }
            }

            return res;
        }

        // Compute epsilon-closure of state set S in transition relation trans.  
        private static Set<int> EpsilonClose(Set<int> S, IDictionary<int, List<Transition>> trans)
        {
            // The worklist initially contains all S members
            var worklist = new Queue<int>(S);
            var res = new Set<int>(S);
            while (worklist.Count != 0)
            {
                var s = worklist.Dequeue();
                foreach (var tr in trans[s])
                    if (tr.lab == null && !res.Contains(tr.target))
                    {
                        res.Add(tr.target);
                        worklist.Enqueue(tr.target);
                    }
            }

            return res;
        }

        // Compute a renamer, which is a Map from Set of int to int
        private static IDictionary<Set<int>, int> MkRenamer(ICollection<Set<int>> states)
        {
            var renamer = new Dictionary<Set<int>, int>();
            var count = 0;
            foreach (var k in states)
                renamer.Add(k, count++);
            return renamer;
        }

        // Using a renamer (a Map from Set of int to int), replace
        // composite (Set of int) states with simple (int) states in
        // the transition relation trans, which is assumed to be a Map
        // from Set of int to Map from String to Set of int.  The
        // result is a Map from int to Map from String to int.
        private static IDictionary<int, IDictionary<string, int>>
            Rename(IDictionary<Set<int>, int> renamer, IDictionary<Set<int>, IDictionary<string, Set<int>>> trans)
        {
            var newtrans = new Dictionary<int, IDictionary<string, int>>();
            foreach (var entry
                in trans)
            {
                var k = entry.Key;
                IDictionary<string, int> newktrans = new Dictionary<string, int>();
                foreach (var tr in entry.Value)
                    newktrans.Add(tr.Key, renamer[tr.Value]);
                newtrans.Add(renamer[k], newktrans);
            }

            return newtrans;
        }

        private static Set<int> AcceptStates(ICollection<Set<int>> states,
            IDictionary<Set<int>, int> renamer, int exit)
        {
            var acceptStates = new Set<int>();
            foreach (var state in states)
                if (state.Contains(exit))
                    acceptStates.Add(renamer[state]);
            return acceptStates;
        }

        public Dfa ToDfa()
        {
            var cDfaTrans = CompositeDfaTrans(Start, Trans);
            var cDfaStart = EpsilonClose(new Set<int>(Start), Trans);
            var cDfaStates = cDfaTrans.Keys;
            var renamer = MkRenamer(cDfaStates);
            var simpleDfaTrans = Rename(renamer, cDfaTrans);
            var simpleDfaStart = renamer[cDfaStart];
            var simpleDfaAccept = AcceptStates(cDfaStates, renamer, Exit);
            return new Dfa(simpleDfaStart, simpleDfaAccept, simpleDfaTrans);
        }

        // Nested class for creating distinctly named states when constructing NFAs

        public class NameSource
        {
            private static int nextName;

            public int next()
            {
                return nextName++;
            }
        }
    }

    /// <summary>
    /// a transition from one state to another
    /// </summary>
    public class Transition
    {
        public string lab;
        public int target;

        public Transition(string lab, int target)
        {
            this.lab = lab;
            this.target = target;
        }

        public override string ToString()
        {
            return "-" + lab + "-> " + target;
        }
    }
}