using System.Collections.Generic;
using QueryEngineCore.Contracts;

namespace QueryEngineCore
{
    public class QueryEngine : IQueryEngine
    {
        private readonly IScanner _scanner;
        private readonly IParser _parser;
        private readonly IInterpreter _interpreter;
        
        public QueryEngine(IScanner scanner, IParser parser, IInterpreter interpreter)
        {
            _scanner = scanner;
            _parser = parser;
            _interpreter = interpreter;
        }
        
        public IEnumerable<IAccessible> Run(string query)
        {
            var tokens = _scanner.Scan(query);
            var queryAst = _parser.Parse(tokens);
            return _interpreter.Execute(queryAst);
        }
    }
}