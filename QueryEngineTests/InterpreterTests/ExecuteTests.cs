using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QueryEngineCore.Contracts.Errors.Runtime;
using QueryEngineDataSource;
using QueryEngineDataSource.Entities;
using QueryEngineInterpreter;
using QueryEngineParser;
using QueryEngineParser.Rules;
using QueryEngineScanner;


namespace QueryEngineTests.InterpreterTests
{
    public class ExecuteTests
    {
        private Scanner _scanner;
        private Parser _parser;
        private Interpreter _interpreter;
        
        [SetUp]
        public void Setup()
        {
            _scanner = new Scanner();
            _parser = new Parser(new QueryRule());
        }

        [Test]
        public void Execute_Returns_AgeOfOneUser()
        {
            // Arrange
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
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from Users where Email = 'luka@isobarot.com' and Name = \"Luka Bjelica\" select Age";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
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
        
        [Test]
        public void Execute_Returns_NameAndEmailOfTwoUsers()
        {
            // Arrange
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
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from Users where Age >= 10 and (Name = \"Luka Bjelica\" or Name = 'Test User') select Name, Email";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            var result = _interpreter.Execute(queryAst).ToList();
            
            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Luka Bjelica"));
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Test User"));
            Assert.IsTrue(result.Any(a => (string) a["Email"] == "luka@isobarot.com"));
            Assert.IsTrue(result.Any(a => (string) a["Email"] == "test@isobarot.com"));
            Assert.Throws<FieldNotFoundRuntimeError>(() =>
            {
                var shouldThrow = result.Any(a => a["Age"] == null);
            });
        }
        
        [Test]
        public void Execute_ForStringField_Throws_InvalidTypesRuntimeError()
        {
            // Arrange
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
                }
            };
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from Users where Email >= 1 select Age";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            // Assert
            Assert.Throws<InvalidTypesInRelationalExpressionRuntimeError>(() =>
            {
                _interpreter.Execute(queryAst);
            });
        }
        
        [Test]
        public void Execute_ForIntField_Throws_InvalidTypesRuntimeError()
        {
            // Arrange
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
                }
            };
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from Users where Age >= \"Test\" select Age";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            // Assert
            Assert.Throws<InvalidTypesInRelationalExpressionRuntimeError>(() =>
            {
                _interpreter.Execute(queryAst);
            });
        }
        
        [Test]
        public void Execute_ForInvalidSource_Throws_InvalidSourceRuntimeError()
        {
            // Arrange
            var dataSource = new DataSource
            {
                Users = new List<User>()
            };
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from NotExisting where Age >= 1 select Age";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            // Assert
            Assert.Throws<InvalidSourceRuntimeError>(() =>
            {
                _interpreter.Execute(queryAst);
            });
        }
        
        [Test]
        public void Execute_ForInvalidField_Throws_FieldNotFoundRuntimeError()
        {
            // Arrange
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
                }
            };
            _interpreter = new Interpreter(dataSource);
            
            const string query = "from Users where Age >= 1 select NotExisting";
            var queryAst = _parser.Parse(_scanner.Scan(query).ToList());
            
            // Act
            // Assert
            Assert.Throws<FieldNotFoundRuntimeError>(() =>
            {
                _interpreter.Execute(queryAst);
            });
        }
    }
}