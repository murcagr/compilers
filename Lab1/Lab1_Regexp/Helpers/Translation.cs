using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_Regexp.Helpers
{
    public class Translation
    {
        public const char Eps = 'ε';
        public const char None = '\0';

        public State State { get; set; }
        public char Symbol { get; set; }
        public bool IsCycleTranslation { get; set; }

        public Translation(State state, char symb)
        {
            State = state;
            Symbol = symb;
            IsCycleTranslation = false;
        }

        public Translation(State state, char symb, bool isCycle)
        {
            State = state;
            Symbol = symb;
            IsCycleTranslation = isCycle;
        }
    }
}
