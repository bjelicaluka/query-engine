using System.Collections.Generic;
using QueryEngineModel.Tokens;
using QueryEngineParser.Rules;

namespace QueryEngineParser
{
    public class Parser
    {
        private readonly IMatchable _rule;
        public Parser(IMatchable rule)
        {
            _rule = rule;
        }
        
        public object Parse(IEnumerable<Token> tokens)
        {
            return _rule.Match(tokens).Value;
        }
    }
}