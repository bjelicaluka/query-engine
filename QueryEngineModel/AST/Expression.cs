using System;

namespace QueryEngineModel.AST
{
    public class Expression : ExpressionBase
    {
        public ExpressionBase Left { get; set; }
        public ExpressionBase Right { get; set; }
        
        public override bool Evaluate(IAccessible i)
        {
            switch (Operation)
            {
                case "and": return Left.Evaluate(i) && Right.Evaluate(i);
                case "or": return Left.Evaluate(i) || Right.Evaluate(i);
                default:
                    throw new Exception("Invalid operation.");
            }
        }
    }
}