using Lab4_Precedence;
using System;
using Xunit;

namespace Tests
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {
            PrecedenceParser parser = new PrecedenceParser();
            string expr, postfix;

            expr = "( a + c ) * a <> a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("ac+a*a<>", postfix);

            expr = "( a + c ) * a <> a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("ac+a*a<>", postfix);

            expr = "a - a + c";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("aa-c+", postfix);

            expr = "a - c / a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("ca/a-", postfix);

            expr = "a - a div c";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("acdiva-", postfix);

            expr = "a - c mod a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("camoda-", postfix);

            expr = "+ a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("as+", postfix);

            expr = "- a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("as-", postfix);

            expr = "- a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("as-", postfix);

            expr = "- a + a + a + a";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("as-a+a+a+", postfix);

            expr = "- ( a + c / a - a )";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("ca/a+a-s-", postfix);

            expr = "( - a + a )";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Equal("as-a+", postfix);

            expr = "( a + c";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Null(postfix);

            expr = "( a + c ) )";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Null(postfix);

            expr = "( a + c * )";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Null(postfix);

            expr = "a + c )";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Null(postfix);

            expr = "a + c >=";
            postfix = parser.ParseToPostfix(expr.Split(' '));
            Assert.Null(postfix);
        }
    }
}
