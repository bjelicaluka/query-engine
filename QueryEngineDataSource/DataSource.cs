using System.Collections.Generic;
using QueryEngineDataSource.Entities;

namespace QueryEngineDataSource
{
    public class DataSource : Accessible
    {
        public List<User> Users { get; set; }
    }
}