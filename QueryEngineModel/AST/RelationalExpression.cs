using System;

namespace QueryEngineModel.AST
{
    public class RelationalExpression : ExpressionBase
    {
        public RelationalExpressionOperand Left { get; set; }
        public RelationalExpressionOperand Right { get; set; }
        
        public override bool Evaluate(IAccessible i)
        {
            switch (Operation)
            {
                case "=": return (string) Left.ValueOf(i) == (string) Right.ValueOf(i);
                case "<>": return (string) Left.ValueOf(i) != (string) Right.ValueOf(i);
                case ">=": return (int) Left.ValueOf(i) >= (int) Right.ValueOf(i);
                case "<=": return (int) Left.ValueOf(i) <= (int) Right.ValueOf(i);
                case ">": return (int) Left.ValueOf(i) > (int) Right.ValueOf(i);
                case "<": return (int) Left.ValueOf(i) < (int) Right.ValueOf(i);
                default:
                    throw new Exception("Invalid operation.");
            }
        }
    }
}