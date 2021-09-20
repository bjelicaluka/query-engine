using System.Collections.Generic;
using System.Linq;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser.Rules
{
    public class ListRule : IMatchable
    {

        // Id COMMA List
        public Match Match(IList<Token> tokens, int index)
        {
            
            var idMatch = MatchId(tokens, index);
            if(idMatch.Index == -1)
                return new Match { Index = -1 };

            if (idMatch.Index >= tokens.Count - 1 || tokens[idMatch.Index + 1].Type != TokenType.Comma) return idMatch;
            
            var childMatch = Match(tokens, idMatch.Index + 2);
            return new Match
            {
                Index = childMatch.Index,
                Value = ((IList<string>) childMatch.Value).Union((IList<string>) idMatch.Value).ToList()
            };

        }

        // Id
        private static Match MatchId(IList<Token> tokens, int index)
        {
            var idMatch = new Rule(TokenType.Id).Match(tokens, index);
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