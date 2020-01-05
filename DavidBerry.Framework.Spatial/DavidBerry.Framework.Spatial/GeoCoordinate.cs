using System;
using UnitsNet;
using UnitsNet.Units;

namespace DavidBerry.Framework.Spatial
{
    public class GeoCoordinate : IGeoCoordinate
    {

        /// <summary>
        /// Create a new geographic point by specifying the latitude and longitude in decimal degrees
        /// </summary>
        /// <param name="latitude">The latitude of the location in decimal degrees</param>
        /// <param name="longitude">The longitude of the location in decimal degrees</param>
        public GeoCoordinate(double latitude, double longitude)
            : this(Angle.FromDegrees(latitude), Angle.FromDegrees(longitude))
        {

        }


        /// <summary>
        /// Create a new geographic point by specifying the latitude and longitude as Angle objects
        /// </summary>
        /// <param name="latitude">The latitude of the location as an Angle object</param>
        /// <param name="longitude">The longitude of the location as an Angle object</param>
        public GeoCoordinate(Angle latitude, Angle longitude)
        {
            var latitudeDegrees = latitude.ToUnit(AngleUnit.Degree);
            if (latitudeDegrees.Value < MINIMUM_LATITUDE || latitudeDegrees.Value > MAXIMUM_LATITUDE)
                throw new ArgumentException($"Latitude must be between {MINIMUM_LATITUDE} and {MAXIMUM_LATITUDE}");

            var longitudeDegrees = longitude.ToUnit(AngleUnit.Degree);
            if (longitudeDegrees.Value < MINIMUM_LONGITUDE || longitudeDegrees.Value > MAXIMUM_LONGITUDE)
                throw new ArgumentException("Longitude must be between {MINIMUM_LONGITUDE} and {MAXIMUM_LONGITUDE}");

            Latitude = latitudeDegrees;
            Longitude = longitudeDegrees;
        }


        public const double MINIMUM_LATITUDE = -90.0;

        public const double MAXIMUM_LATITUDE = 90.0;

        public const double MINIMUM_LONGITUDE = -180;

        public const double MAXIMUM_LONGITUDE = 180;



        public Angle Latitude { get; private set; }

        public Angle Longitude { get; private set; }


    }
}
