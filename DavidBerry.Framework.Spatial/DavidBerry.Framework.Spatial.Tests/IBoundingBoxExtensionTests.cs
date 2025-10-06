using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Spatial.Tests
{
    public class IBoundingBoxExtensionTests
    {



        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 50.072097, 1.8031111)]
        [InlineData(48.1798839, -1.954995, 50.072097, 1.8031111, 50.072097, 1.8031111)]
        [InlineData(13.7994072, 145.112915, 13.1022175, 144.4647218, 13.7994072, 145.112915)]
        [InlineData(-31.6244855, 116.239023, -32.4556424, 115.6840483, -31.6244855, 116.239023)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -30.0852149, -53.0779284)]
        public void ValidateNortheastExtensionMethod(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double expectedLatitude, double expectedLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var northeastPoint = box.Northeast();

            northeastPoint.Latitude.Value.ShouldBe(expectedLatitude, 0.0001);
            northeastPoint.Longitude.Value.ShouldBe(expectedLongitude, 0.0001);
        }


        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 50.072097, -1.954995)]
        [InlineData(48.1798839, -1.954995, 50.072097, 1.8031111, 50.072097, -1.954995)]
        [InlineData(13.7994072, 145.112915, 13.1022175, 144.4647218, 13.7994072, 144.4647218)]
        [InlineData(-31.6244855, 116.239023, -32.4556424, 115.6840483, -31.6244855, 115.6840483)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -30.0852149, -58.49136094)]
        public void ValidateNorthwestExtensionMethod(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double expectedLatitude, double expectedLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var northwestPoint = box.Northwest();

            northwestPoint.Latitude.Value.ShouldBe(expectedLatitude, 0.0001);
            northwestPoint.Longitude.Value.ShouldBe(expectedLongitude, 0.0001);
        }


        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 48.1798839, -1.954995)]
        [InlineData(48.1798839, -1.954995, 50.072097, 1.8031111, 48.1798839, -1.954995)]
        [InlineData(13.7994072, 145.112915, 13.1022175, 144.4647218, 13.1022175, 144.4647218)]
        [InlineData(-31.6244855, 116.239023, -32.4556424, 115.6840483, -32.4556424, 115.6840483)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -35.1558001, -58.49136094)]
        public void ValidateSouthwestExtensionMethod(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double expectedLatitude, double expectedLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var southwestPoint = box.Southwest();

            southwestPoint.Latitude.Value.ShouldBe(expectedLatitude, 0.0001);
            southwestPoint.Longitude.Value.ShouldBe(expectedLongitude, 0.0001);
        }



        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 48.1798839, 1.8031111)]
        [InlineData(48.1798839, -1.954995, 50.072097, 1.8031111, 48.1798839, 1.8031111)]
        [InlineData(13.7994072, 145.112915, 13.1022175, 144.4647218, 13.1022175, 145.112915)]
        [InlineData(-31.6244855, 116.239023, -32.4556424, 115.6840483, -32.4556424, 116.239023)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -35.1558001, -53.0779284)]
        public void ValidateSoutheastExtensionMethod(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double expectedLatitude, double expectedLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var southeastPoint = box.Southeast();

            southeastPoint.Latitude.Value.ShouldBe(expectedLatitude, 0.0001);
            southeastPoint.Longitude.Value.ShouldBe(expectedLongitude, 0.0001);
        }




        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 49.186818, -0.362724)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -34.760174, -56.193534)]

        public void ContainsKeyReturnsTrueForPointInBoundingBox(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double testPointLatitude, double testPointLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var contains = box.ContainsPoint(testPointLatitude, testPointLongitude);

            contains.ShouldBeTrue();
        }


        [Theory]
        [InlineData(50.072097, 1.8031111, 48.1798839, -1.954995, 48.860004, 2.294274)]
        [InlineData(-30.0852149, -53.0779284, -35.1558001, -58.4913609, -32.959023, -60.623749)]
        public void ContainsKeyReturnsFalseForPointOutsideBoundingBox(double pointOneLatitude, double pointOneLongitude,
            double pointTwoLatitude, double pointTwoLongitude, double testPointLatitude, double testPointLongitude)
        {
            BoundingBox box = new BoundingBox(pointOneLatitude, pointOneLongitude, pointTwoLatitude, pointTwoLongitude);

            var contains = box.ContainsPoint(testPointLatitude, testPointLongitude);

            contains.ShouldBeFalse();
        }

    }
}
