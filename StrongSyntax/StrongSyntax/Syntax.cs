using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrongSyntax.QueryBuilders;

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

        public Syntax(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDynamicQuery GetQuery()
        {
            return new DbQueryBuilder(_connectionString);
        }

        public IInsertQuery GetInsert()
        {
            return new InsertBuilder(_connectionString);
        }

        public IUpdateQuery GetUpdate()
        {
            return new UpdateBuilder(_connectionString);
        }

        public IStrongQuery<TEntity> GetStrongQuery<TEntity>()
            where TEntity : class
        {
            return new StrongQueryBuilder<TEntity>(_connectionString);
        }
    }
}
