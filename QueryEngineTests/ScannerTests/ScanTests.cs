using System.Linq;
using NUnit.Framework;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.Tokens;
using QueryEngineScanner;

namespace QueryEngineTests.ScannerTests
{
    public class ScanTests
    {
        private IScanner _scanner;
        
        [SetUp]
        public void Setup()
        {
            _scanner = new Scanner();
        }

        [Test]
        public void Scan_Detects_FromToken()
        {
            // Arrange
            const string query = "from ";
            
            // Act
            var tokens = _scanner.Scan(query);
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.From));
        }
        
        [Test]
        public void Scan_Detects_WhereToken()
        {
            // Arrange
            const string query = " where ";
            
            // Act
            var tokens = _scanner.Scan(query);
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Where));
        }
        
        [Test]
        public void Scan_Detects_SelectToken()
        {
            // Arrange
            const string query = " select ";
            
            // Act
            var tokens = _scanner.Scan(query);
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Select));
        }
        
        [Test]
        public void Scan_Detects_IdToken()
        {
            // Arrange
            const string id = "TestId";
            const string idWithNum = "TestId2";
            var query = $"({id}, {idWithNum})";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Id && token.Value == id));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Id && token.Value == idWithNum));
        }
        
        [Test]
        public void Scan_Detects_StringToken()
        {
            // Arrange
            const string stringLitDq = "\"jobs@ravendb.net\"";
            const string stringLitSq = "\'email1@ravendb.net\'";
            const string stringLitWs = "\'Luka Bjelica\'";
            
            var query = $"{stringLitDq} {stringLitSq} {stringLitWs}";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.String && token.Value == stringLitDq));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.String && token.Value == stringLitSq));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.String && token.Value == stringLitWs));
        }
        
        [Test]
        public void Scan_Detects_NumberToken()
        {
            // Arrange
            const string numLit = "55";
            
            var query = $"{numLit} ";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Number && token.Value == numLit));
        }
        
        [Test]
        public void Scan_Detects_RelOpToken()
        {
            // Arrange
            const string query = "> >= < <= = <> ";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == ">"));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == ">="));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == "<"));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == "<="));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == "="));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RelOp && token.Value == "<>"));
        }
        
        [Test]
        public void Scan_Detects_LogOpToken()
        {
            // Arrange
            const string query = "and or AND OR ";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.LogOp && token.Value == "and"));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.LogOp && token.Value == "or"));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.LogOp && token.Value == "AND"));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.LogOp && token.Value == "OR"));
        }
        
        [Test]
        public void Scan_Detects_CommaToken()
        {
            // Arrange
            const string query = "ff,";
            
            // Act
            var tokens = _scanner.Scan(query);
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.Comma && token.Value == ","));
        }
        
        [Test]
        public void Scan_Detects_ParenToken()
        {
            // Arrange
            const string query = "(Test) ";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.LParen && token.Value == "("));
            Assert.NotNull(tokens.FirstOrDefault(token => token.Type == TokenType.RParen && token.Value == ")"));
        }
        
        [Test]
        public void Scan_Skips_Whitespaces()
        {
            // Arrange
            const string query = "  ";
            
            // Act
            var tokens = _scanner.Scan(query).ToList();
            
            // Assert
            Assert.IsEmpty(tokens);
        }
    }
}