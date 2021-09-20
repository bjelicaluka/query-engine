using System.Collections.Generic;
using System.Linq;
using QueryEngineModel.AST;
using QueryEngineModel.Tokens;
using QueryEngineParser.Errors.Syntax;
using QueryEngineParser.Utils;

namespace QueryEngineParser.Rules
{
    public class QueryRule : IMatchable
    {
        public Match Match(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var fromMatch = new Rule(TokenType.From).Match(tokenList);
            if(fromMatch.Index == -1)
                throw new MissingFromSyntaxError();
            tokenList = ListUtils.EatTokens(fromMatch, tokenList);
            
            var sourceMatch = new SourceRule().Match(tokenList);
            if(sourceMatch.Index == -1)
                throw new MissingSourceSyntaxError();
            tokenList = ListUtils.EatTokens(sourceMatch, tokenList);
            
            var whereMatch = new Rule(TokenType.Where).Match(tokenList);
            if(whereMatch.Index == -1)
                throw new MissingWhereSyntaxError();
            tokenList = ListUtils.EatTokens(whereMatch, tokenList);
            
            var expressionMatch = new ExpressionRule().Match(tokenList);
            if(expressionMatch.Index == -1)
                throw new MissingExpressionSyntaxError();
            tokenList = ListUtils.EatTokens(expressionMatch, tokenList);
            
            var selectMatch = new Rule(TokenType.Select).Match(tokenList);
            if(selectMatch.Index == -1)
                throw new MissingSelectSyntaxError();
            tokenList = ListUtils.EatTokens(selectMatch, tokenList);
            
            var fieldsMatch = new ListRule().Match(tokenList);
            if(fieldsMatch.Index == -1)
                throw new MissingFieldsSyntaxError();
            
            return new Match
            {
                Value = new Query
                {
                    Source = (string) sourceMatch.Value,
                    Expression = (ExpressionBase) expressionMatch.Value,
                    Fields = (IList<string>) fieldsMatch.Value
                }
            };
        }
    }
}