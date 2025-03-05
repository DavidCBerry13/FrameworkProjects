using DavidBerry.Framework.Functional;
using DavidBerry.Framework.Spatial.Geocoding;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DavidBerry.Framework.Spatial.Geocoding.Google
{
    public class GoogleGeocodingService : IGeocodingService
    {


        public GoogleGeocodingService(string apiKey)
            : this(apiKey, new RestClient("https://maps.googleapis.com"))
        {

        }

        public GoogleGeocodingService(string apiKey, IRestClient restClient)
        {
            _apiKey = apiKey;
            _restClient = restClient;
        }


        private readonly IRestClient _restClient;
        private readonly string _apiKey;

        private readonly static Dictionary<string, LocationType> LocationTypeMap = new Dictionary<string, LocationType>()
        {
            { "point_of_interest", LocationType.NONE },   // POI is attached to everything in Google, so basically throw it out
            { "street_address", LocationType.STREET_ADDRESS },
            { "route", LocationType.ROAD },
            { "intersection", LocationType.INTERSECTION },
            { "country", LocationType.COUNTRY },
            { "administrative_area_level_1", LocationType.STATE_PROVINCE },
            { "administrative_area_level_2", LocationType.COUNTY },
            { "locality", LocationType.CITY },
            { "sublocality", LocationType.NEIGHBORHOOD },
            { "neighborhood", LocationType.NEIGHBORHOOD },
            { "premise", LocationType.BUILDING },
            { "subpremise", LocationType.BUILDING },
            { "postal_code", LocationType.POSTAL_CODE },
            { "airport", LocationType.BUSINESS | LocationType.AIRPORT },
            { "tourist_attraction", LocationType.POINT_OF_INTEREST },
            { "park", LocationType.PARK },
            { "store", LocationType.BUSINESS | LocationType.STORE },
            { "restaurant", LocationType.BUSINESS | LocationType.RESTAURANT },
            { "supermarket", LocationType.BUSINESS | LocationType.SUPERMARKET },
            { "local_government_office", LocationType.BUSINESS | LocationType.GOVERNMENT_OFFICE },
            { "post_office", LocationType.BUSINESS | LocationType.POST_OFFICE },
            { "museum", LocationType.BUSINESS | LocationType.MUSEUM },
            { "gas_station", LocationType.BUSINESS | LocationType.GAS_STATION }
        };


        public Result<List<GeocodingResult>> GeocodeAddress(string address)
        {
            var request = new RestRequest("maps/api/geocode/json", Method.GET);
            request.AddParameter("key", _apiKey);
            request.AddParameter("query", address);

            IRestResponse response = _restClient.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                var googleResponse = JsonConvert.DeserializeObject<GoogleGeocodingResponse>(response.Content);
                return (googleResponse.Status == "OK") ?
                    Result.Success<List<GeocodingResult>>(MapGoogleResponse(googleResponse)) :
                    Result.Failure<List<GeocodingResult>>("Call to Google geocoding service failed");
            }
            else
            {
                return Result.Failure<List<GeocodingResult>>("Call to Google geocoding service failed");
            }                         
        }


        internal static List<GeocodingResult> MapGoogleResponse(GoogleGeocodingResponse googleResponse)
        {
            return googleResponse.Results.Select(r => 
                new GeocodingResult()
                {
                    Location = new GeoCoordinate(r.Geometry.Location.Latitude, r.Geometry.Location.Longitude ),
                    FormattedAddress = r.FormattedAddress,
                    Viewport = new BoundingBox(r.Geometry.Viewport.NortheastPoint.Latitude, r.Geometry.Viewport.NortheastPoint.Longitude,
                        r.Geometry.Viewport.SouthwestPoint.Latitude, r.Geometry.Viewport.SouthwestPoint.Longitude),
                    LocationType = DecodeLocationType(r)
                }
            ).ToList();
        }


        internal static LocationType DecodeLocationType(GoogleGeocodingResponse.GeocodingResult result)
        {
            var locationType = result.ResultTypes
                .Select(x => LocationTypeMap.ContainsKey(x) ? LocationTypeMap[x] : LocationType.NONE)
                .Aggregate(LocationType.NONE, (acc, x) => acc | x);

            return locationType;
        }


    }
}
