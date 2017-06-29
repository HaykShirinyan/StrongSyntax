using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class UpdateBuilder : DmlBase, IUpdateQuery, IUpdateClause, ISetClause, IUpdateFromClause
    {
        private SetClause _setClause;

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

                var sqlParams = this.AddParam(new SqlParameter("@Set" + counter, kvp.Value));

                setClause.SqlParameters.Add(sqlParams);

                counter++;
            }
        }

        private void ValidateSetClause(SetClause setClause)
        {
            this.CheckNullException(setClause, "setClause");

            if (setClause.Count == 0)
            {
                throw new ArgumentException("Set clause of the update query wasn't provided.");
            }
        }

        public ISetClause Set(SetClause setClause)
        {
            ValidateSetClause(setClause);

            string[] values = new string[setClause.Count];

            _query.AppendLine("SET");

            AddSetClause(values, setClause);

            _query.AppendLine(this.CreateList(values).ToString());

            _setClause = setClause;

            return this;
        }

        private void RemoveTableParam(string tableName)
        {
            foreach(var sqlParam in _setClause.SqlParameters)
            {
                if (sqlParam.Value.ToString().IndexOf(tableName + ".", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    this._query.Replace(sqlParam.ParameterName, sqlParam.Value.ToString());

                    this._paramList.Remove(sqlParam);
                }
            }
        }

        private void ValidateFromClause(ITempTable tempTable, string alias)
        {
            this.CheckNullException(tempTable, "tempTable");
            this.CheckNullException(alias, "alias");
        }

        private void AddTempTableQueryToMain(ITempTable tempTable)
        {
            this._query.Insert(0, tempTable.ToString());

            foreach (var param in tempTable.SqlParameters)
            {
                this.AddParam(param);
            }
        }

        public new IUpdateFromClause From(string tableName)
        {
            var from = (UpdateBuilder)base.From(tableName);

            RemoveTableParam(tableName);

            return from;
        }

        public new IUpdateFromClause From(ICompleteQuery subSelect, string alias)
        {
            var from = (UpdateBuilder)base.From(subSelect, alias);

            RemoveTableParam(alias);

            return from;
        }

        public IUpdateFromClause From(ITempTable tempTable, string alias)
        {
            ValidateFromClause(tempTable, alias);

            AddTempTableQueryToMain(tempTable);

            this._query
                .AppendFormat("FROM {0} AS [{1}]", tempTable.TableName, alias)
                .AppendLine();

            RemoveTableParam(alias);

            return this;
        }

        public new IUpdateFromClause LeftJoin(string tableName, string condition)
        {
            var joined = (UpdateBuilder)base.LeftJoin(tableName, condition);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause LeftJoin(string tableName, string condition, params object[] values)
        {
            var joined = (UpdateBuilder)base.LeftJoin(tableName, condition, values);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            var joined = (UpdateBuilder)base.LeftJoin(subSelect, condition, alias);

            RemoveTableParam(alias);

            return joined;
        }

        public new IUpdateFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            var joined = (UpdateBuilder)base.LeftJoin(subSelect, condition, alias, values);

            RemoveTableParam(alias);

            return joined;
        }

        public new IUpdateFromClause RightJoin(string tableName, string condition)
        {
            var joined = (UpdateBuilder)base.RightJoin(tableName, condition);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause RightJoin(string tableName, string condition, params object[] values)
        {
            var joined = (UpdateBuilder)base.RightJoin(tableName, condition, values);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            var joined = (UpdateBuilder)base.RightJoin(subSelect, condition, alias);

            RemoveTableParam(alias);

            return joined;
        }

        public new IUpdateFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            var joined = (UpdateBuilder)base.RightJoin(subSelect, condition, alias, values);

            RemoveTableParam(alias);

            return joined;
        }

        public new IUpdateFromClause InnerJoin(string tableName, string condition)
        {
            var joined = (UpdateBuilder)base.InnerJoin(tableName, condition);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause InnerJoin(string tableName, string condition, params object[] values)
        {
            var joined = (UpdateBuilder)base.InnerJoin(tableName, condition, values);

            RemoveTableParam(tableName);

            return joined;
        }

        public new IUpdateFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            var joined = (UpdateBuilder)base.InnerJoin(subSelect, condition, alias);

            RemoveTableParam(alias);

            return joined;
        }

        public new IUpdateFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            var joined = (UpdateBuilder)base.InnerJoin(subSelect, condition, alias, values);

            RemoveTableParam(alias);

            return joined;
        }

        public new ICompleteDml Where(string filter, params object[] values)
        {
            return (UpdateBuilder)base.Where(filter, values);
        }
    }
}
