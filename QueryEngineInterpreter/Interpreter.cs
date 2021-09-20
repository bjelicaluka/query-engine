using System.Collections.Generic;
using System.Linq;
using QueryEngineModel;
using QueryEngineModel.AST;

namespace QueryEngineInterpreter
{
    public class Interpreter
    {
        private readonly IAccessible _accessible;

        public Interpreter(IAccessible accessible)
        {
            _accessible = accessible;
        }
        
        public IEnumerable<IAccessible> Execute(Query query)
        {
            return ((IEnumerable<IAccessible>) _accessible[query.Source])
                .Where(i => query.Expression.Evaluate(i))
                .Select(i =>
                {
                    var result = new SingleQueryResult();
                    query.Fields.ToList().ForEach(f => result[f] = i[f]);
                    return result;
                })
            ;
        }
    }
}