using System;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.AST;

namespace QueryEngineParser.AST
{
    public class Expression : Evaluable
    {
        public Evaluable Left { get; set; }
        public Evaluable Right { get; set; }
        
        public override bool Evaluate(IAccessible a)
        {
            switch (Operation)
            {
                case "and": return Left.Evaluate(a) && Right.Evaluate(a);
                case "or": return Left.Evaluate(a) || Right.Evaluate(a);
                default:
                    throw new Exception("Invalid operation.");
            }
        }
    }
}