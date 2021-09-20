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
        public virtual Match Match(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            const int index = 0;
            if(index >= tokenList.Count || tokenList[index].Type != _tokenType)
                return new Match { Index = -1 };
            
            return new Match()
            {
                Index = index,
                Value = tokenList[index]
            };
        }
    }
}