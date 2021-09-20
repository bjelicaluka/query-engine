using System.Collections.Generic;
using System.Linq;
using QueryEngineModel.Tokens;
using QueryEngineParser.Rules;

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