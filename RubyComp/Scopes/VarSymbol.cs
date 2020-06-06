using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public class VarSymbol : ISymbol
    {
        public string Name { get; }
        public SymbolType Type { get; } 

        public string Parameters { get; set; }
        public bool IsGlobal { get; set; }

        public VarSymbol(string name, SymbolType type, int arraySize = -1)
        {
            Name = name;
            Type = type;
            IsGlobal = true;
        }

    }
}
