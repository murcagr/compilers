using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace Lab3_Descending
{
    class RecProc
    {

        private static int cur;
        private static string curString;

        public static bool Parse(string input)
        {
            cur = 0;
            curString = input;

            bool result = true;
            if (!EXP(curString))
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
            else if (curString.StartsWith("<"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith("<="))
            {
                cur += 2;
                curString = curString.Substring(2, curString.Length - 2);
            }
            else if (curString.StartsWith(">"))
            {
                cur += 1;
                curString = curString.Substring(1, curString.Length - 1);
            }
            else if (curString.StartsWith(">="))
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
                var tmpcurStriong = curString;

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
    }
}
