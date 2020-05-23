using Lab2_Grammar.Elements;
using Lab2_Grammar.Helpers;
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
                // Копируем порождения Ai нетерминала
                List<Generation> currGens = new List<Generation>(productions[productionNonTerms[i]]);

                for (int j = 0; j < i; j++)
                {
                    int gensCount = currGens.Count;
                    for (int k = 0; k < gensCount; k++)
                    {
                        // Копируем одно k-тое порождение Ai нетерминала
                        Generation gen = new Generation(currGens[k]);

                        // Если оно имеет вид Ai -> Aja
                        if (gen.Gen.First().Name == productionNonTerms[j].Name)
                        {
                            // Копируем порождения Aj нетерминала
                            List<Generation> jGens = new List<Generation>(productions[productionNonTerms[j]]);

                            // Удаляем это k-тое порождение, чтобы заменить его потом раскрытыми порождениями Aj нетерминала
                            currGens.RemoveAt(k);
                            // Удаляем первый нетерминал (Aj) в k-том порождении, чтобы заменить его после продукциями Aj
                            gen.Gen.RemoveAt(0);  // Остается a

                            // Проходим по всем порождениям Aj нетрминала
                            foreach (var jGen in jGens)
                            {
                                // Копируем очередное порождение Aj
                                Generation newJGen = new Generation(jGen);
                                // Добавляем к нему k-тое порождение Ai без первого нетерминала (ранее удаленного)
                                newJGen.Gen.AddRange(gen.Gen);
                                // Вставляем получившееся раскрытое порождение в продукции Ai нетерминала
                                currGens.Add(newJGen);
                            }
                        }
                    }
                }

                // Удаляем из словаря старые продукции от нетерминала Ai
                productions.Remove(productionNonTerms[i]);

                // Если есть леворекурсивные порождения
                if (currGens.Any(g => g.Gen.First().Name == productionNonTerms[i].Name))
                {
                    // Добавляем новый нетерминал отличающий от текущего на ' (Ai и Ai')
                    nonTerminals.Add(new NonTerminal() { Name = productionNonTerms[i].Name + "'" });
                    // Новые продукции от нового нетерминала Ai'
                    List<Generation> newNTGens = new List<Generation>();
                    // новые продукции от старого нетерминала Ai
                    List<Generation> oldNTGens = new List<Generation>();

                    // Для каждого порождения Ai нетерминала (Ai -> Aia | b)
                    foreach (var gen in currGens)
                    {
                        // Если это леворекурсивное порождение => должно иметь вид Ai' -> aAi'
                        if (gen.Gen.First().Name == productionNonTerms[i].Name)
                        {
                            // Удаляем первый "рекурсивный" нетерминал (само Ai) из исходного порождения
                            gen.Gen.RemoveAt(0);

                            // Создаем новое порождение
                            Generation newGen = new Generation();
                            // Вставляем в начало порождения исходное порождение без первого символа (a)
                            newGen.Gen.AddRange(gen.Gen);
                            // Добавляем новый только что добавленный нетерминал (Ai')
                            newGen.Gen.Add(nonTerminals.Last());
                            newNTGens.Add(newGen);
                        }
                        else  // Если не леворекурсивное порождение => должно иметь вид Ai -> bAi'
                        {
                            // Создаем новое порождение
                            Generation newGen = new Generation();
                            // Вставляем в начало порождения исходное не леворекурсивное порождение (b)
                            newGen.Gen.AddRange(gen.Gen);
                            // Добавляем новый только что добавленный нетерминал (Ai')
                            newGen.Gen.Add(nonTerminals.Last());
                            oldNTGens.Add(newGen);
                        }
                    }
                    // Добавляем к продукциям от нового нетерминала Ai' продукцию Ai' -> eps
                    newNTGens.Add(new Generation() { Gen = new List<Symbol>() { terminals.Find(t => t.Name == "EPS") } });

                    // Добавляем в словарь новые продукции от нетерминала Ai
                    productions.Add(productionNonTerms[i], oldNTGens);
                    // Добавляем в словарь новые продукции от нетерминала Ai'
                    productions.Add(nonTerminals.Last(), newNTGens);
                }
                else  // Если леворекурсивных порождений нет - просто оставляем раскрытые порождения
                {
                    // Добавляем в словарь новые раскрытые продукции от нетерминала Ai
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
