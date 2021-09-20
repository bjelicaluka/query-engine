using System.Collections.Generic;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;
using QueryEngineParser.AST;

namespace QueryEngineParser.Rules
{
    public class RelationalExpressionRule : IMatchable
    {
        public Match Match(IList<Token> tokens, int index)
        {
            var idStringMatch = MatchIdString(tokens, index);
            if (idStringMatch.Index != -1)
                return idStringMatch;
            var idNumMatch = MatchIdNum(tokens, index);
            if (idNumMatch.Index != -1)
                return idNumMatch;
            var stringIdMatch = MatchStringId(tokens, index);
            if (stringIdMatch.Index != -1)
                return stringIdMatch;
            var numIdMatch = MatchNumId(tokens, index);
            if (numIdMatch.Index != -1)
                return numIdMatch;
            return new Match { Index = -1 };
        }

        // Id RelOp String
        private Match MatchIdString(IList<Token> tokens, int index)
        {
            var idMatch = new Rule(TokenType.Id).Match(tokens, index);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokens, idMatch.Index + 1);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var stringMatch = new Rule(TokenType.String).Match(tokens, relOpMatch.Index + 1);
            if(stringMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = stringMatch.Index,
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
        private Match MatchIdNum(IList<Token> tokens, int index)
        {
            var idMatch = new Rule(TokenType.Id).Match(tokens, index);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokens, idMatch.Index + 1);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var numberMatch = new Rule(TokenType.Number).Match(tokens, relOpMatch.Index + 1);
            if(numberMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = numberMatch.Index,
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
        private Match MatchStringId(IList<Token> tokens, int index)
        {
            var stringMatch = new Rule(TokenType.String).Match(tokens, index);
            if(stringMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokens, stringMatch.Index + 1);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var idMatch = new Rule(TokenType.Id).Match(tokens, relOpMatch.Index + 1);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = idMatch.Index,
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
        private Match MatchNumId(IList<Token> tokens, int index)
        {
            var numberMatch = new Rule(TokenType.Number).Match(tokens, index);
            if(numberMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var relOpMatch = new Rule(TokenType.RelOp).Match(tokens, numberMatch.Index + 1);
            if(relOpMatch.Index == -1)
                return new Match{ Index = -1 };
            
            var idMatch = new Rule(TokenType.Id).Match(tokens, relOpMatch.Index + 1);
            if(idMatch.Index == -1)
                return new Match{ Index = -1 };
            
            return new Match
            {
                Index = idMatch.Index,
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