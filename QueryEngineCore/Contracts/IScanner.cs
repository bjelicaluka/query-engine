using System.Collections.Generic;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineCore.Contracts
{
    public interface IScanner
    {
        IEnumerable<Token> Scan(string query);
    }
}