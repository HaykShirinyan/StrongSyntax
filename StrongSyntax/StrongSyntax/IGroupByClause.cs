using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IGroupByClause : ICompleteQuery
    {
        IOrderByClause OrderBy(params string[] orderList);
        IOrderByClause OrderByDescending(params string[] orderList);
    }
}
