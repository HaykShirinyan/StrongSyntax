using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class DeleteBuilder : DeleteUpdateBase, IDeleteQuery, IDeleteClause, IFromDml
    {
        public DeleteBuilder(Syntax syntax) 
            : base(syntax)
        {
        }

        public IDeleteClause Delete()
        {
            _query.Append("DELETE ");

            return this;
        }

        public new IFromDml From(string tableName)
        {
            return (IFromDml)base.From(tableName);
        }
    }
}
