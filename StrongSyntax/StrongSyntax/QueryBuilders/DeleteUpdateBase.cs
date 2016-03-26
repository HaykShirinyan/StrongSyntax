using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    abstract class DeleteUpdateBase : DmlBase, ICompleteDml
    {
        public DeleteUpdateBase(Syntax syntax)
            : base(syntax)
        {
        }

        public new ICompleteDml Where(string filter, params object[] values)
        {
            return (DeleteUpdateBase)base.Where(filter, values);
        }
    }
}
