using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_Regexp.Helpers
{
    class TableOps
    {
        
        public void PrintTransitions(List<List<List<int>>> table)
        {
            Console.WriteLine("Print table");
            for (int x = 0; x < table.Count; x++)
            {
                for (int y = 0; y < table[x].Count; y++)
                {
                    for (int z = 0; z < table[x][y].Count; z++)
                    {
                        Console.WriteLine("From {0} by {1} to {2}", x, y, table[x][y][z]);
                    }
                }
            }
        }

        public List<List<List<int>>> GetReverseFrom(List<List<List<int>>> original)
        {
            List<List<List<int>>> reverseTable = new List<List<List<int>>>(original.Count);
            for (int i = 0; i < original.Count; i++)
            {
                reverseTable.Add(new List<List<int>>(original[i].Count));
                for (int j = 0; j < original[i].Count; j++)
                    reverseTable[i].Add(new List<int>());
            }

            for (int x = 0; x < original.Count; x++)
            {
                for (int y = 0; y < original[x].Count; y++)
                {
                    for (int z = 0; z < original[x][y].Count; z++)
                    {
                        reverseTable[original[x][y][z]][y].Add(x);
                    }
                }
            }
            return reverseTable;
        }


        public List<List<List<int>>> GetDfaFromNfa(List<List<List<int>>> original)
        {
            List<List<List<int>>> dfaTable= new List<List<List<int>>>(original.Count);

            List<List<int>> states = new List<List<int>>();

            states.Add(new List<int> { 0 });
            dfaTable.Add(original[0]);

            for (int x = 0; x < dfaTable[0].Count; x++)
            {
                var tmp = new List<int>();

                for (int y = 0; y < dfaTable[0][x].Count; y++)
                {
                    tmp.Add(dfaTable[0][x][y]);
                }

                if (!states.Contains(tmp)) {
                    states.Add(tmp);
                }

            }

            return dfaTable;
        }

    }
}
