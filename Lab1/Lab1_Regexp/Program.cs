using Lab1_Regexp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1_Regexp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //List<char> alphabet = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char) i).ToList();
            List<char> alphabet = Enumerable.Range('a', 'b' - 'a' + 1).Select(i => (Char) i).ToList();
            //alphabet = alphabet.Concat(Enumerable.Range('0', '9' - '0' + 1).Select(i => (Char) i)).ToList();
            List<char> epsAlphabet = alphabet.Concat(new char[]{ 'ε' }.ToList()).ToList();

            GraphVizBuilder graphViz = new GraphVizBuilder();

            // Read valid regexp
            Console.WriteLine("Enter RegExp to build FA:");
            string regexp = Console.ReadLine();

            // Preprocess regexp (adding dots where concatenating)
            string processedRegexp = Preprocess(regexp);

            // Process regexp to postfix
            string postfixRegexp = ToPostfix(processedRegexp);

            // Building NFA in graph from postfix regexp
            NFA nfa = ToNFA(postfixRegexp);
            var nfaTable = nfa.BuildTable(epsAlphabet);  // Building NFA in table from graph
            

            // Draw NFA
            string graphNFA = graphViz.BuildString(epsAlphabet, nfaTable, nfa.Initial.S, new List<int>() { nfa.Finish.S });
            graphViz.CreateGraph(graphNFA, "nfa.png");

            // Building DFA from NFA
            DFA dfa = new DFA(nfaTable, alphabet, epsAlphabet.IndexOf(Translation.Eps), nfa.Initial.S, nfa.Finish.S);

            


            string graphDFA = graphViz.BuildString(alphabet, dfa.DFATable, dfa.Initial, dfa.Finish);
            graphViz.CreateGraph(graphDFA, "dfa.png");

            //R

            dfa.Reverse();
            string graphDFAr = graphViz.BuildString(dfa.Alphabet, dfa.DFATable, dfa.Initial, dfa.Finish);
            graphViz.CreateGraph(graphDFAr, "dfa_r.png");

            //RD
            dfa.MakeDfa();

            string graphDFArd = graphViz.BuildString(dfa.Alphabet, dfa.DFATable, dfa.Initial, dfa.Finish);
            graphViz.CreateGraph(graphDFArd, "dfa_rd.png");

            //RDR
            
            dfa.Reverse();
            string graphDFArdr = graphViz.BuildString(dfa.Alphabet, dfa.DFATable, dfa.Initial, dfa.Finish);
            graphViz.CreateGraph(graphDFArdr, "dfa_rdr.png");

            //RDRD
            dfa.MakeDfa();
            string graphDFArdrd = graphViz.BuildString(dfa.Alphabet, dfa.DFATable, dfa.Initial, dfa.Finish);
            graphViz.CreateGraph(graphDFArdrd, "dfa_rdrd.png");

            while (true)
            {
                Console.WriteLine("Enter word to check allowness:");
                string word = Console.ReadLine();
                if (word == "`")
                    break;

                // Model FA for given word
                bool res = dfa.Model(word, alphabet);
                Console.WriteLine($"Result: {res}\n");
            }
        }

        public static string Preprocess(string regexp)
        {
            StringBuilder nRegexp = new StringBuilder();

            CharEnumerator c, up;
            c = regexp.GetEnumerator();
            up = regexp.GetEnumerator();

            up.MoveNext();

            while (up.MoveNext())
            {
                c.MoveNext();

                nRegexp.Append(c.Current);

                if ((char.IsLetterOrDigit(c.Current) || c.Current == ')' || c.Current == '*' ||
                  c.Current == '?' || c.Current == '+') && up.Current != ')' && up.Current != '|' &&
                  up.Current != '*' && up.Current != '?' && up.Current != '+')
                    nRegexp.Append('.');
            }

           if (c.MoveNext())
                nRegexp.Append(c.Current);

            return nRegexp.ToString();
        }

        public static string ToPostfix(string regexp)
        {
            List<Operator> ops = new List<Operator>() { new Operator('*', 3), new Operator('+', 3), new Operator('?', 3),
                new Operator('.', 2), new Operator('|', 1), new Operator('(', 0), new Operator(')', 0) };

            StringBuilder postfix = new StringBuilder();
            Stack<char> operators = new Stack<char>();

            CharEnumerator c = regexp.GetEnumerator();

            while (c.MoveNext())
            {
                if (char.IsLetterOrDigit(c.Current))
                    postfix.Append(c.Current);
                else if (c.Current == ops[5].Symbol)  // Open bracket
                    operators.Push(c.Current);
                else if (c.Current == ops[6].Symbol)  // Close bracket
                {
                    while (operators.Peek() != ops[5].Symbol)  
                        postfix.Append(operators.Pop());

                    operators.Pop();  
                }
                else if (ops.Any(op => op.Symbol == c.Current))
                {
                    while (operators.Count > 0 && 
                        ops.Find(op => op.Symbol == operators.Peek()).Priority >= ops.Find(op => op.Symbol == c.Current).Priority)
                        postfix.Append(operators.Pop());

                    operators.Push(c.Current);
                }
            }

            while (operators.Count > 0)
                postfix.Append(operators.Pop());

            return postfix.ToString();
        }

        public static NFA ToNFA(string postfix)
        {
            Stack<NFA> nfas = new Stack<NFA>();

            CharEnumerator c = postfix.GetEnumerator();

            while (c.MoveNext())
            {
                if (char.IsLetterOrDigit(c.Current))
                    nfas.Push(new NFA(c.Current));
                else if (c.Current == '.')
                {
                    NFA b = nfas.Pop();
                    NFA a = nfas.Pop();
                    a.Concat(b);

                    nfas.Push(a);
                }
                else if (c.Current == '|')
                {
                    NFA b = nfas.Pop();
                    NFA a = nfas.Pop();
                    a.Alter(b);

                    nfas.Push(a);
                }
                else if (c.Current == '*')
                {
                    nfas.Peek().Star();
                }
                else if (c.Current == '+')
                {
                    nfas.Peek().Plus();
                }
            }

            return nfas.Pop();
        }
    }
}
