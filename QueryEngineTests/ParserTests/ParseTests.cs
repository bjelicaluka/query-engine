using System.Linq;
using NUnit.Framework;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.Errors.Syntax;
using QueryEngineParser;
using QueryEngineParser.Rules;
using QueryEngineScanner;

namespace QueryEngineTests.ParserTests
{
    public class ParseTests
    {
        private IScanner _scanner;
        private IParser _parser;

        [SetUp]
        public void Setup()
        {
            _scanner = new Scanner();
            _parser = new Parser(new QueryRule());
        }

        [Test]
        public void Parse_Throws_MissingFromSyntaxError()
        {
            // Arrange
            const string query = "Users";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingFromSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }

        [Test]
        public void Parse_Throws_MissingSourceSyntaxError()
        {
            // Arrange
            const string query = "from ";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingSourceSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }

        [Test]
        public void Parse_Throws_MissingWhereSyntaxError()
        {
            // Arrange
            const string query = "from Users";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingWhereSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }
        
        [Test]
        public void Parse_Throws_MissingExpressionSyntaxError()
        {
            // Arrange
            const string query = "from Users where";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingExpressionSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }
        
        [Test]
        public void Parse_Throws_MissingSelectSyntaxError()
        {
            // Arrange
            const string query = "from Users where Test = 1";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingSelectSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }
        
        [Test]
        public void Parse_Throws_MissingFieldsSyntaxError()
        {
            // Arrange
            const string query = "from Users where Test = 1 select";
            
            // Act
            var tokens = _scanner.Scan(query);

            // Assert
            Assert.Throws<MissingFieldsSyntaxError>(() => _parser.Parse(tokens.ToList()));
        }
        
        [Test]
        public void Parse_Not_Throws()
        {
            // Arrange
            const string query = "from Users where Test = 1 select Field";

            // Act
            _parser.Parse(_scanner.Scan(query).ToList());
            
            // Assert
            Assert.Pass();
        }
    }
}