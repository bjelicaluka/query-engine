using System.Collections.Generic;

namespace QueryEngineModel.AST
{
    public class Query
    {
        public string Source { get; set; }
        public ExpressionBase Expression { get; set; }
        public IList<string> Fields { get; set; }
    }
}