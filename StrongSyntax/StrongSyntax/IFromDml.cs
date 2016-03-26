using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IFromDml : ICompleteDml
    {
        ICompleteDml Where(string filter, params object[] values);
    }
}
