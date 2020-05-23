using Lab2_Grammar.Elements;
using System.Collections.Generic;

namespace Lab2_Grammar.Helpers
{
    public class GenerationCountComparer : IComparer<Generation>
    {
        public int Compare(Generation a, Generation b)
        {
            if (a.Gen.Count == b.Gen.Count)
                return 0;
            if (a.Gen.Count > b.Gen.Count)
                return -1;

            return 1;
        }
    }
}
