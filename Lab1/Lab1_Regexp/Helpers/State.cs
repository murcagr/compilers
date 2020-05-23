using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_Regexp.Helpers
{
    public class State
    {
        public int S { get; set; }
        public List<Translation> Translations { get; set; }

        private bool marked;
        private bool isIterating;

        public State(int s, List<Translation> translations)
        {
            S = s;
            Translations = translations;

            marked = true;
            isIterating = false;
        }

        public int Update()
        {
            int i = 0;

            DeleteMark();
            i = Update(i);

            return i;
        }

        public int Update(int i)
        {
            if (!marked)
            {
                marked = true;
                S = i++;
                foreach (var translation in Translations)
                {
                    i = translation.State.Update(i);
                }
            }

            return i;
        }

        public void DeleteMark()
        {
            if (marked)
            {
                marked = false;
                foreach (var translation in Translations)
                {
                    translation.State.DeleteMark();
                }
            }
        }

        public List<List<List<int>>> BuildTable(List<List<List<int>>> table, List<char> alphabet)
        {
            if (!isIterating)
            {
                isIterating = true;
                foreach (var translation in Translations)
                {
                    int letterI = alphabet.IndexOf(translation.Symbol);
                    if (!table[S][letterI].Contains(translation.State.S))
                        table[S][letterI].Add(translation.State.S);

                    table = translation.State.BuildTable(table, alphabet);
                }
                isIterating = false;
            }

            return table;
        }
    }
}
