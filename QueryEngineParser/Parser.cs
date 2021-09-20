using System.Collections.Generic;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.AST;
using QueryEngineCore.Contracts.Rules;
using QueryEngineCore.Contracts.Tokens;

namespace QueryEngineParser
{
    public class Parser : IParser
    {
        private readonly IMatchable _rule;
        public Parser(IMatchable rule)
        {
            _rule = rule;
        }
        
        public Query Parse(IEnumerable<Token> tokens)
        {
            return (Query) _rule.Match(tokens).Value;
        }
    }
}