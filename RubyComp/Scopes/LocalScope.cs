using System;
using System.Collections.Generic;
using System.Text;

namespace RubyComp.Scopes
{
    public class LocalScope : Scope
    {
        public LocalScope(Scope parent) : base(parent)
        {

        }
    }
}
