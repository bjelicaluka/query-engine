using System.Collections.Generic;
using System.Text.RegularExpressions;
using QueryEngineModel.Tokens;

namespace QueryEngineScanner
{
    public class Scanner
    {
        private readonly IDictionary<string, TokenType> _tokenRegistry = new Dictionary<string, TokenType>
        {
            {"^from", TokenType.From},
            {"^where", TokenType.Where},
            {"^select", TokenType.Select},
            {"^\\(", TokenType.LParen},
            {"^\\)", TokenType.RParen},
            {"^<>|^>=|^<=|^=|^<|^>", TokenType.RelOp},
            {"^and|^or|^AND|^OR", TokenType.LogOp},
            {"^[_a-zA-Z][_a-zA-Z0-9]*", TokenType.Id},
            {"^'[^']*'|^\"[^\"]*\"", TokenType.String},
            {"^([0-9]+)", TokenType.Number},
            {"^,", TokenType.Comma},
            {"^ ", TokenType.Ws},
        };
        public IEnumerable<Token> Scan(string query)
        {
            var tokens = new List<Token>();

            while (query.Length != 0)
            {
                foreach(var entry in _tokenRegistry)
                {
                    // Try to match the token
                    var match = Regex.Match(query, entry.Key, RegexOptions.Singleline);
                    if (!match.Success) continue;
                    
                    // Eat the token
                    query = query.Substring(match.Length);
                    
                    // Skip whitespaces
                    if (entry.Value == TokenType.Ws) break;
                    
                    // Add token to the list
                    tokens.Add(new Token
                    {
                        Type = entry.Value,
                        Value = match.Value
                    });
                    break;
                }
            }

            return tokens;
        }
    }
}