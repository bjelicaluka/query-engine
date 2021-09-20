using System.Linq;
using NUnit.Framework;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.AST;
using QueryEngineParser;
using QueryEngineParser.AST;
using QueryEngineParser.Rules;
using QueryEngineScanner;

namespace QueryEngineTests.ParserTests
{
    public class AstTests
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
        public void Parse_Returns_Query()
        {
            // Arrange
            const string query = "from Users where Name = 'luka' select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
        }
        
        [Test]
        public void Parse_Returns_QueryWithSource()
        {
            // Arrange
            const string source = "Users";
            var query = $"from {source} where Name = 'luka' select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.AreEqual(source, result.Source);
        }
        
        [Test]
        public void Parse_Returns_QueryWithField()
        {
            // Arrange
            const string field = "Name";
            var query = $"from Users where Name = 'luka' select {field}";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.Contains(field, result.Fields.ToList());
        }
        
        [Test]
        public void Parse_Returns_QueryWithFields()
        {
            // Arrange
            const string nameField = "Name";
            const string emailField = "Email";
            const string ageField = "Age";
            var query = $"from Users where Name = 'luka' select {nameField}, {emailField}, {ageField}";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.AreEqual(3, result.Fields.Count());
            Assert.Contains(nameField, result.Fields.ToList());
            Assert.Contains(emailField, result.Fields.ToList());
            Assert.Contains(ageField, result.Fields.ToList());
        }
        
        [Test]
        public void Parse_Returns_QueryWithRelationalExpression()
        {
            // Arrange
            const string leftOperand = "Name";
            const string operation = "=";
            const string rightOperand = "'luka'";
            var query = $"from Users where {leftOperand} {operation} {rightOperand} select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.NotNull(result.Expression);
            Assert.IsAssignableFrom<RelationalExpression>(result.Expression);
            Assert.AreEqual(leftOperand, ((RelationalExpression) result.Expression).Left.Value);
            Assert.AreEqual(operation, result.Expression.Operation);
            Assert.AreEqual(rightOperand, ((RelationalExpression) result.Expression).Right.Value);
        }
        
        [Test]
        public void Parse_Returns_QueryWithRelationalExpressionInParentheses()
        {
            // Arrange
            const string leftOperand = "Name";
            const string operation = "=";
            const string rightOperand = "'luka'";
            var query = $"from Users where ({leftOperand} {operation} {rightOperand}) select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.NotNull(result.Expression);
            Assert.IsAssignableFrom<RelationalExpression>(result.Expression);
            Assert.AreEqual(leftOperand, ((RelationalExpression) result.Expression).Left.Value);
            Assert.AreEqual(operation, result.Expression.Operation);
            Assert.AreEqual(rightOperand, ((RelationalExpression) result.Expression).Right.Value);
        }
        
        [Test]
        public void Parse_Returns_QueryWithExpression()
        {
            // Arrange
            const string logOperation = "and";
            var query = $"from Users where Name = 'luka' {logOperation} Age >= 18 select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.NotNull(result.Expression);
            Assert.IsAssignableFrom<Expression>(result.Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) result.Expression).Left);
            Assert.AreEqual(logOperation, result.Expression.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) result.Expression).Right);
        }
        
        [Test]
        public void Parse_Returns_QueryWithComplexExpressionFirst()
        {
            // Arrange
            const string firstLogOperation = "or";
            const string secondLogOperation = "and";
            var query =
                $"from Users where (Name = 'luka' {firstLogOperation} Age >= 18) {secondLogOperation} Email = \"luka@isobarot.com\" select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.NotNull(result.Expression);
            Assert.IsAssignableFrom<Expression>(result.Expression);
            
            Assert.IsAssignableFrom<Expression>(((Expression) result.Expression).Left);
            Assert.AreEqual(secondLogOperation, result.Expression.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) result.Expression).Right);
            
            var expressionInParentheses = (Expression) ((Expression) result.Expression).Left;
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Left);
            Assert.AreEqual(firstLogOperation, expressionInParentheses.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Right);
        }
        
        [Test]
        public void Parse_Returns_QueryWithComplexExpressionSecond()
        {
            // Arrange
            const string firstLogOperation = "or";
            const string secondLogOperation = "and";
            var query =
                $"from Users where Name = 'luka' {firstLogOperation} (Age >= 18 {secondLogOperation} Email = \"luka@isobarot.com\") select Name";
            var tokens = _scanner.Scan(query);
            
            // Act
            var result = _parser.Parse(tokens.ToList());
            
            // Assert
            Assert.IsAssignableFrom<Query>(result);
            Assert.NotNull(result.Expression);
            Assert.IsAssignableFrom<Expression>(result.Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) result.Expression).Left);
            Assert.AreEqual(firstLogOperation, result.Expression.Operation);
            Assert.IsAssignableFrom<Expression>(((Expression) result.Expression).Right);
            var expressionInParentheses = (Expression) ((Expression) result.Expression).Right;
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Left);
            Assert.AreEqual(secondLogOperation, expressionInParentheses.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Right);
        }
    }
}