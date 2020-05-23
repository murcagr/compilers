using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_Regexp.Helpers
{
    public class Operator
    {
        public char Symbol { get; private set; }
        public int Priority { get; private set; }

        public Operator(char symb, int prior)
        {
            Symbol = symb;
            Priority = prior;
        }
    }
}
