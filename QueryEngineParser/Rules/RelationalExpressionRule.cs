using System.Collections.Generic;
using System.Linq;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;
using QueryEngineParser.AST;
using QueryEngineParser.Utils;

namespace QueryEngineParser.Rules
{
    public class RelationalExpressionRule : IMatchable
    {
        public Match Match(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var idStringMatch = MatchIdString(tokenList);
            if (idStringMatch.Index != -1)
                return idStringMatch;
            var idNumMatch = MatchIdNum(tokenList);
            if (idNumMatch.Index != -1)
                return idNumMatch;
            var stringIdMatch = MatchStringId(tokenList);
            if (stringIdMatch.Index != -1)
                return stringIdMatch;
            var numIdMatch = MatchNumId(tokenList);
            if (numIdMatch.Index != -1)
                return numIdMatch;
            return new Match { Index = -1 };
        }

        // Id RelOp String
        private Match MatchIdString(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var idMatch = new Rule(TokenType.Id).Match(tokenList);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(idMatch, tokenList);
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokenList);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(relOpMatch, tokenList);
            
            var stringMatch = new Rule(TokenType.String).Match(tokenList);
            if(stringMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = idMatch.Index + relOpMatch.Index + stringMatch.Index + 2,
                Value = new RelationalExpression
                {
                    Left = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Field,
                        Value = ((Token) idMatch.Value).Value
                    },
                    Operation = ((Token) relOpMatch.Value).Value,
                    Right = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.String,
                        Value = ((Token) stringMatch.Value).Value
                    },
                }
            };
        }
        
        // Id RelOp Number
        private Match MatchIdNum(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var idMatch = new Rule(TokenType.Id).Match(tokenList);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(idMatch, tokenList);
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokenList);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(relOpMatch, tokenList);
            
            var numberMatch = new Rule(TokenType.Number).Match(tokenList);
            if(numberMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = idMatch.Index + relOpMatch.Index + numberMatch.Index + 2,
                Value = new RelationalExpression
                {
                    Left = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Field,
                        Value = ((Token) idMatch.Value).Value
                    },
                    Operation = ((Token) relOpMatch.Value).Value,
                    Right = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Num,
                        Value = ((Token) numberMatch.Value).Value
                    },
                }
            };
        }
        
        // String RelOp Id
        private Match MatchStringId(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var stringMatch = new Rule(TokenType.String).Match(tokenList);
            if(stringMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(stringMatch, tokenList);
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokenList);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(relOpMatch, tokenList);
            
            var idMatch = new Rule(TokenType.Id).Match(tokenList);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = stringMatch.Index + relOpMatch.Index + idMatch.Index + 2,
                Value = new RelationalExpression
                {
                    Left = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.String,
                        Value = ((Token) stringMatch.Value).Value
                    },
                    Operation = ((Token) relOpMatch.Value).Value,
                    Right = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Field,
                        Value = ((Token) idMatch.Value).Value
                    },
                }
            };
        }
        
        // Number RelOp Id
        private Match MatchNumId(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();
            
            var numberMatch = new Rule(TokenType.Number).Match(tokenList);
            if(numberMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(numberMatch, tokenList);
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokenList);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            tokenList = ListUtils.EatTokens(relOpMatch, tokenList);
            
            var idMatch = new Rule(TokenType.Id).Match(tokenList);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = numberMatch.Index + relOpMatch.Index + idMatch.Index + 2,
                Value = new RelationalExpression
                {
                    Left = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Num,
                        Value = ((Token) numberMatch.Value).Value
                    },
                    Operation = ((Token) relOpMatch.Value).Value,
                    Right = new RelationalExpressionOperand
                    {
                        Type = RelationalExpressionOperandType.Field,
                        Value = ((Token) idMatch.Value).Value
                    },
                }
            };
        }

        
    }
}