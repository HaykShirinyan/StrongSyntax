using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class InsertBuilder : DmlBase, IInsertQuery, IInsertClause, IIntoClause
    {
        private bool _isValueAdded;

        public InsertBuilder(Syntax syntax) 
            : base(syntax)
        {
        }

        public IInsertClause Insert(params string[] columns)
        {
            _query.AppendLine("INSERT INTO")
               .AppendLine("(");

            var insertCols = this.CreateList(columns);

            _query.AppendLine(insertCols.ToString())
                .AppendLine(")");

            return this;
        }

        public IIntoClause Into(string tableName)
        {
            this.CheckNullException(tableName, "tableName");

            _query.Replace("INSERT INTO", "INSERT INTO " + tableName);

            return this;
        }

        private IEnumerable<string> Parametrize(object[] values)
        {
            List<string> paramsetrizedList = new List<string>();

            foreach (var val in values)
            {
                string paramName = string.Format("@{0}", _paramList.Count);

                paramsetrizedList.Add(paramName);

                this._paramList.Add(new SqlParameter(paramName, val));
            }

            return paramsetrizedList;
        }

        private void AppendValues(StringBuilder values)
        {
            if (_isValueAdded)
            {
                _query.Append(",");
            }
            else
            {
                _query.AppendLine("VALUES");
            }

            _query.AppendLine("(")
                .AppendLine(values.ToString())
                .AppendLine(")");

            _isValueAdded = true;
        }

        public IIntoClause Values(params object[] values)
        {
            var parametrized = Parametrize(values);

            var valueList = this.CreateList(parametrized.ToArray());

            AppendValues(valueList);

            return this;
        }
    }
}
