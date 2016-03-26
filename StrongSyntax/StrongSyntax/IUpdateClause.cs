using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IUpdateClause
    {
        /// <summary>
        /// Appends a set clause to the query.
        /// </summary>
        /// <param name="setList">Key value pairs of teh columns and the values to update.</param>
        /// <returns></returns>
        ISetClause Set(SetClause setList);
    }
}
