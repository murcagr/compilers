using System;

namespace Lab4_Precedence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrecedenceParser parser = new PrecedenceParser();

            //string expr = "( a + c ) * a <> a";
            string expr = "a + a + ( a + a )";
            Console.WriteLine("\nInfix: " + expr);

            string[] tokens = expr.Split(' ');
            

            string postfix = parser.ParseToPostfix(tokens);

            Console.WriteLine();
            if (postfix != null)
                Console.WriteLine("All ok! Postfix: " + postfix);
            else
                Console.WriteLine("Errors during parse!");
        }

        
    }




}
