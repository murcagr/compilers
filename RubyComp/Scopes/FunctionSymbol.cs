using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public class FunctionSymbol : Scope, ISymbol
    {
        public string Name { get; }
        public SymbolType Type { get; }
       
       public bool IsGlobal { get; set; }

        public string Parameters { get; set; }

        public FunctionSymbol(string name, SymbolType type, Scope parent) : base(parent)
        {
            Name = name;
            Type = type;
            IsGlobal = true;
        }

        public FunctionSymbol(string name, SymbolType type , string parameters, Scope parent) : base(parent)
        {
            Name = name;
            IsGlobal = true;
            Type = type;
            Parameters = parameters;
        }

        public int Size { get => 0; }
    }
}
