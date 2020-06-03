using Lab3_Descending.Elements;
using System.Collections.Generic;

namespace Lab3_Descending.Helpers
{
    public class GenerationCountComparer : IComparer<Generation>
    {
        public int Compare(Generation a, Generation b)
        {
            if (a.GenString.Length == b.GenString.Length)
                return 0;
            if (a.GenString.Length > b.GenString.Length)
                return -1;

            return 1;
        }
    }
}
