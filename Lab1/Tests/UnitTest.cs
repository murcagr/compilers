using Lab1_Regexp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class UnitTest
    {
        static string rg1 = "a";
        static string rg2 = "ab";
        static string rg3 = "(a|b)";
        static string rg4 = "a*";
        static string rg5 = "(ab|b*)*";
        static string rg6 = "(ba|b*aa)+";

        [Fact]
        public void TestWork()
        {
            List<char> alphabet = Enumerable.Range('a', 'b' - 'a' + 1).Select(i => (Char) i).ToList();
            List<char> epsAlphabet = alphabet.Concat(new char[] { 'ε' }.ToList()).ToList();

            string processedRegexp1 = Program.Preprocess(rg1);
            string postfix1 = Program.ToPostfix(processedRegexp1);
            NFA nfa1 = Program.ToNFA(postfix1);
            var nfaTable1 = nfa1.BuildTable(epsAlphabet);
            DFA dfa1 = new DFA(nfaTable1, alphabet, epsAlphabet.IndexOf('ε'), nfa1.Initial.S, nfa1.Finish.S);
            dfa1.Minimize();

            Assert.True(dfa1.Model("a", alphabet));
            Assert.False(dfa1.Model("aa", alphabet));
            Assert.False(dfa1.Model("b", alphabet));

            string processedRegexp2 = Program.Preprocess(rg2);
            string postfix2 = Program.ToPostfix(processedRegexp2);
            NFA nfa2 = Program.ToNFA(postfix2);
            var nfaTable2 = nfa2.BuildTable(epsAlphabet);
            DFA dfa2 = new DFA(nfaTable2, alphabet, epsAlphabet.IndexOf('ε'), nfa2.Initial.S, nfa2.Finish.S);
            dfa2.Minimize();
            Assert.True(dfa2.Model("ab", alphabet));
            Assert.False(dfa2.Model("aba", alphabet));
            Assert.False(dfa2.Model("abb", alphabet));
            Assert.False(dfa2.Model("ba", alphabet));

            string processedRegexp3 = Program.Preprocess(rg3);
            string postfix3 = Program.ToPostfix(processedRegexp3);
            NFA nfa3 = Program.ToNFA(postfix3);
            var nfaTable3 = nfa3.BuildTable(epsAlphabet);
            DFA dfa3 = new DFA(nfaTable3, alphabet, epsAlphabet.IndexOf('ε'), nfa3.Initial.S, nfa3.Finish.S);
            dfa3.Minimize();
            Assert.True(dfa3.Model("a", alphabet));
            Assert.True(dfa3.Model("b", alphabet));
            Assert.False(dfa3.Model("ab", alphabet));
            Assert.False(dfa3.Model("ba", alphabet));

            string processedRegexp4 = Program.Preprocess(rg4);
            string postfix4 = Program.ToPostfix(processedRegexp4);
            NFA nfa4 = Program.ToNFA(postfix4);
            var nfaTable4 = nfa4.BuildTable(epsAlphabet);
            DFA dfa4 = new DFA(nfaTable4, alphabet, epsAlphabet.IndexOf('ε'), nfa4.Initial.S, nfa4.Finish.S);
            dfa4.Minimize();
            Assert.True(dfa4.Model("a", alphabet));
            Assert.True(dfa4.Model("aa", alphabet));
            Assert.True(dfa4.Model("aaaaaaaa", alphabet));
            Assert.True(dfa4.Model("", alphabet));
            Assert.False(dfa4.Model("b", alphabet));

            string processedRegexp5 = Program.Preprocess(rg5);
            string postfix5 = Program.ToPostfix(processedRegexp5);
            NFA nfa5 = Program.ToNFA(postfix5);
            var nfaTable5 = nfa5.BuildTable(epsAlphabet);
            DFA dfa5 = new DFA(nfaTable5, alphabet, epsAlphabet.IndexOf('ε'), nfa5.Initial.S, nfa5.Finish.S);
            dfa5.Minimize();
            Assert.True(dfa5.Model("ab", alphabet));
            Assert.True(dfa5.Model("abb", alphabet));
            Assert.True(dfa5.Model("", alphabet));
            Assert.False(dfa5.Model("aba", alphabet));

            string processedRegexp6 = Program.Preprocess(rg6);
            string postfix6 = Program.ToPostfix(processedRegexp6);
            NFA nfa6 = Program.ToNFA(postfix6);
            var nfaTable6 = nfa6.BuildTable(epsAlphabet);
            DFA dfa6 = new DFA(nfaTable6, alphabet, epsAlphabet.IndexOf('ε'), nfa6.Initial.S, nfa6.Finish.S);
            dfa6.Minimize();
            Assert.True(dfa6.Model("baaa", alphabet));
            Assert.False(dfa6.Model("", alphabet));
            Assert.True(dfa6.Model("babbaa", alphabet));
            Assert.False(dfa6.Model("bb", alphabet));
        }
    }
}
