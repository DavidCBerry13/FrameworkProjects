using System;
using Xunit;
using FluentAssertions;
using UnitsNet.Units;
using DavidBerry.Framework.Spatial;

namespace DavidBerry.Framework.Spatial.Tests
{
    public class GeoCoordinateTests
    {
        [Theory]
        [InlineData(41.866738, -87.616977)]   // Field Museum Chicago
        [InlineData(35.025678, 135.762184)]   // Kyoto Imperial Palace
        [InlineData(40.689311, -74.044503)]   // Statue of Liberty
        [InlineData(-22.951807, -43.210462)]  // Christ the Redeemer Statue
        [InlineData(0, 0)]   // Test the limits - somewhere in the Atlantic south of Ghana
        [InlineData(0, 180)]   // Test the limits - West of Howland Island
        [InlineData(0, -180)]   // Test the limits - West of Howland Island
        [InlineData(90, 0)]   // Test the limits - North Pole
        [InlineData(-90, 0)]   // Test the limits - South Pole
        public void CanConstructValidPointWithDegrees(double latitude, double longitude)
        {
            GeoCoordinate p = new GeoCoordinate(latitude, longitude);

            p.Should().NotBeNull();

            p.Latitude.Unit.Should().BeOfType<AngleUnit>();
            p.Latitude.Unit.Should().Be(AngleUnit.Degree);
            p.Latitude.Value.Should().Be(latitude);

            p.Longitude.Unit.Should().BeOfType<AngleUnit>();
            p.Longitude.Unit.Should().Be(AngleUnit.Degree);
            p.Longitude.Value.Should().Be(longitude);
        }


        [Theory]
        [InlineData(-90.0001, 0)]
        [InlineData(90.0001, 0)]
        [InlineData(0, -180.0001)]
        [InlineData(0, 180.0001)]
        public void LocationsWithInvalidDegreesCannotBeConstructed(double latitude, double longitude)
        {
            Assert.Throws<ArgumentException>(() => new GeoCoordinate(latitude, longitude));
        }





    }
}
