using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class SubSelectBuilder : SelectBuilder
    {
        private QueryBuilderBase _mainSelect;

        public SubSelectBuilder(Syntax syntax, QueryBuilderBase mainSelect)
            : base(syntax)
        {
            _mainSelect = mainSelect;
        }

        private void AddParamsToMainSelect()
        {
            foreach (var p in this._paramList)
            {
                _mainSelect.SqlParameters.Add(p);
            }
        }

        public override string ToString(string alias)
        {
            var query = base.ToString(alias);

            AddParamsToMainSelect();

            //just formatting the query.
            var formattedQuery = query.Replace(Environment.NewLine, Environment.NewLine + "\t");

            return formattedQuery;
        }
    }
}
