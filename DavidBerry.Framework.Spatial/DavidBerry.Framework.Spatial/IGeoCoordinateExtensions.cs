using DavidBerry.Framework.Spatial;
using System;
using System.Collections.Generic;
using System.Text;
using UnitsNet;
using UnitsNet.Units;

namespace DavidBerry.Framework.Spatial
{
    public static class IGeoCoordinateExtensions
    {

        public const double EARTH_RADIUS_METERS = 6_371_000;


        /// <summary>
        /// Returns the distance between two locations (points) in the distance units specified
        /// </summary>
        /// <param name="origin">The starting location (origin)</param>
        /// <param name="destination">The ending location (the destination)</param>
        /// <param name="unit">An optional LengthUnit value of the units the length should come back in.  If this parameter is not used then the returned length will be in meters</param>
        /// <returns>A Length object of the distance between the two locations</returns>
        public static Length HaversineDistance(this IGeoCoordinate origin, IGeoCoordinate destination, LengthUnit lengthUnit = LengthUnit.Meter)
        {
            var lat = (destination.Latitude - origin.Latitude).ToUnit(AngleUnit.Radian);
            var lng = (destination.Longitude - origin.Longitude).ToUnit(AngleUnit.Radian);
            var h1 = Math.Sin(lat.Value / 2) * Math.Sin(lat.Value / 2) +
                          Math.Cos(origin.Latitude.ToUnit(AngleUnit.Radian).Value) * Math.Cos(destination.Latitude.ToUnit(AngleUnit.Radian).Value) *
                          Math.Sin(lng.Value / 2) * Math.Sin(lng.Value / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            
            var distanceInMeters = Length.From(EARTH_RADIUS_METERS * h2, LengthUnit.Meter);
            return distanceInMeters.ToUnit(lengthUnit);
        }

        /// <summary>
        /// Returns the initial bearing to take to go from the first point (origin) to the second point (the destination)
        /// </summary>
        /// <remarks>
        /// Note this is an initial bearing.  The bearing of the course changes throughout the journey because you are traversing
        /// a great circle path over a sphere
        /// </remarks>
        /// <param name="origin">The starting location (origin)</param>
        /// <param name="destination">The ending location (the destination)</param>
        /// <returns>An Angle object with the initial bearing in the units of degrees</returns>
        public static Angle InitialBearing(this IGeoCoordinate origin, IGeoCoordinate destination)
        {
            var longitudeDelta = destination.Longitude.ToUnit(AngleUnit.Radian).Value - origin.Longitude.ToUnit(AngleUnit.Radian).Value;
            var y = Math.Sin(longitudeDelta) * Math.Cos(destination.Latitude.ToUnit(AngleUnit.Radian).Value);
            var x = Math.Cos(origin.Latitude.ToUnit(AngleUnit.Radian).Value) * Math.Sin(destination.Latitude.ToUnit(AngleUnit.Radian).Value) -
                    Math.Sin(origin.Latitude.ToUnit(AngleUnit.Radian).Value) * Math.Cos(destination.Latitude.ToUnit(AngleUnit.Radian).Value) * Math.Cos(longitudeDelta);
            var bearingInRadians = Math.Atan2(y, x);

            var degrees = (Angle.FromRadians(bearingInRadians).ToUnit(AngleUnit.Degree).Value + 360) % 360;

            return Angle.FromDegrees(degrees);
        }
        



    }
}
