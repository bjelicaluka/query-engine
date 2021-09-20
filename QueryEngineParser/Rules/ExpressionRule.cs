using System.Collections.Generic;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;
using QueryEngineParser.AST;

namespace QueryEngineParser.Rules
{
    public class ExpressionRule : IMatchable
    {
        public Match Match(IList<Token> tokens, int index)
        {
            var matchedExpression = MatchExpression(tokens, index);
            if (matchedExpression.Index == -1)
                return new Match {Index = -1};

            if (matchedExpression.Index >= tokens.Count - 1 ||
                tokens[matchedExpression.Index + 1].Type != TokenType.LogOp) return matchedExpression;
            
            var childMatch = Match(tokens, matchedExpression.Index + 2);
            return new Match
            {
                Index = childMatch.Index,
                Value = new Expression
                {
                    Left = (Evaluable) matchedExpression.Value,
                    Operation = tokens[matchedExpression.Index + 1].Value,
                    Right = (Evaluable) childMatch.Value
                }
            };
        }

        // Expression
        private Match MatchExpression(IList<Token> tokens, int index)
        {
            var parenthesesMatch = MatchWithParentheses(tokens, index);
            if (parenthesesMatch.Index != -1)
            {
                return parenthesesMatch;
            }
            var expressionMatch = MatchWithLogOperation(tokens, index);
            if (expressionMatch.Index != -1)
            {
                return expressionMatch;
            }
            var relationalExpressionMatch = MatchRelationalExpression(tokens, index);
            if (relationalExpressionMatch.Index != -1)
            {
                return relationalExpressionMatch;
            }
            return new Match { Index = -1 };
        }

        // (Expression)
        private Match MatchWithParentheses(IList<Token> tokens, int index)
        {
            
            var lparenMatch = new Rule(TokenType.LParen).Match(tokens, index);
            if(lparenMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var expressionMatch = Match(tokens, lparenMatch.Index + 1);
            if(expressionMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var rparenMatch = new Rule(TokenType.RParen).Match(tokens, expressionMatch.Index + 1);
            if(rparenMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = rparenMatch.Index,
                Value = expressionMatch.Value
            };
        }
        
        // RelationalExpression LogOp Expression
        private Match MatchWithLogOperation(IList<Token> tokens, int index)
        {
            var relExpressionMatch = new RelationalExpressionRule().Match(tokens, index);
            if(relExpressionMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var logOpMatch = new Rule(TokenType.LogOp).Match(tokens, relExpressionMatch.Index + 1);
            if(logOpMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var expressionMatch = Match(tokens, logOpMatch.Index + 1);
            if(expressionMatch.Index == -1)
                return new Match{ Index = -1 };

            return new Match
            {
                Index = expressionMatch.Index,
                Value = new Expression
                {
                    Left = (Evaluable) relExpressionMatch.Value,
                    Operation = ((Token) logOpMatch.Value).Value,
                    Right = (Evaluable) expressionMatch.Value
                }
            };
        }
        
        // RelationalExpression
        private Match MatchRelationalExpression(IList<Token> tokens, int index)
        {
            var relExpressionMatch = new RelationalExpressionRule().Match(tokens, index);
            return new Match
            {
                Index = relExpressionMatch.Index,
                Value = relExpressionMatch.Value
            };
        }
    }
}