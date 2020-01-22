using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace App.Tests
{
    public class ApiStarshipsJsonParserTests
    {
        Mock<ILogger> mockLogger = new Mock<ILogger> ();

        string jsonStringWithAllDataMonth = 
            "{" + 
                "\"next\":\"next_value\", " + 
                "\"results\": " +
                "[" + 
                    "{\"name\":\"TestStarship1\", \"MGLT\":\"10\", \"consumables\":\"1 month\"}," + 
                    "{\"name\":\"TestStarship2\", \"MGLT\":\"20\", \"consumables\":\"1 month\"}" + 
                "]" + 
            "}";

        string jsonStringWithoutValue = 
            "{" + 
                "\"results\": " +
                "[" + 
                    "{\"name\":\"TestStarship1\", \"consumables\":\"1 hour\"}," + 
                    "{\"name\":\"TestStarship2\", \"MGLT\":\"20\", \"consumables\":\"1 hour\"}" + 
                "]" + 
            "}";
        
        [Fact]
        public void TestGetCorrectValueAsString ()
        {
            ApiJsonStarshipsParser parser = new ApiJsonStarshipsParser (jsonStringWithAllDataMonth, mockLogger.Object);
            string result = parser.GetPropertyAsString ("next");
            Assert.Equal ("next_value", result);
        }

        [Fact]
        public void TestGetIncorrectValueAsString ()
        {
            ApiJsonStarshipsParser parser = new ApiJsonStarshipsParser (jsonStringWithoutValue, mockLogger.Object);
            Assert.Throws<KeyNotFoundException> (() => {
                string result = parser.GetPropertyAsString ("next");
            });
        }

        [Fact]
        public void TestParseCorrect ()
        {
            ApiJsonStarshipsParser parser = new ApiJsonStarshipsParser (jsonStringWithAllDataMonth, mockLogger.Object);
            List<Starship> result = parser.Parse ();

            Assert.Equal (2, result.Count);
            Assert.Equal (730, (int)result[0].Consumables);
        }

        [Fact]
        public void TestParseIncorrect ()
        {
            ApiJsonStarshipsParser parser = new ApiJsonStarshipsParser (jsonStringWithoutValue, mockLogger.Object);
            Assert.Throws<AggregateException> (parser.Parse);
        }
    }
}