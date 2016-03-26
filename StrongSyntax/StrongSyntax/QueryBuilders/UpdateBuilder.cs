using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class UpdateBuilder : DmlBase, IUpdateQuery, IUpdateClause, ISetClause
    {
        public UpdateBuilder(Syntax syntax) 
            : base(syntax)
        {
        }

        public IUpdateClause Update(string tableName)
        {
            _query.AppendFormat("UPDATE {0}", tableName)
                .AppendLine(); 
            
            return this;
        }

        private void AddSetClause(string[] values, SetClause setClause)
        {
            int counter = 0;

            foreach (var kvp in setClause)
            {
                values[counter] = string.Format("{0} = @Set{1}", kvp.Key, counter);

                this._paramList.Add(new SqlParameter("@Set" + counter, kvp.Value));

                counter++;
            }
        }

        public ISetClause Set(SetClause setClause)
        {
            string[] values = new string[setClause.Count];

            _query.AppendLine("SET");

            AddSetClause(values, setClause);

            _query.AppendLine(this.CreateList(values).ToString());

            return this;
        }

        public new ICompleteDml Where(string filter, params object[] values)
        {
            return (UpdateBuilder)base.Where(filter, values);
        }
    }
}
