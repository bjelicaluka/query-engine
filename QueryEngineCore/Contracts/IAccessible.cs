namespace QueryEngineCore.Contracts
{
    public interface IAccessible
    {
        object this[string fieldName] { get; }
    }
}