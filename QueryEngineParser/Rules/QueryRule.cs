using System.Collections.Generic;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Errors.Syntax;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser.Rules
{
    public class QueryRule : IMatchable
    {
        // from Source where Expression select List
        public Match Match(IList<Token> tokens, int index)
        {
            var fromMatch = new Rule(TokenType.From).Match(tokens, index);
            if(fromMatch.Index == -1)
                throw new MissingFromSyntaxError();
            
            var sourceMatch = new SourceRule().Match(tokens, fromMatch.Index + 1);
            if(sourceMatch.Index == -1)
                throw new MissingSourceSyntaxError();
            
            var whereMatch = new Rule(TokenType.Where).Match(tokens, sourceMatch.Index + 1);
            if(whereMatch.Index == -1)
                throw new MissingWhereSyntaxError();
            
            var expressionMatch = new ExpressionRule().Match(tokens, whereMatch.Index + 1);
            if(expressionMatch.Index == -1)
                throw new MissingExpressionSyntaxError();
            
            var selectMatch = new Rule(TokenType.Select).Match(tokens, expressionMatch.Index + 1);
            if(selectMatch.Index == -1)
                throw new MissingSelectSyntaxError();
            
            var fieldsMatch = new ListRule().Match(tokens, selectMatch.Index + 1);
            if(fieldsMatch.Index == -1)
                throw new MissingFieldsSyntaxError();
            
            return new Match
            {
                Value = new Query
                {
                    Source = (string) sourceMatch.Value,
                    Expression = (Evaluable) expressionMatch.Value,
                    Fields = (IList<string>) fieldsMatch.Value
                }
            };
        }
    }
}