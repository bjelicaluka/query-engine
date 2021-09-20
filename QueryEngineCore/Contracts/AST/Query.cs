using System.Collections.Generic;

namespace QueryEngineCore.Contracts.AST
{
    public class Query
    {
        public string Source { get; set; }
        public Evaluable Expression { get; set; }
        public IEnumerable<string> Fields { get; set; }
    }
}