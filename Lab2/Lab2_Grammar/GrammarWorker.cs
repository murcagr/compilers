using Lab2_Grammar.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2_Grammar
{
    public static class GrammarWorker
    {

        public static Dictionary<NonTerminal, List<Generation>> LRElimination(List<Terminal> terminals, 
            List<NonTerminal> nonTerminals, Dictionary<NonTerminal, List<Generation>> productions)
        {
            // Cписок нетерминалов
            var productionNonTerms = productions.Keys.ToList();
            int ntCount = productionNonTerms.Count;

            for (int i = 0; i < ntCount; i++)
            {
                
                List<Generation> currGens = new List<Generation>(productions[productionNonTerms[i]]);
                // Indirect recur
                for (int j = 0; j < i; j++)
                {
                    int gensCount = currGens.Count;
                    for (int k = 0; k < gensCount; k++)
                    {
                        
                        Generation gen = new Generation(currGens[k]);

                        if (gen.Gen.First().Name == productionNonTerms[j].Name)
                        {
                            List<Generation> jGens = new List<Generation>(productions[productionNonTerms[j]]);

                            currGens.RemoveAt(k);
                            gen.Gen.RemoveAt(0);  // Остается a

                            foreach (var jGen in jGens)
                            {
                                Generation newJGen = new Generation(jGen);
                                newJGen.Gen.AddRange(gen.Gen);
                                currGens.Add(newJGen);
                            }
                        }
                    }
                }

                productions.Remove(productionNonTerms[i]);


                // Direct
                if (currGens.Any(g => g.Gen.First().Name == productionNonTerms[i].Name))
                {
                    nonTerminals.Add(new NonTerminal() { Name = productionNonTerms[i].Name + "'" });
                    List<Generation> newNTGens = new List<Generation>();
                    List<Generation> oldNTGens = new List<Generation>();

                    foreach (var gen in currGens)
                    {
                        if (gen.Gen.First().Name == productionNonTerms[i].Name)
                        {
                            gen.Gen.RemoveAt(0);

                            Generation newGen = new Generation();
                            newGen.Gen.AddRange(gen.Gen);
                            newGen.Gen.Add(nonTerminals.Last());
                            newNTGens.Add(newGen);
                        }
                        else  
                        {
                            
                            Generation newGen = new Generation();
                            
                            newGen.Gen.AddRange(gen.Gen);
                            
                            newGen.Gen.Add(nonTerminals.Last());
                            oldNTGens.Add(newGen);
                        }
                    }
                    
                    newNTGens.Add(new Generation() { Gen = new List<Symbol>() { terminals.Find(t => t.Name == "EPS") } });

                    
                    productions.Add(productionNonTerms[i], oldNTGens);
                    productions.Add(nonTerminals.Last(), newNTGens);
                }
                else  
                {
                    
                    productions.Add(productionNonTerms[i], currGens);
                }
            }

            return productions;
        }


        public static Dictionary<NonTerminal, List<Generation>> chainRuleElimination(List<Terminal> terminals,
            List<NonTerminal> nonTerminals, Dictionary<NonTerminal, List<Generation>> productions)
        {
            // Список Na, Nb, Nc ... , где a,b,c - нетерминалы 
            var Nlit = new Dictionary<NonTerminal, HashSet<NonTerminal>>();

            // Для каждого нетерминала
            for (int i = 0; i < nonTerminals.Count; i++)
            {
                // Na = {B|A -> B} 
                var N = new List<HashSet<NonTerminal>>();
                // Добавляем в N0 текущий нетерминал 
                N.Add(new HashSet<NonTerminal>() { nonTerminals[i]} );
                int j = 1;

                // Ищем такое C, что B->C
                foreach (var production in productions)
                {
                    // Если B принадлежит N-1
                    if (N[j - 1].Contains(production.Key))
                    {
                        foreach (var value in production.Value)
                        {
                            // Если мы нашли C, удовл B->C
                            if (value.Gen.Count == 1 && nonTerminals.Contains(value.Gen[0]))
                            {
                                // Добавить в cписок Ni C
                                var tmp = new HashSet<NonTerminal>() { (NonTerminal)value.Gen[0] };
                                var tmp2 = N[j - 1];
                                // Объединяем Nj и Nj-1
                                tmp.UnionWith(tmp2);
                                // Добавляем в Na
                                N.Add(tmp);
                                
                                // Если текущий Nj отличается от Nj-1 - продолжить
                                if (!N[j].SetEquals(N[j - 1]))
                                    j++;
                            }

                        }
                    }
                
                }
                // Добавить в список Na последний элемент Nj
                Nlit.Add(nonTerminals[i], N[N.Count - 1]);
            }

            var newProductions = new Dictionary<NonTerminal, List<Generation>>();

            // Идем по всем продукциям
            foreach (var production in productions)
            {

                foreach (var value in production.Value)
                {
                    // Если правило не цепное
                    if (!(value.Gen.Count == 1 && nonTerminals.Contains(value.Gen[0])))
                    {
                        // Для всех Na 
                        foreach (var Nl in Nlit)
                        {
                            // Если среди Na есть есть текущий нетерминал
                            if (Nl.Value.Contains(production.Key))
                            {
                                if (newProductions.ContainsKey(Nl.Key))
                                {
                                    newProductions[Nl.Key].Add(value);
                                }
                                else
                                {
                                    newProductions.Add(Nl.Key, new List<Generation>() { value });
                                }
                            }
                        }
                    }
                }
            }

            return newProductions;
        }

    }
}
