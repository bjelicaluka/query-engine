using System.Collections.Generic;
using QueryEngineModel.Tokens;

namespace QueryEngineParser.Rules
{
    public interface IMatchable
    {
        Match Match(IEnumerable<Token> tokens);
    }
}