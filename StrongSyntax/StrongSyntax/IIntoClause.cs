using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IIntoClause : ICompleteDml
    {
        /// <summary>
        /// Appends the values to insert into the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        IIntoClause Values(params object[] values);
    }
}
