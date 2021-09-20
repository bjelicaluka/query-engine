using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.Errors.Runtime;

namespace QueryEngineDataSource
{
    public class Accessible : IAccessible
    {
        public virtual object this[string fieldName]
        {
            get
            {
                var field = GetType().GetProperty(fieldName);
                if(field == null)
                    throw new FieldNotFoundRuntimeError();
                return field.GetValue(this, null);
            }
        }
    }
}