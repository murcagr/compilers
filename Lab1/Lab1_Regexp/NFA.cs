using Lab1_Regexp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_Regexp
{
    public class NFA
    {
        public State Initial { get; set; }
        public State Finish { get; set; }
        public int Size { get; set; }

        public NFA(char symb)
        {
            Finish = new State(1, new List<Translation>());
            Initial = new State(0, new List<Translation>() { new Translation(Finish, symb) });
            Size = 2;
        }

        public void Concat(NFA second)
        {
            Finish.Translations.Add(new Translation(second.Initial, Translation.Eps));  // Соединяем первый финиш и второе начало по ε
            Finish = second.Finish;  // Сохраняем новый финиш

            Size = UpdateStates();  // Обновляем номера состояний
        }

        public void Alter(NFA second)
        {
            State newFinish = new State(1, new List<Translation>());  // Создаем новый финиш
            Finish.Translations.Add(new Translation(newFinish, Translation.Eps));  // Добавляем старому финишу первого автомата переход в новый  
            second.Finish.Translations.Add(new Translation(newFinish, Translation.Eps));  // Добавляем старому финишу второго автомата переход в новый
            Finish = newFinish;  // Сохраняем новый финиш на место старого
            State oldInit = Initial;  // Сохраняем старое начало
            Initial = new State(0, new List<Translation>() { new Translation(oldInit, Translation.Eps),
                new Translation(second.Initial, Translation.Eps) });  // Создаем новое начало с двумя ε переходами в данные автоматы

            Size = UpdateStates();  // Обновляем номера состояний
        }

        public void Star()
        {
            State newFinish = new State(1, new List<Translation>());  // Создаем новый финиш
            Finish.Translations.Add(new Translation(Initial, Translation.Eps, true));  // Добавляем старому финишу ε переход в старое начало
            Finish.Translations.Add(new Translation(newFinish, Translation.Eps));  // Добавляем старому финишу переход в новый  
            Finish = newFinish;  // Сохраняем новый финиш на место старого
            State oldInit = Initial;  // Сохраняем старое начало
            Initial = new State(0, new List<Translation>() { new Translation(oldInit, Translation.Eps), 
                new Translation(Finish, Translation.Eps) });  // Создаем новое начало с двумя ε переходами в старое начало и в новый конец

            Size = UpdateStates();  // Обновляем номера состояний
        }

        public void Plus()
        {
            State newFinish = new State(1, new List<Translation>());  // Создаем новый финиш
            Finish.Translations.Add(new Translation(Initial, Translation.Eps, true));  // Добавляем старому финишу ε переход в старое начало
            Finish.Translations.Add(new Translation(newFinish, Translation.Eps));  // Добавляем старому финишу переход в новый  
            Finish = newFinish;  // Сохраняем новый финиш на место старого
            State oldInit = Initial;  // Сохраняем старое начало
            Initial = new State(0, new List<Translation>() { new Translation(oldInit, Translation.Eps) });  // Создаем новое начало с ε переходом в старое начало

            Size = UpdateStates();  // Обновляем номера состояний
        }

        public List<List<List<int>>> BuildTable(List<char> alphabet)
        {
            List<List<List<int>>> table = new List<List<List<int>>>(Size);
            for (int i = 0; i < Size; i++)
            {
                table.Add(new List<List<int>>(alphabet.Count));
                for (int j = 0; j < alphabet.Count; j++)
                    table[i].Add(new List<int>());
            }

            table = Initial.BuildTable(table, alphabet);

            return table;
        }



        public void PrintTable(List<List<List<int>>> table)
        {
            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; i < table[i].Count; i++)
                {
                    Console.WriteLine("State {0} goes by {1} to state {2}", i, j, table[i][j]);
                }
            }
        }

        private int UpdateStates()
        {
            return Initial.Update();
        }
    }
}
