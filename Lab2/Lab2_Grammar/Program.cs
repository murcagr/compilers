using Lab2_Grammar.Elements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab2_Grammar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Устранение левой рекурсии у грамматики G0
            Dictionary<NonTerminal, List<Generation>> productions = XmlWorker.GrammarReader("Grammar0.xml", out List<Terminal> terminals,
                out List<NonTerminal> nonTerminals);
            SaveReadableFormat("grammar_in.txt", terminals, nonTerminals, productions);
            
            productions = GrammarWorker.LRElimination(terminals, nonTerminals, productions);
            SaveReadableFormat("grammar_out_noLR.txt", terminals, nonTerminals, productions);
            XmlWorker.GrammarWriter("Grammar_noLR.xml", terminals, nonTerminals, productions);

            // Устранение левой рекурсии у грамматики G1
            Dictionary<NonTerminal, List<Generation>> productions2 = XmlWorker.GrammarReader("Grammar1.xml", out List<Terminal> terminals2,
                out List<NonTerminal> nonTerminals2);
            SaveReadableFormat("grammar_in2.txt", terminals2, nonTerminals2, productions2);

            productions2 = GrammarWorker.LRElimination(terminals2, nonTerminals2, productions2);
            SaveReadableFormat("grammar_out_noLR2.txt", terminals2, nonTerminals2, productions2);
            XmlWorker.GrammarWriter("Grammar_noLR2.xml", terminals2, nonTerminals2, productions2);

            // Устранение цепных правил  и левой рекурсии у грамматики G0 

            Dictionary<NonTerminal, List<Generation>> productions3 = XmlWorker.GrammarReader("Grammar0.xml", out List<Terminal> terminals3,
                out List<NonTerminal> nonTerminals3);
            SaveReadableFormat("grammar_in3.txt", terminals3, nonTerminals3, productions3);

            productions3 = GrammarWorker.chainRuleElimination(terminals3, nonTerminals3, productions3);
            SaveReadableFormat("grammar_out_noCR3.txt", terminals3, nonTerminals3, productions3);
            XmlWorker.GrammarWriter("Grammar_noCR3.xml", terminals3, nonTerminals3, productions3);
            productions3 = GrammarWorker.chainRuleElimination(terminals3, nonTerminals3, productions3);
            SaveReadableFormat("grammar_out_noLR3.txt", terminals3, nonTerminals3, productions3);
            XmlWorker.GrammarWriter("Grammar_noLR3.xml", terminals3, nonTerminals3, productions3);


        }

        private static void SaveReadableFormat(string filename, List<Terminal> terminals, List<NonTerminal> nonTerminals,
            Dictionary<NonTerminal, List<Generation>> productions)
        {
            StringBuilder terms = new StringBuilder("T: ");
            foreach (var terminal in terminals)
                terms.Append($"{terminal.Spell}, ");

            StringBuilder nonterms = new StringBuilder("NT: ");
            string start = "";
            foreach (var nonTerminal in nonTerminals)
            {
                nonterms.Append($"{nonTerminal.Name}, ");
                if (nonTerminal.IsStart)
                    start = nonTerminal.Name;
            }

            List<StringBuilder> sProductions = new List<StringBuilder>();
            foreach (var produciton in productions)
            {
                StringBuilder sProduction = new StringBuilder($"{produciton.Key.Name} -> ");
                foreach (var generation in produciton.Value)
                {
                    sProduction.Append(generation.GenString);
                    sProduction.Append(" | ");
                }
                sProductions.Add(sProduction);
            }

            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.WriteLine(terms.ToString());
                sw.WriteLine(nonterms.ToString());
                foreach (var sProduction in sProductions)
                    sw.WriteLine(sProduction.ToString());
                sw.WriteLine(start);
            }
        }
    }
}
