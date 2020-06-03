using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace Lab3_Descending
{
    public class RecAddProc
    {

        private static int cur;
        private static string curString;

        public static bool Parse(string input)
        {
            cur = 0;
            curString = input;

            bool result = true;
            if (!PROG(curString))
            {
                result = false;
            }
            if (cur != input.Length)
            {
                result = false;
            }
            return result;
        }


        public static bool MOP(string input)
        {

            if (curString.StartsWith("*"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("/"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("div"))
            {
                cur += 3;
                curString = curString.Substring(3, curString.Length - 3);
            }
            else if (curString.StartsWith("mod"))
            {
                cur += 3;
                curString = curString.Substring(3, curString.Length - 3);
            }
            else if (curString.StartsWith("and"))
            {
                cur += 3;
                curString = curString.Substring(3, curString.Length - 3);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool SIGN(string input)
        {

            if (curString.StartsWith("+"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("-"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool AOP(string input)
        {

            if (curString.StartsWith("+"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("-"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("or"))
            {
                cur += 2;
                curString = curString.Substring(2, curString.Length - 2);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool ROP(string input)
        {

            if (curString.StartsWith("="))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("<>"))
            {
                cur += 2;
                curString = curString.Substring(2, curString.Length - 2);
            }

            else if (curString.StartsWith("<="))
            {
                cur += 2;
                curString = curString.Substring(2, curString.Length - 2);
            }

            else if (curString.StartsWith(">="))
            {
                cur += 2;
                curString = curString.Substring(2, curString.Length - 2);
            }
            else if (curString.StartsWith("<"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith(">"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool F(string input)
        {

            if (curString.StartsWith("a"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
                return true;
            }
            else if (curString.StartsWith("c"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
                return true;
            }
            else if (curString.StartsWith("("))
            {
                var tmpcur = cur;
                var tmpcurString = curString;

                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);

                if (SEXP(input))
                {
                    if (curString.StartsWith(")"))
                    {
                        cur += 1;
                        curString = curString.Substring(1, curString.Length - 1);
                        return true;
                    }
                    return false;
                }
                else
                    return false;

            }
            else if (curString.StartsWith("not"))
            {
                cur += 3;
                curString = curString.Substring(3, curString.Length - 3);

                if (F(input))
                {
                    return true;
                }
                else
                    return false;

            }
            else
            {
                return false;
            }
        }


        public static bool EXP(string input)
        {
            bool res = false;

            var tmpcur = cur;
            var tmpcurString = curString;

            if (SEXP(input))
            {
                if (ROP(input))
                {
                    if (SEXP(input))
                    {
                        res = true;
                    }
                }
            }

            if (res != true)
            {
                cur = tmpcur;
                curString = tmpcurString;

                if (SEXP(input))
                {
                    res = true;
                }
            }

            if (res != true)
            {
                cur = tmpcur;
                curString = tmpcurString;
            }

            return res;
        }

        private static bool SEXP(string input)
        {
            var res = false;
            var tmpcur = cur;
            var tmpcurString = curString;

            if (T(input))
            {
                if (SEXP1(input))
                {
                    res = true;
                }
            }

            if (res != true)
            {
                cur = tmpcur;
                curString = tmpcurString;
                if (SIGN(input))
                {
                    if (T(input))
                    {
                        if (SEXP1(input))
                        {
                            res = true;
                        }
                    }
                }
            }

            if (res != true){
                cur = tmpcur;
                curString = tmpcurString;
            }

            return res;
        }

        private static bool SEXP1(string input)
        {
            var res = false;
            var tmpcur = cur;
            var tmpcurString = curString;

            if (AOP(input))
            {
                if (T(input))
                {
                    if (SEXP1(input))
                    {
                        res = true;
                    }
                }
            }
            // EPS 
            else
            {
                res = true;
                cur = tmpcur;
                curString = tmpcurString;
            }

            if (res != true)
            {
                cur = tmpcur;
                curString = tmpcurString;
            }

            return res;

        
        }

        private static bool T(string input)
        {
            var res = false;
            var tmpcur = cur;
            var tmpcurString = curString;

            if (F(input))
            {
                if (T1(input))
                {
                    res = true;
                }
            }

            if (res != true) {
                cur = tmpcur;
                curString = tmpcurString;
            }

            return res;
        }

        private static bool T1(string input)
        {
            var res = false;
            var tmpcur = cur;
            var tmpcurString = curString;

            if (MOP(input))
            {
                if (F(input))
                {
                    if (T1(input))
                    {
                        res = true;
                    }
                }
            }
            // EPS
            else
            {
                res = true;
                cur = tmpcur;
                curString = tmpcurString;
            }

            if (res != true) {
                cur = tmpcur;
                curString = tmpcurString;
            }



            return res;

        }


        public static bool PROG(string input)
        {
            bool result = true;
            if (!BLOCK(input))
            {
                result = false;
            }
            return result;
        }

        public static bool BLOCK(string input)
        {
            bool result = false;

            if (curString.StartsWith("begin"))
            {
                cur += 5;
                curString = curString.Substring(5, curString.Length - 5);
                if (OPLIST(input))
                {
                    if (curString.StartsWith("end"))
                    {
                        cur += 3;
                        curString = curString.Substring(3, curString.Length - 3);
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool OPLIST(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = input;

            if (OPERATOR(input))
            {
                if (!OPLIST1(input))
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static bool OPLIST1(string input)
        {
            bool result = true;
            if (curString.StartsWith(";"))
            {
                cur++;
                curString = curString.Substring(1, curString.Length - 1);
                if (OPERATOR(input))
                {
                    if (!OPLIST1(input))
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool OPERATOR(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = curString;

            if (curString.StartsWith("a"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
                if (curString.StartsWith("="))
                {
                    cur += 1;
                    curString = curString.Substring(1, curString.Length - 1);
                    if (!EXP(input))
                    {
                        result = false;
                    }
                }
                else 
                    result = false;
            }
            else
            {
                result = false;
            }


            return result;
        }

    }
}
