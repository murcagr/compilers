using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public class GlobalScope : Scope
    {
        public GlobalScope() : base(null)
        {
            ScopeName = "global";
        }
    }
}
