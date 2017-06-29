using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrongSyntax.QueryBuilders;
using System.Data.Common;

namespace StrongSyntax
{
    public class Syntax
    {
        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public int Timout { get; set; }
        public DbTransaction CurrentTransaction { get; set; }

        public Syntax(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ITempTable GetTempTable<TTable>(IEnumerable<TTable> recordList)
            where TTable : class, new()
        {
            return GetTempTable(recordList, true);
        }

        public ITempTable GetTempTable<TTable>(IEnumerable<TTable> recordList, bool parametrizeQuery)
            where TTable : class, new()
        {
            var tempTable = new TempTable<TTable>();

            tempTable.ParametrizeQuery = parametrizeQuery;
            tempTable.FillTable(recordList);

            return tempTable;
        }

        public IDynamicQuery GetQuery()
        {
            return new SelectBuilder(this);
        }

        public IInsertQuery GetInsert()
        {
            return GetInsert(true);
        }

        public IInsertQuery GetInsert(bool parametrizeQuery)
        {
            var builder = new InsertBuilder(this);

            builder.ParametrizeQuery = parametrizeQuery;

            return builder;
        }

        public IUpdateQuery GetUpdate()
        {
            return new UpdateBuilder(this);
        }

        public IDeleteQuery GetDelete()
        {
            return new DeleteBuilder(this);
        }

        public IStrongQuery<TEntity> GetStrongQuery<TEntity>()
            where TEntity : class
        {
            return new StrongQueryBuilder<TEntity>(this);
        }
    }
}
