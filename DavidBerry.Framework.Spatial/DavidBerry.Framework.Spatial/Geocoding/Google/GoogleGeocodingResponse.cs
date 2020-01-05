using System;
using Newtonsoft.Json;

namespace DavidBerry.Framework.Spatial.Geocoding.Google
{

    public class GoogleGeocodingResponse
    {
        [JsonProperty("results")]
        public GeocodingResult[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }


        #region Inner Classes

        /// <summary>
        /// Represents a result provided by the Google Geocoder
        /// </summary>
        public class GeocodingResult
        {

            [JsonProperty("address_components")]
            public AddressComponent[] AddressComponents { get; set; }

            [JsonProperty("formatted_address")]
            public string FormattedAddress { get; set; }

            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }

            [JsonProperty("place_id")]
            public string GoogleMapsPlaceId { get; set; }

            [JsonProperty("types")]
            public string[] ResultTypes { get; set; }
        }

        public class AddressComponent
        {
            [JsonProperty("long_name")]
            public string LongValue { get; set; }

            [JsonProperty("short_name")]
            public string ShortValue { get; set; }

            [JsonProperty("types")]
            public string[] ValueTypes { get; set; }
        }

        /// <summary>
        /// Represents the geometry (coordinates) for a geocoding result
        /// </summary>
        public class Geometry
        {
            [JsonProperty("location")]
            public Point Location { get; set; }

            [JsonProperty("location_type")]
            public string LocationType { get; set; }

            [JsonProperty("viewport")]
            public Viewport Viewport { get; set; }
        }

        /// <summary>
        /// Represents a coordinate (latitude/longitude pair) in a geometry
        /// </summary>
        public class Point
        {
            [JsonProperty("lat")]
            public double Latitude { get; set; }

            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }

        /// <summary>
        /// Represents a box around the result sufficient to get a view of the result
        /// </summary>
        public class Viewport
        {
            [JsonProperty("northeast")]
            public Point NortheastPoint { get; set; }

            [JsonProperty("southwest")]
            public Point SouthwestPoint { get; set; }
        }


        #endregion


    }

}














