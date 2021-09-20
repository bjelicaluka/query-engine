using System.Collections.Generic;

namespace QueryEngineCore.Contracts
{
    public interface IQueryEngine
    {
        IEnumerable<IAccessible> Run(string query);
    }
}