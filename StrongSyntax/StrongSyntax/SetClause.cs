using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    [Serializable]
    public class SetClause : Dictionary<string, object>
    {
        public ICollection<SqlParameter> SqlParameters { get; set; }

        public SetClause()
        {
            this.SqlParameters = new List<SqlParameter>();
        }
    }
}
