using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QueryEngineCore;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.Errors.Runtime;
using QueryEngineDataSource;
using QueryEngineDataSource.Entities;
using QueryEngineInterpreter;
using QueryEngineParser;
using QueryEngineParser.Rules;
using QueryEngineScanner;


namespace QueryEngineTests.QueryEngineTests
{
    public class RunTests
    {
        private Scanner _scanner;
        private Parser _parser;
        private Interpreter _interpreter;
        private IQueryEngine _queryEngine;
        
        [SetUp]
        public void Setup()
        {
            var dataSource = new DataSource
            {
                MiddleEarthPlaces = new List<MiddleEarthPlace>
                {
                    new MiddleEarthPlace
                    {
                        Name = "Rohan",
                        Population = 500_000
                    },
                    new MiddleEarthPlace
                    {
                        Name = "Gondor",
                        Population = 45_000
                    },
                    new MiddleEarthPlace
                    {
                        Name = "Shire",
                        Population = 65_000
                    },
                }
            };
            _scanner = new Scanner();
            _parser = new Parser(new QueryRule());
            _interpreter = new Interpreter(dataSource);
            _queryEngine = new QueryEngine(_scanner, _parser, _interpreter);
        }

        [Test]
        public void Run_Returns_LowPopulatedMiddleEarthPlaces()
        {
            // Arrange
            const string query = "from MiddleEarthPlaces where Population >= 40000 and Population <= 100000 select Name";
            
            // Act
            var result = _queryEngine.Run(query).ToList();
            
            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Shire"));
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Gondor"));
        }
        
        [Test]
        public void Run_Returns_HighPopulatedMiddleEarthPlaces()
        {
            // Arrange
            const string query = "from MiddleEarthPlaces where Population >= 500000 select Name";
            
            // Act
            var result = _queryEngine.Run(query).ToList();
            
            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Rohan"));
        }
        
        [Test]
        public void Run_Returns_HobbitMiddleEarthPlaces()
        {
            // Arrange
            const string query = "from MiddleEarthPlaces where Name <> 'Gondor' and Name <> 'Rohan' select Name";
            
            // Act
            var result = _queryEngine.Run(query).ToList();
            
            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(a => (string) a["Name"] == "Shire"));
        }
    }
}