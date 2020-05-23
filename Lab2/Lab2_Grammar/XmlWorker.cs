using Lab2_Grammar.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lab2_Grammar
{
    public static class XmlWorker
    {
        public static Dictionary<NonTerminal, List<Generation>> GrammarReader(string filename, out List<Terminal> terminals, 
            out List<NonTerminal> nonTerminals)
        {
            var grammarXml = XDocument.Load(filename);

            // Read terms and nonterms in temp vars
            List<Terminal> terminalsT = grammarXml.Descendants("term").Select(i => new Terminal
            {
                Name = (string) i.Attribute("name"),
                Spell = (string) i.Attribute("spell")
            }).ToList();
            List<NonTerminal> nonTerminalsT = grammarXml.Descendants("nonterm").Select(i => new NonTerminal
            {
                Name = (string) i.Attribute("name")
            }).ToList();

            // Save start symbol index
            nonTerminalsT.Find(nt => nt.Name == (string) grammarXml.Root.Element("startsymbol").Attribute("name")).IsStart = true;

            Dictionary<NonTerminal, List<Generation>> productions = new Dictionary<NonTerminal, List<Generation>>(nonTerminalsT.Count);
            // Read productions
            foreach (var production in grammarXml.Descendants("production"))
            {
                NonTerminal nonTerm = nonTerminalsT.Find(t => t.Name == (string) production.Element("lhs").Attribute("name"));
                Generation generation = new Generation
                {
                    Gen = production.Element("rhs").Elements().Select((XElement i) =>
                    {
                        if ((string) i.Attribute("type") == "term")
                            return (Symbol) terminalsT.Find(t => (string) i.Attribute("name") == t.Name);
                        else if ((string) i.Attribute("type") == "nonterm")
                            return (Symbol) nonTerminalsT.Find(nt => (string) i.Attribute("name") == nt.Name);
                        else
                            return null;
                    }).ToList()
                };

                if (productions.TryGetValue(nonTerm, out List<Generation> gens))
                {
                    gens.Add(generation);
                    productions.Remove(nonTerm);
                    productions.Add(nonTerm, gens);
                }
                else
                {
                    productions.Add(nonTerm, new List<Generation>() { generation });
                }
            }

            terminals = terminalsT;
            nonTerminals = nonTerminalsT;
            return productions;
        }

        public static void GrammarWriter(string filename, List<Terminal> terminals, List<NonTerminal> nonTerminals, 
            Dictionary<NonTerminal, List<Generation>> productions)
        {
            XElement terminalsymbols = new XElement("terminalsymbols");
            foreach (var terminal in terminals)
                terminalsymbols.Add(new XElement("term", new XAttribute("name", terminal.Name), new XAttribute("spell", terminal.Spell)));

            XElement nonterminalsymbols = new XElement("nonterminalsymbols");
            XElement startsymbol = null;
            foreach (var nonterminal in nonTerminals)
            {
                if (nonterminal.IsStart)
                    startsymbol = new XElement("startsymbol", new XAttribute("name", nonterminal.Name));

                nonterminalsymbols.Add(new XElement("term", new XAttribute("name", nonterminal.Name)));
            }

            XElement xProductions = new XElement("productions");
            foreach (var produciton in productions)
            {
                XElement lhs = new XElement("lhs", new XAttribute("name", produciton.Key.Name));
                
                foreach (var generation in produciton.Value)
                {
                    XElement rhs = new XElement("rhs");
                    foreach (var symbol in generation.Gen)
                    {
                        rhs.Add(new XElement("symbol", new XAttribute("type", symbol is Terminal ? "term" : "nonterm"), 
                            new XAttribute("name", symbol.Name)));
                    }

                    xProductions.Add(new XElement("production", lhs, rhs));
                }
            }

            XElement grammar = new XElement("grammar", terminalsymbols, nonterminalsymbols, xProductions, startsymbol, 
                new XAttribute("name", "G"));
            XDocument xDoc = new XDocument(grammar);
            xDoc.Save(filename);
        }
    }
}
