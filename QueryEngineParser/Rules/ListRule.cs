using System.Collections.Generic;
using System.Linq;
using QueryEngineModel.Tokens;
using QueryEngineParser.Utils;

namespace QueryEngineParser.Rules
{
    public class ListRule : IMatchable
    {

        public Match Match(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var idMatch = MatchId(tokenList);
            if(idMatch.Index == -1)
                return new Match { Index = -1 };
            tokenList = ListUtils.EatTokens(idMatch, tokenList);
            
            if (tokenList.Any() && tokenList[0].Type == TokenType.Comma)
            {
                var value = (IList<string>) Match(ListUtils.EatTokens(new Match {Index = 0 }, tokenList))
                    .Value;
                return new Match
                {
                    Index = idMatch.Index + 1,
                    Value = value.Union((IList<string>) idMatch.Value).ToList()
                };
            }

            return idMatch;
        }

        private static Match MatchId(IEnumerable<Token> tokens)
        {
            var idMatch = new Rule(TokenType.Id).Match(tokens);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            return new Match
            {
                Index = idMatch.Index,
                Value = new List<string>
                {
                    ((Token) idMatch.Value).Value
                }
            };
        }
    }
}