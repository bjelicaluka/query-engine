using System.Collections.Generic;
using System.Linq;
using QueryEngineModel.AST;
using QueryEngineModel.Tokens;
using QueryEngineParser.Utils;

namespace QueryEngineParser.Rules
{
    public class ExpressionRule : IMatchable
    {
        public Match Match(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();

            var matchedExpression = MatchExpression(tokenList);
            if (matchedExpression.Index == -1)
                return new Match {Index = -1};
            tokenList = ListUtils.EatTokens(matchedExpression, tokenList);

            if (!tokenList.Any() || tokenList[0].Type != TokenType.LogOp) return matchedExpression;
            
            var childMatch = Match(ListUtils.EatTokens(new Match {Index = 0}, tokenList));
            return new Match
            {
                Index = childMatch.Index + 2 + matchedExpression.Index,
                Value = new Expression
                {
                    Left = (ExpressionBase) matchedExpression.Value,
                    Operation = tokenList[0].Value,
                    Right = (ExpressionBase) childMatch.Value
                }
            };
        }

        private Match MatchExpression(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var parenthesesMatch = MatchWithParentheses(tokenList);
            if (parenthesesMatch.Index != -1)
            {
                return parenthesesMatch;
            }
            var expressionMatch = MatchWithLogOperation(tokenList);
            if (expressionMatch.Index != -1)
            {
                return expressionMatch;
            }
            var relationalExpressionMatch = MatchRelationalExpression(tokenList);
            if (relationalExpressionMatch.Index != -1)
            {
                return relationalExpressionMatch;
            }
            return new Match { Index = -1 };
        }

        // (Expression)
        private Match MatchWithParentheses(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var lparenMatch = new Rule(TokenType.LParen).Match(tokenList);
            if(lparenMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(lparenMatch, tokenList);
            
            var expressionMatch = Match(tokenList);
            if(expressionMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(expressionMatch, tokenList);
            
            var rparenMatch = new Rule(TokenType.RParen).Match(tokenList);
            if(rparenMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = lparenMatch.Index + expressionMatch.Index + rparenMatch.Index + 2,
                Value = expressionMatch.Value
            };
        }
        
        // RelationalExpression LogOp Expression
        private Match MatchWithLogOperation(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var relExpressionMatch = new RelationalExpressionRule().Match(tokenList);
            if(relExpressionMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(relExpressionMatch, tokenList);
            
            var logOpMatch = new Rule(TokenType.LogOp).Match(tokenList);
            if(logOpMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(logOpMatch, tokenList);
            
            var expressionMatch = Match(tokenList);
            if(expressionMatch.Index == -1)
                return new Match{ Index = -1 };

            return new Match
            {
                Index = relExpressionMatch.Index + logOpMatch.Index + expressionMatch.Index + 2,
                Value = new Expression
                {
                    Left = (ExpressionBase) relExpressionMatch.Value,
                    Operation = ((Token) logOpMatch.Value).Value,
                    Right = (ExpressionBase) expressionMatch.Value
                }
            };
        }
        
        // RelationalExpression
        private Match MatchRelationalExpression(IEnumerable<Token> tokens)
        {
            var relExpressionMatch = new RelationalExpressionRule().Match(tokens);
            return new Match
            {
                Index = relExpressionMatch.Index,
                Value = relExpressionMatch.Value
            };
        }
    }
}