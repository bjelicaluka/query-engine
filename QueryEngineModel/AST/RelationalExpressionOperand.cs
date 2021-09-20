
using System;

namespace QueryEngineModel.AST
{
    public class RelationalExpressionOperand
    {
        public RelationalExpressionOperandType Type { get; set; }
        public string Value { get; set; }

        public object ValueOf(IAccessible i)
        {
            switch (Type)
            {
                case RelationalExpressionOperandType.Field: return i[Value];
                case RelationalExpressionOperandType.String: return Value.Trim('\'').Trim('"');
                case RelationalExpressionOperandType.Num: return int.Parse(Value);
                default:
                    throw new Exception("Invalid operand.");
            }
        }
    }
}