namespace Lab4_Precedence.Elements
{
    public class Operator : Symbol
    {
        public enum Assoc { Left, Right }

        public int Prior { get; set; }
        public Assoc Ass { get; set; }

        public Operator(string symb, int prior, Assoc ass) : base(symb)
        {
            Prior = prior;
            Ass = ass;
        }
    }
}
