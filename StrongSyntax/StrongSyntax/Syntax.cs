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
            return new DbQueryBuilder(this);
        }

        public IInsertQuery GetInsert()
        {
            return new InsertBuilder(this);
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
