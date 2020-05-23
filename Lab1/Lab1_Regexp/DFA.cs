using Lab1_Regexp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1_Regexp
{
    public class DFA
    {
        public List<List<List<int>>> DFATable { get; private set; }
        public int Initial { get; set; }
        public List<int> Finish { get; private set; }
        public List<int> NonFinish { get; private set; }

        public List<char> Alphabet { get; private set; }


        public bool isTrueDfa { get; set; }

        public DFA(List<List<List<int>>> nfaTable, List<char> alphabet, int epsI, int initState, int finishState)
        {
            this.MakeDfaFromNfa(nfaTable, alphabet, epsI, initState, finishState);
        }

        public void MakeDfaFromNfa(List<List<List<int>>> nfaTable, List<char> alphabet, int epsI, int initState, int finishState)
        {

            Finish = new List<int>();
            NonFinish = new List<int>();

            Initial = 0;
            isTrueDfa = true;
            Alphabet = new List<char>(alphabet);
            Alphabet.Remove('ε');
            List<HashSet<int>> DStates = new List<HashSet<int>>();
            Stack<HashSet<int>> stack = new Stack<HashSet<int>>();
            Stack<int> stackIndexes = new Stack<int>();
            List<List<int>> Dtran = new List<List<int>>();


            var epsClosure0 = EpsClosure(new HashSet<int>() { initState }, nfaTable, epsI);
            DStates.Add(epsClosure0);
            Dtran.Add(new List<int>(Enumerable.Repeat(-1, this.Alphabet.Count)));
            stack.Push(epsClosure0);
            stackIndexes.Push(0);


            while (stack.Count > 0)
            {
                var state = stack.Pop();
                var stateInd = stackIndexes.Pop();

                for (int i = 0; i < this.Alphabet.Count; i++)
                {
                    var nextStates = EpsClosure(Move(state, i, nfaTable), nfaTable, epsI);
                    int index = DStates.FindIndex(s => s.SetEquals(nextStates));
                    
                    //for (int k = 0; k < DStates.Count ; k++)
                    //{
                    //    bool isSuperset = new HashSet<int>(DStates[k]).IsSupersetOf(nextStates);
                    //    if (isSuperset)
                    //    {
                    //        index = DStates.IndexOf(state);
                    //        break;
                    //    }
                    //}

                    if (index == -1)
                    {
                        index = DStates.Count;
                        DStates.Add(nextStates);
                        Dtran.Add(new List<int>(Enumerable.Repeat(-1, this.Alphabet.Count)));
                        stack.Push(nextStates);
                        stackIndexes.Push(index);
                    }

                    Dtran[stateInd][i] = index;
                }
            }

            DFATable = new List<List<List<int>>>(DStates.Count);
            for (int i = 0; i < DStates.Count; i++)
            {
                DFATable.Add(new List<List<int>>(alphabet.Count));

                if (DStates[i].Contains(finishState))
                    Finish.Add(i);
                else
                    NonFinish.Add(i);

                for (int j = 0; j < this.Alphabet.Count; j++)
                {
                    DFATable[i].Add(new List<int>(1));
                    DFATable[i][j].Add(Dtran[i][j]);
                }
            }
        }


        public void MakeDfa()
        {
            if (this.Alphabet.Contains('ε'))
            {
                this.MakeDfaFromNfa(this.DFATable, this.Alphabet, this.Alphabet.IndexOf(Translation.Eps), this.Initial, this.Finish[0]);
            }
            else
            {
                this.MakeDfaFromDfa(this.DFATable, this.Initial, this.Alphabet, this.Finish[0]);
            }
        }



        public void MakeDfaFromDfa(List<List<List<int>>> nfaTable, int initState, List<char> alphabet, int finishState)
        {
            Finish = new List<int>();
            NonFinish = new List<int>();
            Initial = 0;

            List<HashSet<int>> DStates = new List<HashSet<int>>();
            Stack<HashSet<int>> stack = new Stack<HashSet<int>>();
            Stack<int> stackIndexes = new Stack<int>();
            List<List<int>> Dtran = new List<List<int>>();


            //DStates.Add(new HashSet<int>() { 0 });
            DStates.Add(new HashSet<int>() { initState });
            Dtran.Add(new List<int>(Enumerable.Repeat(-1, alphabet.Count)));
            stack.Push(new HashSet<int>() { initState });
            stackIndexes.Push(0);


            while (stack.Count > 0)
            {
                var state = stack.Pop();
                var stateInd = stackIndexes.Pop();

                for (int i = 0; i < alphabet.Count; i++)
                {
                    var nextStates = Move(state, i, nfaTable);
                    int index = DStates.FindIndex(s => s.SetEquals(nextStates));
                    if (index == -1)
                    {
                        index = DStates.Count;
                        DStates.Add(nextStates);
                        Dtran.Add(new List<int>(Enumerable.Repeat(-1, alphabet.Count)));
                        stack.Push(nextStates);
                        stackIndexes.Push(index);
                    }

                    Dtran[stateInd][i] = index;
                }
            }

            DFATable = new List<List<List<int>>>(DStates.Count);
            for (int i = 0; i < DStates.Count; i++)
            {
                DFATable.Add(new List<List<int>>(alphabet.Count));

                if (DStates[i].Contains(finishState))
                    Finish.Add(i);
                else
                    NonFinish.Add(i);

                for (int j = 0; j < alphabet.Count; j++)
                {
                    DFATable[i].Add(new List<int>(1));
                    DFATable[i][j].Add(Dtran[i][j]);
                }
            }

        }


        public void MakeOneFinalState()
        {
            // It converts dfa to nfa
            if (this.Finish.Count > 1)
            {
                this.Alphabet = this.Alphabet.Concat(new char[] { 'ε' }.ToList()).ToList();

                // Adding eps trans to nothing for every state
                for (int i = 0; i < this.DFATable.Count; i++)
                {
                    this.DFATable[i].Add(new List<int>());
                }

                // Adding new state to direct  prev final states to this one
                var newFinalState = new List<List<int>>();

                // Adding empty trans
                for (int i = 0; i < Alphabet.Count; i++)
                    newFinalState.Add(new List<int>());


                DFATable.Add(newFinalState);


                // Point prev final states to new state
                for (int i = 0; i < this.Finish.Count; i++)
                {
                    DFATable[this.Finish[i]][this.Alphabet.IndexOf(Translation.Eps)].Add(this.DFATable.IndexOf(newFinalState));
                }

                this.Finish = new List<int>() { this.DFATable.IndexOf(newFinalState) };
            }

        }

        public void Reverse()
        {
            this.MakeOneFinalState();
            var tableOps = new TableOps();
            this.DFATable = tableOps.GetReverseFrom(this.DFATable);
            var tmp = this.Finish[0];
            this.Finish[0] = this.Initial;
            this.Initial = tmp;

        }

        public void Minimize()
        {
            this.Reverse();
            this.MakeDfa();
            this.Reverse();
            this.MakeDfa();
        }

        public bool Model(string word, List<char> alphabet)
        {
            if (word.Length == 0 && Finish.Contains(Initial))
                return true;
            else if (word.Length == 0 && !Finish.Contains(Initial))
                return false;
            else
            {
                CharEnumerator c = word.GetEnumerator();
                c.MoveNext();
                int letterI = alphabet.IndexOf(c.Current);
                int nextState = DFATable[Initial][letterI][0];

                while (c.MoveNext())
                {
                    letterI = alphabet.IndexOf(c.Current);
                    nextState = DFATable[nextState][letterI][0];
                }

                if (Finish.Contains(nextState))
                    return true;
                else
                    return false;
            }
        }

        private HashSet<int> EpsClosure(HashSet<int> states, List<List<List<int>>> nfaTable, int epsI)
        {
            Stack<int> stack = new Stack<int>(states);
            HashSet<int> epsClosure = new HashSet<int>(states);

            while (stack.Count > 0)
            {
                int state = stack.Pop();
                for (int i = 0; i < nfaTable[state][epsI].Count; i++)
                {
                    int nextState = nfaTable[state][epsI][i];
                    if (!epsClosure.Contains(nextState))
                    {
                        epsClosure.Add(nextState);
                        stack.Push(nextState);
                    }
                }
            }

            return epsClosure;
        }

        private HashSet<int> Move(HashSet<int> states, int letterI, List<List<List<int>>> nfaTable)
        {
            HashSet<int> move = new HashSet<int>();


            foreach (int state in states)
            {
                for (int i = 0; i < nfaTable[state][letterI].Count; i++)
                {
                    int nextState = nfaTable[state][letterI][i];
                    move.Add(nextState);
                }
            }

            return move;
        }
        private HashSet<int> ReverseMove(HashSet<int> states, int letterI)
        {
            HashSet<int> reverseMove = new HashSet<int>();

            foreach (int state in states)
            {
                for (int i = 0; i < DFATable.Count; i++)
                {
                    int prevState = DFATable[i][letterI][0];
                    if (prevState == state)
                        reverseMove.Add(i);
                }
            }

            return reverseMove;
        }
    }
}
