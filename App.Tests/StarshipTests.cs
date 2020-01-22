using System;
using Xunit;
using App;

namespace App.Tests
{
    public class StarshipTests
    {
        private readonly Starship _starshipWithDefinedValues;
        private readonly Starship _starshipWithUndefinedValues;

        public StarshipTests ()
        {
            _starshipWithDefinedValues = new Starship ("TestStarship", 100, 1);
            _starshipWithUndefinedValues = new Starship ("TestStarship", 0, 0);
        }

        [Fact]
        public void StarshipWithDefinedValuesTestZeroDistance ()
        {
            var result = _starshipWithDefinedValues.CalculateNumberOfStopsForDistance (0);

            Assert.Equal (0, result);
        }

        [Fact]
        public void StarshipWithUndefinedValuesTestZeroDistance ()
        {
            var result = _starshipWithUndefinedValues.CalculateNumberOfStopsForDistance (0);

            Assert.Equal (0, result);
        }

        [Fact]
        public void StarshipWithUndefinedValuesTestNormalDistance ()
        {
            var result = _starshipWithUndefinedValues.CalculateNumberOfStopsForDistance (100);

            Assert.Equal (-1, result);
        }

        [Fact]
        // Boundary condition test - when starship can cover a distance in an integer number of jumps
        // (point of destination is planned point of resupply)
        public void StarshipWithDefinedValuesTestBoundaryCondition ()
        {
            var result = _starshipWithDefinedValues.CalculateNumberOfStopsForDistance (200);

            Assert.Equal (1, result);
        }

        [Fact]
        //Testing case when number of jumps is not an integer
        public void StarshipWithDefinedValuesTestNonIntegerJumps ()
        {
            var result = _starshipWithDefinedValues.CalculateNumberOfStopsForDistance (199);

            Assert.Equal (1, result);
        }

        [Fact]
        //Testing case when number of jumps is not an integer
        public void StarshipWithDefinedValuesTestZeroJumps ()
        {
            var result = _starshipWithDefinedValues.CalculateNumberOfStopsForDistance (80);

            Assert.Equal (0, result);
        }
    }
}
