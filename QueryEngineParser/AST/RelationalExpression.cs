using System;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Errors.Runtime;

namespace QueryEngineParser.AST
{
    public class RelationalExpression : Evaluable
    {
        public RelationalExpressionOperand Left { get; set; }
        public RelationalExpressionOperand Right { get; set; }
        
        public override bool Evaluate(IAccessible a)
        {
            switch (Operation)
            {
                case "=": return (string) Left.ValueOf(a) == (string) Right.ValueOf(a);
                case "<>": return (string) Left.ValueOf(a) != (string) Right.ValueOf(a);
                case ">=":
                {
                    ValidateOperandTypes(a);
                    return (int) Left.ValueOf(a) >= (int) Right.ValueOf(a);
                }
                case "<=":
                {
                    ValidateOperandTypes(a);
                    return (int) Left.ValueOf(a) <= (int) Right.ValueOf(a);
                }
                case ">":
                {
                    ValidateOperandTypes(a);
                    return (int) Left.ValueOf(a) > (int) Right.ValueOf(a);
                }
                case "<":
                {
                    ValidateOperandTypes(a);
                    return (int) Left.ValueOf(a) < (int) Right.ValueOf(a);
                }
                default:
                    throw new Exception("Invalid operation.");
            }
        }

        private void ValidateOperandTypes(IAccessible a)
        {
            if(Left.ValueOf(a) is string || Right.ValueOf(a) is string)
                throw new InvalidTypesInRelationalExpressionRuntimeError();
        }
    }
}