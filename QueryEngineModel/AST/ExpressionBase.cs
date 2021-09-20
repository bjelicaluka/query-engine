
namespace QueryEngineModel.AST
{
    public abstract class ExpressionBase
    {
        public string Operation { get; set; }

        public abstract bool Evaluate(IAccessible i);
    }
}