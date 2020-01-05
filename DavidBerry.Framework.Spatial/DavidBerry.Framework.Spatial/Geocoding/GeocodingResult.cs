using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Spatial.Geocoding
{
    public class GeocodingResult
    {

        
        public string FormattedAddress { get; set; }

        public IGeoCoordinate Location { get; set; }

        public LocationType LocationType { get; set; }

        public IBoundingBox Viewport { get; set; }



    }
}
