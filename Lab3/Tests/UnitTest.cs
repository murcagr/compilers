using Lab3_Descending;
using Lab3_Descending.Elements;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {

            Assert.True(RecAddProc.Parse("begina=aend"));
            Assert.True(RecAddProc.Parse("begina=a;a=c<aend"));
            Assert.True(RecAddProc.Parse("begina=c;a=(a*a)+a<a*aend"));
            Assert.True(RecAddProc.Parse("begina=a<>(c+a*a)end"));
            Assert.False(RecAddProc.Parse("beginend"));
            Assert.False(RecAddProc.Parse("beginaend"));
            Assert.False(RecAddProc.Parse("begina;end"));
            Assert.False(RecAddProc.Parse("begina=c;end"));
            Assert.False(RecAddProc.Parse("begina==cend"));
            Assert.False(RecAddProc.Parse("begina=c(end"));
            Assert.False(RecAddProc.Parse("begina=a"));
            Assert.False(RecAddProc.Parse("begina=a/end"));

        }
    }
}
