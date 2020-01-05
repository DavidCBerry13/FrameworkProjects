using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace DavidBerry.Framework.Spatial.Tests
{
    public class CompassDirectionTests
    {

        [Theory]
        [InlineData(0.0, "N")]
        [InlineData(22.5, "NNE")]
        [InlineData(45.0, "NE")]
        [InlineData(67.5, "ENE")]
        [InlineData(90.0, "E")]
        [InlineData(112.5, "ESE")]
        [InlineData(135.0, "SE")]
        [InlineData(157.5, "SSE")]
        [InlineData(180.0, "S")]
        [InlineData(202.5, "SSW")]
        [InlineData(225.0, "SW")]
        [InlineData(247.5, "WSW")]
        [InlineData(270.0, "W")]
        [InlineData(292.5, "WNW")]
        [InlineData(315.0, "NW")]
        [InlineData(337.5, "NNW")]
        public void CheckBearingsResultInCorrectCompassDirection(double bearing, string expectedDirection)
        {
            // Act
            var direction = CompassDirection.GetDirection(bearing);

            // Assert
            direction.Abbreviation.Should().Be(expectedDirection,
                $"Expected direction of {expectedDirection} but got {direction.Abbreviation} for bearing {bearing}");
        }


        [Theory]
        [InlineData(348.75, "N")]
        [InlineData(11.25, "N")]
        [InlineData(78.75, "E")]
        [InlineData(101.25, "E")]
        [InlineData(168.75, "S")]
        [InlineData(191.25, "S")]
        [InlineData(258.75, "W")]
        [InlineData(281.25, "W")]
        public void CheckBoundaryBearingsReturCorrectDirectionForCardinalDirections(double bearing, string expectedDirection)
        {
            // Act
            var direction = CompassDirection.GetDirection(bearing);

            // Assert
            direction.Abbreviation.Should().Be(expectedDirection,
                $"Expected direction of {expectedDirection} but got {direction.Abbreviation} for bearing {bearing}");
        }


        [Theory]
        [InlineData(33.75, "NE")]
        [InlineData(56.25, "NE")]
        [InlineData(123.75, "SE")]
        [InlineData(146.25, "SE")]
        [InlineData(213.75, "SW")]
        [InlineData(236.25, "SW")]
        [InlineData(303.75, "NW")]
        [InlineData(326.25, "NW")]
        public void CheckBoundaryBearingsReturCorrectDirectionForIntercardinalDirections(double bearing, string expectedDirection)
        {
            // Act
            var direction = CompassDirection.GetDirection(bearing);

            // Assert
            direction.Abbreviation.Should().Be(expectedDirection,
                $"Expected direction of {expectedDirection} but got {direction.Abbreviation} for bearing {bearing}");
        }



        [Theory]
        [InlineData(-0.1)]
        [InlineData(360.1)]
        public void GetDirectionThrowsExceptionWhenIllegalBearingPassedIn(double bearing)
        {
            // Act And Assert
            Assert.Throws<ArgumentException>(() => CompassDirection.GetDirection(bearing));

        }


        [Fact]
        public void CheckEqualsReturnsFalseWhenNullPassedIn()
        {
            var result = CompassDirection.NORTH.Equals(null);

            result.Should().BeFalse();
        }


        [Fact]
        public void CheckAllDirectionsEqualThemselves()
        {
            CompassDirection.NORTH.Equals(CompassDirection.NORTH).Should().BeTrue();
            CompassDirection.NORTH_NORTHEAST.Equals(CompassDirection.NORTH_NORTHEAST).Should().BeTrue();
            CompassDirection.NORTHEAST.Equals(CompassDirection.NORTHEAST).Should().BeTrue();
            CompassDirection.EAST_NORTHEAST.Equals(CompassDirection.EAST_NORTHEAST).Should().BeTrue();
            CompassDirection.EAST.Equals(CompassDirection.EAST).Should().BeTrue();
            CompassDirection.EAST_SOUTHEAST.Equals(CompassDirection.EAST_SOUTHEAST).Should().BeTrue();
            CompassDirection.SOUTHEAST.Equals(CompassDirection.SOUTHEAST).Should().BeTrue();
            CompassDirection.SOUTH_SOUTHEAST.Equals(CompassDirection.SOUTH_SOUTHEAST).Should().BeTrue();
            CompassDirection.SOUTH.Equals(CompassDirection.SOUTH).Should().BeTrue();
            CompassDirection.SOUTH_SOUTHWEST.Equals(CompassDirection.SOUTH_SOUTHWEST).Should().BeTrue();
            CompassDirection.SOUTHWEST.Equals(CompassDirection.SOUTHWEST).Should().BeTrue();
            CompassDirection.WEST_SOUTHWEST.Equals(CompassDirection.WEST_SOUTHWEST).Should().BeTrue();
            CompassDirection.WEST.Equals(CompassDirection.WEST).Should().BeTrue();
            CompassDirection.WEST_NORTHWEST.Equals(CompassDirection.WEST_NORTHWEST).Should().BeTrue();
            CompassDirection.NORTHWEST.Equals(CompassDirection.NORTHWEST).Should().BeTrue();
            CompassDirection.NORTH_NORTHWEST.Equals(CompassDirection.NORTH_NORTHWEST).Should().BeTrue();
        }


        [Fact]
        public void CheckDirectionsThatDoNotEqualReturnFalse()
        {
            CompassDirection.NORTH.Equals(CompassDirection.EAST).Should().BeFalse();
        }

        [Fact]
        public void CheckAllDirectionsHaveUniqueAbbreviations()
        {
            var uniquePoints = CompassDirection.COMPASS_POINTS
                .Select(p => p.Abbreviation)
                .Distinct()
                .Count();

            uniquePoints.Should().Be(CompassDirection.COMPASS_POINTS.Length);
        }


        [Fact]
        public void CheckHashcodesDifferentForDifferentCompassPoints()
        {
            var uniqueHashCodes = CompassDirection.COMPASS_POINTS
                .Select(p => p.GetHashCode())
                .Distinct()
                .Count();

            uniqueHashCodes.Should().Be(CompassDirection.COMPASS_POINTS.Length);
        }


        [Theory]
        [InlineData(-0.1)]
        [InlineData(360.0001)]
        public void CheckIsBearingInRangeThrowsExceptionWhenInvalidBearingPassedIn(double bearing)
        {
            Assert.Throws<ArgumentException>(() => CompassDirection.NORTH.IsBearingInRange(bearing));
        }




    }


}
