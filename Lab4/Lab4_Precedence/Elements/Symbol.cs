namespace Lab4_Precedence.Elements
{
    public class Symbol
    {
        public string Symb { get; set; }
        public string Value { get; set; }
        public Symbol(string symb)
        {
            Symb = symb;
        }
        public Symbol(string symb, string value)
        {
            Symb = symb;
            Value = value;
        }
    }
}
