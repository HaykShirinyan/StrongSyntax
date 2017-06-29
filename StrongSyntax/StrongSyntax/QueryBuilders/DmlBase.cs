using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class DmlBase : SelectBuilder
    {
        public bool ParametrizeQuery { get; set; }
        public DbTransaction CurrentTransaction { get; set; }

        public DmlBase(Syntax syntax) 
            : base(syntax)
        {
        }

        public int Execute()
        {
            var helper = new DmlQueryHelper(this);

            int rowsAffected = helper.Execute(this.CurrentTransaction);

            return rowsAffected;
        }

        public async Task<int> ExecuteAsync()
        {
            var helper = new DmlQueryHelper(this);

            int rowsAffected = await helper.ExecuteAsync(this.CurrentTransaction);

            return rowsAffected;
        }
    }
}
