using QueryEngineModel;

namespace QueryEngineDataSource
{
    public class Accessible : IAccessible
    {
        public virtual object this[string fieldName] => GetType().GetProperty(fieldName)?.GetValue(this, null);
    }
}