using System.Collections.Generic;
using System.Linq;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser.Utils
{
    public static class ListUtils
    {
        public static List<Token> EatTokens(Match match, IList<Token> tokens)
        {
            return tokens.ToList().GetRange(match.Index + 1, tokens.Count - match.Index - 1);
        }
    }
}