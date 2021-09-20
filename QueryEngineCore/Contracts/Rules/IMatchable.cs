using System.Collections.Generic;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineCore.Contracts.Rules
{
    public interface IMatchable
    {
        Match Match(IList<Token> tokens, int index = 0);
    }
}