namespace QueryEngineModel
{
    public interface IAccessible
    {
        object this[string fieldName] { get; }
    }
}