using System;
using System.Collections.Generic;
using System.Text;
using UnitsNet;
using Xunit;
using FluentAssertions;
using UnitsNet.Units;
using DavidBerry.Framework.Spatial;

namespace DavidBerry.Framework.Spatial.Tests
{
    public class IGeoCoordinateExtensionsTests
    {


        [Theory]
        [InlineData(-22.951807, -43.210462, 40.689311, -74.044503, 7761382.13)]  // Christ the Redeemer Statue to Statue of Liberty
        [InlineData(41.866738, -87.616977, 41.867843, -87.613981, 276.85)]  // Field Museum to Shedd Aquarium
        public void CheckDistanceCalculationCorrect(double originLatitude, double originLongitude,
            double destinationLatitude, double destinationLongitude, double expectedDistance)
        {
            GeoCoordinate origin = new GeoCoordinate(originLatitude, originLongitude);
            GeoCoordinate destination = new GeoCoordinate(destinationLatitude, destinationLongitude);

            var distanceInMeters = origin.HaversineDistance(destination);

            // Assert
            distanceInMeters.Value.Should().BeApproximately(expectedDistance, 0.005);
        }


        [Fact]
        public void CheckDefaultDistanceUnitsIsMeters()
        {            
            // Arrange
            GeoCoordinate origin = new GeoCoordinate(41.866738, -87.616977);
            GeoCoordinate destination = new GeoCoordinate(40.689311, -74.044503);

            // Act
            var distance = origin.HaversineDistance(destination);

            //Assert
            distance.Unit.Should().Be(LengthUnit.Meter);
        }


        [Fact]
        public void CheckDistanceUnitsPassedInIsUsedInResult()
        {
            // Arrange
            GeoCoordinate origin = new GeoCoordinate(41.890442, 12.492263);        // The Colosseum in Rome
            GeoCoordinate destination = new GeoCoordinate(48.858473, 2.294495);    // Eiffel Tower

            // Act
            var distance = origin.HaversineDistance(destination, LengthUnit.Mile);

            // Assert
            distance.Unit.Should().Be(LengthUnit.Mile);
            distance.Value.Should().BeApproximately(689.36, 0.01);
        }



        [Theory]
        [InlineData(41.866738, -87.616977, 40.689311, -74.044503, 92.065)]
        [InlineData(41.866738, -87.616977, -22.951807, -43.210462, 138.544)]
        public void CheckInitialBearingCalculations(double originLatitude, double originLongitude,
            double destinationLatitude, double destinationLongitude, double expectedBearing)
        {
            GeoCoordinate origin = new GeoCoordinate(originLatitude, originLongitude);
            GeoCoordinate destination = new GeoCoordinate(destinationLatitude, destinationLongitude);

            var bearing = origin.InitialBearing(destination);

            bearing.Unit.Should().Be(AngleUnit.Degree);
            bearing.Value.Should().BeApproximately(expectedBearing, 0.005);
        }

    }
}
