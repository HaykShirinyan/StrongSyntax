using System;
using System.Data;

namespace StrongSyntax
{
    public interface IDynamicQuery : IQuerySyntax
    {
        ISelectClause Select(params string[] selector);
    }
}
