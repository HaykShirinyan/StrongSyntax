using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class DmlBase : DbQueryBuilder
    {
        public DmlBase(Syntax syntax) 
            : base(syntax)
        {
        }

        public int Execute()
        {
            var helper = new DmlQueryHelper(this);

            int rowsAffected = helper.Execute();

            return rowsAffected;
        }

        public async Task<int> ExecuteAsync()
        {
            var helper = new DmlQueryHelper(this);

            int rowsAffected = await helper.ExecuteAsync();

            return rowsAffected;
        }
    }
}
