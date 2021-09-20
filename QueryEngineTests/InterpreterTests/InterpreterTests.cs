using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QueryEngineDataSource;
using QueryEngineDataSource.Entities;
using QueryEngineInterpreter;
using QueryEngineInterpreter.Errors.Runtime;
using QueryEngineModel.AST;
using QueryEngineParser;
using QueryEngineParser.Rules;
using QueryEngineScanner;

namespace QueryEngineTests.InterpreterTests
{
    public class InterpreterTests
    {
        private Scanner _scanner;
        private Parser _parser;
        private Interpreter _interpreter;
        
        [SetUp]
        public void Setup()
        {
            var dataSource = new DataSource
            {
                Users = new List<User>
                {
                    new User
                    {
                        Name = "Luka Bjelica",
                        Email = "luka@isobarot.com",
                        Age = 21
                    },
                    new User
                    {
                        Name = "Test User",
                        Email = "test@isobarot.com",
                        Age = 66
                    },
                }
            };
            _scanner = new Scanner();
            _parser = new Parser(new QueryRule());
            _interpreter = new Interpreter(dataSource);
        }

        [Test]
        public void Execute_Returns_AgeOfOneUser()
        {
            // Arrange
            const string query = "from Users where Email = 'luka@isobarot.com' and Name = \"Luka Bjelica\" select Age";
            var queryAst = (Query) _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            var result = _interpreter.Execute(queryAst).ToList();
            
            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(a => (int) a["Age"] == 21));
            Assert.IsFalse(result.Any(a => (int) a["Age"] == 66));
            Assert.Throws<FieldNotFoundRuntimeError>(() =>
            {
                var shouldThrow = result.Any(a => a["Name"] == null);
            });
        }
    }
}