using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class StrongQueryBuilder<TEntity> : DbQueryBuilder, IStrongQuery<TEntity>
        where TEntity : class
    {
        private string _tableName;

        private string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = GetTableName(typeof(TEntity));
                }

                return _tableName;
            }
        }

        public StrongQueryBuilder(string connectionString)
            : base(connectionString)
        {
        }

        private string GetTableName(Type t)
        {
            return t.Name + "s";
        }

        private string ParsePropertyName(Expression expr)
        {
            string name = string.Empty;
            var memberExpr = expr as MemberExpression;

            if (memberExpr == null)
            {
                var urExpr = expr as UnaryExpression;

                if (urExpr != null && urExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = urExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null)
            {
                name = memberExpr.Expression.Type.Name + "s." + memberExpr.Member.Name;
            }

            return name;
        }

        private string ParseColumnName(Expression expr)
        {
            var type = expr.Type;

            return GetTableName(type);
        }

        private string ParseOperator(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.NotEqual:
                    return " <> ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " OR ";
            }

            return string.Empty;
        }

        private string ParsePredicate(Expression expr)
        {
            string predicate = string.Empty;
            var lambda = expr as LambdaExpression;

            if (lambda != null)
            {
                var binExpr = lambda.Body as BinaryExpression;

                string left = ParseColumnName(lambda.Parameters[0]);
                string right = ParseColumnName(lambda.Parameters[1]);
                string @operator = ParseOperator(binExpr);

                predicate = string.Format("{0}{1}{2}", left, @operator, right);
            }

            return predicate;
        }

        private IEnumerable<string> ParseColumnNames(Expression<Func<TEntity, object[]>> selector)
        {
            List<string> columnList = new List<string>();
            var arrayExpr = selector.Body as NewArrayExpression;

            if (arrayExpr != null)
            {
                foreach (var expr in arrayExpr.Expressions)
                {
                    string name = ParsePropertyName(expr);

                    columnList.Add(name);
                }
            }

            return columnList;
        }

        public IStrongQuery<TEntity> Select(Expression<Func<TEntity, object[]>> selector)
        {
            var columns = ParseColumnNames(selector);

            return (StrongQueryBuilder<TEntity>)Select(columns.ToArray());
        }

        public IStrongQuery<TEntity> From()
        {
            return (StrongQueryBuilder<TEntity>)this.From(this.TableName);
        }

        public IStrongQuery<TEntity> LeftJoin<TJoin>(Expression<Func<TEntity, TJoin, bool>> condition)
        {
            string predicate = ParsePredicate(condition);

            return (StrongQueryBuilder<TEntity>)this.LeftJoin(this.TableName, predicate);
        }

        public IStrongQuery<TEntity> GroupBy(Expression<Func<TEntity, object[]>> groupings)
        {
            var columns = ParseColumnNames(groupings);

            return (StrongQueryBuilder<TEntity>)this.GroupBy(groupings);
        }
    }
}
