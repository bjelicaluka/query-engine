using System.Collections.Generic;
using System.Linq;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Errors.Runtime;

namespace QueryEngineInterpreter
{
    public class Interpreter : IInterpreter
    {
        private readonly IAccessible _accessible;

        public Interpreter(IAccessible accessible)
        {
            _accessible = accessible;
        }
        
        public IEnumerable<IAccessible> Execute(Query query)
        {
            return ((IEnumerable<IAccessible>) _accessible[query.Source])
                .Where(a => query.Expression.Evaluate(a))
                .Select(a =>
                {
                    var result = new SingleQueryResult();
                    query.Fields.ToList().ForEach(f => result[f] = a[f]);
                    return result;
                })
                .ToList()
            ;
        }
    }
}