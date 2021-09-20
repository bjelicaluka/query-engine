using System.Collections.Generic;
using QueryEngineCore.Contracts.AST;

namespace QueryEngineCore.Contracts
{
    public interface IInterpreter
    {
        IEnumerable<IAccessible> Execute(Query query);
    }
}