using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Spatial
{

    /// <summary>
    /// Represents a rectangular area bounded by the given top left and bottom right points
    /// </summary>
    public class BoundingBox : IBoundingBox
    {
        public BoundingBox(IGeoCoordinate pointOne, IGeoCoordinate pointTwo)
        {
            if (pointOne.Latitude.Value == pointTwo.Latitude.Value || pointOne.Longitude.Value == pointTwo.Longitude.Value)
                throw new ArgumentException("The latitudes and longitudes onf the two points must be different");

            PointOne = pointOne;
            PointTwo = pointTwo;
        }


        public BoundingBox(double pointOneLatitude, double pointOneLongitude, 
            double pointTwoLatitude, double pointTwoLongitude)
            : this (new GeoCoordinate(pointOneLatitude, pointOneLongitude), new GeoCoordinate(pointTwoLatitude, pointTwoLongitude))
        {

        }


        public IGeoCoordinate PointOne { get; set; }
        public IGeoCoordinate PointTwo { get; set; }


    }
}
