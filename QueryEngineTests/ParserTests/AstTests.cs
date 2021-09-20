using System.Linq;
using NUnit.Framework;
using QueryEngineModel.AST;
using QueryEngineParser;
using QueryEngineParser.Rules;
using QueryEngineScanner;

namespace QueryEngineTests.ParserTests
{
    public class AstTests
    {
        private Scanner _scanner;
        private Parser _parser;

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
            Assert.AreEqual(source, ((Query) result).Source);
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
            Assert.Contains(field, ((Query) result).Fields.ToList());
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
            Assert.AreEqual(3, ((Query) result).Fields.Count);
            Assert.Contains(nameField, ((Query) result).Fields.ToList());
            Assert.Contains(emailField, ((Query) result).Fields.ToList());
            Assert.Contains(ageField, ((Query) result).Fields.ToList());
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
            Assert.NotNull(((Query) result).Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Query) result).Expression);
            Assert.AreEqual(leftOperand, ((RelationalExpression) ((Query) result).Expression).Left.Value);
            Assert.AreEqual(operation, ((Query) result).Expression.Operation);
            Assert.AreEqual(rightOperand, ((RelationalExpression) ((Query) result).Expression).Right.Value);
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
            Assert.NotNull(((Query) result).Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Query) result).Expression);
            Assert.AreEqual(leftOperand, ((RelationalExpression) ((Query) result).Expression).Left.Value);
            Assert.AreEqual(operation, ((Query) result).Expression.Operation);
            Assert.AreEqual(rightOperand, ((RelationalExpression) ((Query) result).Expression).Right.Value);
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
            Assert.NotNull(((Query) result).Expression);
            Assert.IsAssignableFrom<Expression>(((Query) result).Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) ((Query) result).Expression).Left);
            Assert.AreEqual(logOperation, ((Query) result).Expression.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) ((Query) result).Expression).Right);
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
            Assert.NotNull(((Query) result).Expression);
            Assert.IsAssignableFrom<Expression>(((Query) result).Expression);
            
            Assert.IsAssignableFrom<Expression>(((Expression) ((Query) result).Expression).Left);
            Assert.AreEqual(secondLogOperation, ((Query) result).Expression.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) ((Query) result).Expression).Right);
            
            var expressionInParentheses = (Expression) ((Expression) ((Query) result).Expression).Left;
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
            Assert.NotNull(((Query) result).Expression);
            Assert.IsAssignableFrom<Expression>(((Query) result).Expression);
            Assert.IsAssignableFrom<RelationalExpression>(((Expression) ((Query) result).Expression).Left);
            Assert.AreEqual(firstLogOperation, ((Query) result).Expression.Operation);
            Assert.IsAssignableFrom<Expression>(((Expression) ((Query) result).Expression).Right);
            var expressionInParentheses = (Expression) ((Expression) ((Query) result).Expression).Right;
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Left);
            Assert.AreEqual(secondLogOperation, expressionInParentheses.Operation);
            Assert.IsAssignableFrom<RelationalExpression>(expressionInParentheses.Right);
        }
    }
}