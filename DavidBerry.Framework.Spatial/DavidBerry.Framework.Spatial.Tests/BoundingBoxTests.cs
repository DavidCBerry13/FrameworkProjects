using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Spatial.Tests
{
    public class BoundingBoxTests
    {

        [Theory]
        [InlineData(41.93301, -87.68285, 41.91078, -87.62007)]  // Lincoln Park Chicago
        [InlineData(50.07209, -1.95499, 48.17988, 1.80311)]     // Normandy France
        public void ConsturctorTakesValidPoints(double topLeftLatitude, double topLeftLongitude,
            double bottomRightLatitude, double bottomRightLongitude)
        {
            BoundingBox box = new BoundingBox(topLeftLatitude, topLeftLongitude,
                bottomRightLatitude, bottomRightLongitude);

            box.PointOne.Latitude.Value.ShouldBe(topLeftLatitude);
            box.PointOne.Longitude.Value.ShouldBe(topLeftLongitude);
            box.PointTwo.Latitude.Value.ShouldBe(bottomRightLatitude);
            box.PointTwo.Longitude.Value.ShouldBe(bottomRightLongitude);

        }

        [Fact]
        public void ConsturctorRejectsPointsThatAreTheSame()
        {
            var exception = Assert.Throws<ArgumentException>(() => new BoundingBox(41.93301, -87.62007, 41.93301, -87.62007));
        }

    }
}
