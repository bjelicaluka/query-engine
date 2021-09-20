using System.Collections.Generic;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineCore.Contracts
{
    public interface IParser
    {
        Query Parse(IEnumerable<Token> tokens);
    }
}