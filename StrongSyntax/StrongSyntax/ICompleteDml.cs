using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ICompleteDml : IQuerySyntax
    {
        int Execute();
        Task<int> ExecuteAsync();
    }
}
