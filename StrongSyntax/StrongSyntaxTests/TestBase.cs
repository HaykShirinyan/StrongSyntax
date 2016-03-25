using StrongSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests
{
    public class TestBase
    {
        public static Syntax Syntax { get; set; }

        public TestBase()
        {
            Syntax = new Syntax(@"");
        }
    }
}
