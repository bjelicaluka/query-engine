
namespace QueryEngineCore.Contracts.AST
{
    public abstract class Evaluable
    {
        public string Operation { get; set; }

        public abstract bool Evaluate(IAccessible a);
    }
}