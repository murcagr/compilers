using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Lab1_Regexp.Helpers
{
    public class GraphVizBuilder
    {
        private GraphGeneration wrapper;

        public GraphVizBuilder()
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
        }

        public string BuildString(List<char> alphabet, List<List<List<int>>> FA, int init, List<int> finish)
        {
            StringBuilder graph = new StringBuilder($"digraph finite_state_machine {{ node[shape = doublecircle]; ");
            foreach (var f in finish)
            {
                string output = f == init ? $"S{f}" : $"{f}";

                graph.Append($"{output} ");
            }
            graph.Append($"; node[shape = circle]; ");
            for (int i = 0; i < FA.Count; i++)
                for (int j = 0; j < FA[i].Count; j++)
                {
                    if (FA[i][j].Count > 0)
                    {
                        foreach (int state in FA[i][j])
                        {
                            string output = alphabet[j] == Translation.Eps ? "eps" : alphabet[j].ToString();
                            string from = i == init ? $"S{i}" : $"{i}";
                            string to = state == init ? $"S{state}" : $"{state}";

                            graph.Append($"{from} -> {to} [ label = \" { output }\" ]; ");
                        }
                    }
                }
            graph.Append("}");

            return graph.ToString();
        }

        public void CreateGraph(string graph, string filename)
        {
            byte[] output = wrapper.GenerateGraph(graph, Enums.GraphReturnType.Png);

            using (Image image = Image.FromStream(new MemoryStream(output)))
            {
                image.Save(filename, ImageFormat.Png);
            }
        }
    }
}
