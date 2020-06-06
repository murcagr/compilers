using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public interface ISymbol
    {
        string Name { get; }
        SymbolType Type { get; }
        
        public string Parameters { get; set; }
      
        bool IsGlobal { get; set; }

    }
}
