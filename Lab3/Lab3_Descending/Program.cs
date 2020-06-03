using Lab3_Descending.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab3_Descending
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Читаем из файла нетерминалы, терминалы и продукции
            Dictionary<NonTerminal, List<Generation>> productions = XmlWorker.GrammarReader("Grammar_noLRM.xml", out List<Terminal> terminals,
                out List<NonTerminal> nonTerminals, out NonTerminal start);
            SaveReadableFormat("grammar_in.txt", terminals, nonTerminals, productions);

            string input = "";
            while (true)
            {
                Console.Write("Input string: ");
                input = Console.ReadLine();
                if (input == "exit")
                    break;

                input = input.Replace(" ", string.Empty);

                if (RecAddProc.Parse(input))
                    Console.WriteLine("Ok!\n");
                else
                    Console.WriteLine("Not ok!\n");
            }
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
