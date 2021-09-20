using System.Collections.Generic;
using System.Linq;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser.Rules
{
    public class Rule : IMatchable
    {
        private readonly TokenType _tokenType;
        
        public Rule(TokenType tokenType)
        {
            _tokenType = tokenType;
        }

        // TOKEN
        public virtual Match Match(IList<Token> tokens, int index)
        {
            if(index >= tokens.Count || tokens[index].Type != _tokenType)
                return new Match { Index = -1 };
            
            return new Match()
            {
                Index = index,
                Value = tokens[index]
            };
        }
    }
}