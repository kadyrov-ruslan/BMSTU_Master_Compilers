namespace Lab_2
{
    internal abstract class Regex
    {
        public abstract Nfa MkNfa(Nfa.NameSource names);
    }

    internal class Eps : Regex
    {
        // The resulting nfa0 has form s0s -eps-> s0e
        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            nfa0.AddTrans(s0s, null, s0e);
            return nfa0;
        }
    }

    internal class Sym : Regex
    {
        private readonly string sym;

        public Sym(string sym)
        {
            this.sym = sym;
        }

        // The resulting nfa0 has form s0s -sym-> s0e
        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            nfa0.AddTrans(s0s, sym, s0e);
            return nfa0;
        }
    }

    internal class Seq : Regex
    {
        private readonly Regex r1;
        private readonly Regex r2;

        public Seq(Regex r1, Regex r2)
        {
            this.r1 = r1;
            this.r2 = r2;
        }

        // If   nfa1 has form s1s ----> s1e 
        // and  nfa2 has form s2s ----> s2e 
        // then nfa0 has form s1s ----> s1e -eps-> s2s ----> s2e
        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = r1.MkNfa(names);
            var nfa2 = r2.MkNfa(names);
            var nfa0 = new Nfa(nfa1.Start, nfa2.Exit);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            foreach (var entry in nfa2.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(nfa1.Exit, null, nfa2.Start);
            return nfa0;
        }
    }

    internal class Alt : Regex
    {
        private readonly Regex r1;
        private readonly Regex r2;

        public Alt(Regex r1, Regex r2)
        {
            this.r1 = r1;
            this.r2 = r2;
        }

        // If   nfa1 has form s1s ----> s1e 
        // and  nfa2 has form s2s ----> s2e 
        // then nfa0 has form s0s -eps-> s1s ----> s1e -eps-> s0e
        //                    s0s -eps-> s2s ----> s2e -eps-> s0e
        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = r1.MkNfa(names);
            var nfa2 = r2.MkNfa(names);
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            foreach (var entry in nfa2.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(s0s, null, nfa1.Start);
            nfa0.AddTrans(s0s, null, nfa2.Start);
            nfa0.AddTrans(nfa1.Exit, null, s0e);
            nfa0.AddTrans(nfa2.Exit, null, s0e);
            return nfa0;
        }
    }

    internal class Star : Regex
    {
        private readonly Regex r;

        public Star(Regex r)
        {
            this.r = r;
        }

        // If   nfa1 has form s1s ----> s1e 
        // then nfa0 has form s0s ----> s0s
        //                    s0s -eps-> s1s
        //                    s1e -eps-> s0s
        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = r.MkNfa(names);
            var s0s = names.next();
            var nfa0 = new Nfa(s0s, s0s);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(s0s, null, nfa1.Start);
            nfa0.AddTrans(nfa1.Exit, null, s0s);
            return nfa0;
        }
    }
}