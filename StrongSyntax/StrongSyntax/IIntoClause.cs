using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IIntoClause : ICompleteDml
    {
        IIntoClause Values(params object[] values);
    }
}
