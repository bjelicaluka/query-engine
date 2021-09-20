using System.Collections.Generic;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser.Rules
{
    public class SourceRule : Rule
    {
        public SourceRule() : base(TokenType.Id)
        {
        }
        
        // Id
        public override Match Match(IEnumerable<Token> tokens)
        {
            var result = base.Match(tokens);
            if(result.Index == -1)
                return new Match { Index = -1};
            return new Match
            {
                Index = result.Index,
                Value = ((Token) result.Value).Value
            };
        }
    }
}