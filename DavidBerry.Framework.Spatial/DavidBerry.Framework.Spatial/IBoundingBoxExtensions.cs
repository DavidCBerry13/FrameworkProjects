using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Spatial
{
    public static class IBoundingBoxExtensions
    {



        public static IGeoCoordinate Northwest(this IBoundingBox boundingBox)
        {
            var latitude = Math.Max(boundingBox.PointOne.Latitude.Value, boundingBox.PointTwo.Latitude.Value);
            var longitude = Math.Min(boundingBox.PointOne.Longitude.Value, boundingBox.PointTwo.Longitude.Value);
            return new GeoCoordinate(latitude, longitude);
        }

        public static IGeoCoordinate Northeast(this IBoundingBox boundingBox)
        {
            var latitude = Math.Max(boundingBox.PointOne.Latitude.Value, boundingBox.PointTwo.Latitude.Value);
            var longitude = Math.Max(boundingBox.PointOne.Longitude.Value, boundingBox.PointTwo.Longitude.Value);
            return new GeoCoordinate(latitude, longitude);
        }


        public static IGeoCoordinate Southwest(this IBoundingBox boundingBox)
        {
            var latitude = Math.Min(boundingBox.PointOne.Latitude.Value, boundingBox.PointTwo.Latitude.Value);
            var longitude = Math.Min(boundingBox.PointOne.Longitude.Value, boundingBox.PointTwo.Longitude.Value);
            return new GeoCoordinate(latitude, longitude);
        }

        public static IGeoCoordinate Southeast(this IBoundingBox boundingBox)
        {
            var latitude = Math.Min(boundingBox.PointOne.Latitude.Value, boundingBox.PointTwo.Latitude.Value);
            var longitude = Math.Max(boundingBox.PointOne.Longitude.Value, boundingBox.PointTwo.Longitude.Value);
            return new GeoCoordinate(latitude, longitude);
        }



        public static bool ContainsPoint(this IBoundingBox boundingBox, IGeoCoordinate coordinate)
        {
            return
                coordinate.Latitude.Value <= boundingBox.Northeast().Latitude.Value
                && coordinate.Longitude.Value <= boundingBox.Northeast().Longitude.Value
                && coordinate.Latitude.Value >= boundingBox.Southwest().Latitude.Value
                && coordinate.Longitude.Value >= boundingBox.Southwest().Longitude.Value;
        }


        public static bool ContainsPoint(this IBoundingBox boundingBox, double latitude, double longitude)
        {
            return boundingBox.ContainsPoint(new GeoCoordinate(latitude, longitude));
        }

    }
}
