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
        public override Match Match(IList<Token> tokens, int index)
        {
            var result = base.Match(tokens, index);
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