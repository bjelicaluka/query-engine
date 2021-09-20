using System.Collections.Generic;
using QueryEngineCore.Contracts.Errors.Runtime;
using QueryEngineDataSource.Entities;

namespace QueryEngineDataSource
{
    public class DataSource : Accessible
    {
        public List<User> Users { get; set; }
        public List<MiddleEarthPlace> MiddleEarthPlaces { get; set; }
        
        public override object this[string fieldName]
        {
            get
            {
                var field = GetType().GetProperty(fieldName);
                if(field == null)
                    throw new InvalidSourceRuntimeError();
                return field.GetValue(this, null);
            }
        }
    }
}