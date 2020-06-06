using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Newtonsoft.Json;
using RubyComp.Scopes;
using System;
using System.Collections.Generic;
using System.IO;

namespace RubyComp
{
    class Program
    {
        public static void Main(string[] args)
        {
            SymbolType.AddTypeRange("Integer", "Float", "String", "Array", "Func", "Unknown", "Bool");


            Run("../../../test.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_func_ok.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_func_bad_wa.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_func_bad_cbd.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_plain_ok.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_plain_ok_string.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_usl_ok.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_usl_ok2.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_mas_ok.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_mas_bad.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_mas_bad2.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_usl_ok_string.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_usl_bad_string.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_loops_ok.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_for_bad1.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_for_bad2.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_for_bad3.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_for_bad4.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_fib.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");
            Run("../../../test_bubble.txt");
            Console.WriteLine("-----------------------------------------------------------------------------\n");

            Console.WriteLine("All ok!");
            Console.Read();
        }


        public static void Run(string filename)
        {
            Console.WriteLine($"File {filename}");

            using (StreamReader file = new StreamReader(filename))
            {
                AntlrInputStream inputStream = new AntlrInputStream(file.ReadToEnd());

                RubLexer rubLexer = new RubLexer(inputStream);

                CommonTokenStream commonTokenStream = new CommonTokenStream(rubLexer);

                RubParser rubParser = new RubParser(commonTokenStream);

                SyntaxErrorListener syntaxErrorListener = new SyntaxErrorListener();
                rubParser.AddErrorListener(syntaxErrorListener);


                var tree = rubParser.prog();
                if (rubParser.NumberOfSyntaxErrors != 0)
                {
                    foreach (var error in syntaxErrorListener.ErrorMessages)
                    {
                        Console.WriteLine($"{filename} | Syntax error:  {error}");
                    }

                    Console.ReadKey();
                    return;
                }


                //Console.WriteLine(json);

                ParseTreeWalker walker = new ParseTreeWalker();
                SemanticListener semantic = new SemanticListener();
                try
                {
                    walker.Walk(semantic, tree);
                }
                catch (SemanticException e)
                {
                    Console.WriteLine($"{filename} | Semantic error:  {e.Message}");

                    Console.ReadKey();
                    return;
                }

                var types = semantic.Types;

                Dictionary<string, object> map = new Dictionary<string, object>();
                traverse(tree, types, map);
                string json = JsonConvert.SerializeObject(map, Formatting.Indented);

                using (StreamWriter outFile = File.CreateText(filename + "AST_out.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    outFile.Write(json);
                }

            }
        }

        public static void traverse(IParseTree tree, ParseTreeProperty<SymbolType> types, Dictionary<string, object> map)
        {

            var type = types.Get(tree);
            if (type != null)
            {
                map.Add("type", type.Name);
            }

            if (tree is TerminalNodeImpl)
            {
                IToken token = ((TerminalNodeImpl)tree).Symbol;
                map.Add("tokenType", RubLexer.ruleNames[token.Type]);
                map.Add("text", token.Text);

            }
            else
            {
                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                string name = tree.GetType().Name.Replace("Context", "");
                map.Add(name, children);

                for (int i = 0; i < tree.ChildCount; i++)
                {
                    Dictionary<string, object> nested = new Dictionary<string, object>();
                    children.Add(nested);
                    traverse(tree.GetChild(i), types, nested);
                }
            }
        }

    }


    public class SemanticException : Exception
    {
        public SemanticException() : base()
        { }

        public SemanticException(string msg) : base(msg)
        { }

        public SemanticException(string msg, Exception e) : base(msg, e)
        { }
    }

}