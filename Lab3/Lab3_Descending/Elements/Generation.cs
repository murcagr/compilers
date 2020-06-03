using System.Collections.Generic;
using System.Text;

namespace Lab3_Descending.Elements
{
    /// <summary>
    /// Порождение - одна из продукций нетерминала
    /// </summary>
    public class Generation
    {
        /// <summary>
        /// Все элементы порождения (терминалы и нетерминалы)
        /// </summary>
        public List<Symbol> Gen { get; set; } = new List<Symbol>();

        public string GenString { get
            {
                StringBuilder sb = new StringBuilder(Gen.Count);

                foreach (var symbol in Gen)
                    sb.Append(symbol is NonTerminal ? symbol.Name : (symbol as Terminal).Spell);

                return sb.ToString();
            } 
        }

        public Generation()
        {

        }

        public Generation(Generation copyGen)
        {
            Gen = new List<Symbol>(copyGen.Gen);
        }
    }
}
