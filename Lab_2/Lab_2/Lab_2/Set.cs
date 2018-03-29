using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab_2
{
    /// <summary>
    ///  A set represented as the collection of keys of a Dictionary
    /// </summary>
    public class Set<T> : ICollection<T>
    {
        // Only the keys matter; the type bool used for the value is arbitrary
        private readonly Dictionary<T, bool> dict;

        public Set()
        {
            dict = new Dictionary<T, bool>();
        }

        public Set(T x) : this()
        {
            Add(x);
        }

        public Set(IEnumerable<T> coll) : this()
        {
            foreach (var x in coll)
                Add(x);
        }

        public Set(T[] arr) : this()
        {
            foreach (var x in arr)
                Add(x);
        }

        public bool Contains(T x)
        {
            return dict.ContainsKey(x);
        }

        public void Add(T x)
        {
            if (!Contains(x))
                dict.Add(x, false);
        }

        public bool Remove(T x)
        {
            return dict.Remove(x);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            return dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => dict.Count;

        public void CopyTo(T[] arr, int i)
        {
            dict.Keys.CopyTo(arr, i);
        }

        // Is this set a subset of that?
        public bool Subset(Set<T> that)
        {
            foreach (var x in this)
                if (!that.Contains(x))
                    return false;
            return true;
        }

        // Create new set as intersection of this and that
        public Set<T> Intersection(Set<T> that)
        {
            var res = new Set<T>();
            foreach (var x in this)
                if (that.Contains(x))
                    res.Add(x);
            return res;
        }

        // Create new set as union of this and that
        public Set<T> Union(Set<T> that)
        {
            var res = new Set<T>(this);
            foreach (var x in that)
                res.Add(x);
            return res;
        }

        // Compute hash code -- should be cached for efficiency
        public override int GetHashCode()
        {
            var res = 0;
            foreach (var x in this)
                res ^= x.GetHashCode();
            return res;
        }

        public override bool Equals(object that)
        {
            if (that is Set<T>)
            {
                var thatSet = (Set<T>) that;
                return thatSet.Count == Count
                       && thatSet.Subset(this) && Subset(thatSet);
            }

            return false;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append("{ ");
            var first = true;
            foreach (var x in this)
            {
                if (!first)
                    res.Append(", ");
                res.Append(x);
                first = false;
            }

            res.Append(" }");
            return res.ToString();
        }
    }
}