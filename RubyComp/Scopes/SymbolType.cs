﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public class SymbolType
    {
        private static List<SymbolType> types = new List<SymbolType>();
        private static HashSet<string> setTypes = new HashSet<string>();
        public static GlobalScope GlobalScope;

      

        public bool IsArray { get; set; }
        public bool IsConst { get; set; }

        public string Name { get; }

        public string AddFunc { get; }
      
        

        private SymbolType(string type)
        {
            Name = type;
            IsArray = false;
            IsConst = false;
        }

        private SymbolType(SymbolType type)
        {
            Name = type.Name;
            IsArray = false;
            IsConst = false;
        }

        public bool IsStructType()
        {
            return Name.Contains("struct");
        }
        public static SymbolType GetType(string type)
        {
            SymbolType foundType = types.Find(t => t.Name == type);
            if (foundType != null)
                return new SymbolType(foundType);
            else
                return null;
        }

        public static SymbolType AddType(string type)
        {
            if (!setTypes.Contains(type))
            {
                SymbolType newType = new SymbolType(type);
                types.Add(newType);
                setTypes.Add(type);

                return newType;
            }
            else
                return null;
        }

        public static void AddTypeRange(params string[] types)
        {
            foreach (var type in types)
            {
                if (!setTypes.Contains(type))
                {
                    SymbolType.types.Add(new SymbolType(type));
                    setTypes.Add(type);
                }
            }
        }

        public static bool CheckType(string type)
        {
            return setTypes.Contains(type);
        }

        /// <summary>
        /// For base types only!!!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SymbolType GetBigger(SymbolType a, SymbolType b)
        {
            int indexA = types.FindIndex(t => t.Name == a.Name);
            int indexB = types.FindIndex(t => t.Name == b.Name);

            return indexA > indexB ? a : b;
        }

        public bool IsFullEqual(SymbolType second)
        {
            return Name == second.Name && IsArray == second.IsArray && IsConst == second.IsConst;
        }

        public bool IsEqual(SymbolType second)
        {
            return Name == second.Name && IsArray == second.IsArray;
        }
    }
}
