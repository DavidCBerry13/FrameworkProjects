using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using DavidBerry.Framework.Util;
using DavidBerry.Framework.Spatial.Geocoding;
using DavidBerry.Framework.Spatial.Geocoding.Google;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using RestSharp;
using Moq;
using System.Threading;
using Shouldly.ShouldlyExtensionMethods;

namespace DavidBerry.Framework.Spatial.Tests.Geocoding.Google
{
    public class GoogleGeocoderTests
    {

        [Fact]
        public void GoogleResponseObjectMapsvaluesCorrectlyUsingJsonAttributes()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");

            var googleResponse = JsonConvert.DeserializeObject<GoogleGeocodingResponse>(json);

            googleResponse.Status.ShouldBe("OK");
            googleResponse.ErrorMessage.ShouldBeNull();
            googleResponse.Results.Count().ShouldBe(1);

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("street_number")).ShortValue.ShouldBe("1600");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("street_number")).LongValue.ShouldBe("1600");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("route")).ShortValue.ShouldBe("Amphitheatre Pkwy");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("route")).LongValue.ShouldBe("Amphitheatre Pkwy");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("locality")).ShortValue.ShouldBe("Mountain View");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("locality")).LongValue.ShouldBe("Mountain View");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("administrative_area_level_1")).ShortValue.ShouldBe("CA");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("administrative_area_level_1")).LongValue.ShouldBe("California");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("country")).ShortValue.ShouldBe("US");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("country")).LongValue.ShouldBe("United States");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("postal_code")).ShortValue.ShouldBe("94043");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("postal_code")).LongValue.ShouldBe("94043");

            googleResponse.Results[0].FormattedAddress.ShouldBe("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");

            googleResponse.Results[0].Geometry.Location.Latitude.ShouldBe(37.4224764);
            googleResponse.Results[0].Geometry.Location.Longitude.ShouldBe(-122.0842499);
            googleResponse.Results[0].Geometry.LocationType.ShouldBe("ROOFTOP");
            googleResponse.Results[0].Geometry.Viewport.NortheastPoint.Latitude.ShouldBe(37.4238253802915);
            googleResponse.Results[0].Geometry.Viewport.NortheastPoint.Longitude.ShouldBe(-122.0829009197085);
            googleResponse.Results[0].Geometry.Viewport.SouthwestPoint.Latitude.ShouldBe(37.4211274197085);
            googleResponse.Results[0].Geometry.Viewport.SouthwestPoint.Longitude.ShouldBe(-122.0855988802915);

            googleResponse.Results[0].GoogleMapsPlaceId.ShouldBe("ChIJ2eUgeAK6j4ARbn5u_wAGqWA");
        }


        [Fact]
        public void VerifyFormattedLocationStringIsMappedToGeocodingResponseObject()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.Value[0].FormattedAddress.ShouldBe("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");
        }

        [Fact]
        public void VerifyLocationLatitudeLongitudeMappedCorrectlyInReturnObject()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.Value[0].Location.Latitude.Value.ShouldBe(37.422476, 0.00001);
            result.Value[0].Location.Longitude.Value.ShouldBe(-122.0842499, 0.00001);
        }




        [Fact]
        public void VerifyFullCallResponseWhenGoogleReturnsSingleAddress()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);

            result.Value[0].FormattedAddress.ShouldBe("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");
            result.Value[0].Location.Latitude.Value.ShouldBe(37.422476, 0.00001);
            result.Value[0].Location.Longitude.Value.ShouldBe(-122.0842499, 0.00001);
        }


        [Fact]
        public void VerifyFailureResultReturnedWhenGoogleReturnsError()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.BillingDisabled.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.ShouldBeFalse();
            result.Error.Message.ShouldBe("Call to Google geocoding service failed");
            result.Value.ShouldBeNull();
        }


        [Fact]
        public void VerifySingleCitySearchDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.City-BoiseId.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);

            result.Value[0].FormattedAddress.ShouldBe("Boise, ID, USA");
            result.Value[0].Location.Latitude.Value.ShouldBe(43.6150186, 0.00001);
            result.Value[0].Location.Longitude.Value.ShouldBe(-116.2023137, 0.00001);

            result.Value[0].LocationType.ShouldBe(LocationType.CITY);

            result.Value[0].Viewport.PointOne.Latitude.Value.ShouldBe(43.6898951, 0.00001);
            result.Value[0].Viewport.PointOne.Longitude.Value.ShouldBe(-116.1019091, 0.00001);

            result.Value[0].Viewport.PointTwo.Latitude.Value.ShouldBe(43.511717, 0.00001);
            result.Value[0].Viewport.PointTwo.Longitude.Value.ShouldBe(-116.3658869, 0.00001);
        }


        [Fact]
        public void VerifyCitySearchWithMultipleResultsDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.MultipleCity-Springfield.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(3);

            // Springfield, MO
            result.Value[0].FormattedAddress.ShouldBe("Springfield, MO, USA");
            result.Value[0].Location.Latitude.Value.ShouldBe(37.2089572, 0.00001);
            result.Value[0].Location.Longitude.Value.ShouldBe(-93.29229889999999, 0.00001);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.CITY);

            // Springfield, IL
            result.Value[1].FormattedAddress.ShouldBe("Springfield, IL, USA");
            result.Value[1].Location.Latitude.Value.ShouldBe(39.781721300, 0.00001);
            result.Value[1].Location.Longitude.Value.ShouldBe(-89.6501481, 0.00001);
            result.Value[1].LocationType.ShouldHaveFlag(LocationType.CITY);

            // Springfield, OH
            result.Value[2].FormattedAddress.ShouldBe("Springfield, OH, USA");
            result.Value[2].Location.Latitude.Value.ShouldBe(39.9242266, 0.00001);
            result.Value[2].Location.Longitude.Value.ShouldBe(-83.80881711, 0.00001);
            result.Value[2].LocationType.ShouldHaveFlag(LocationType.CITY);
        }



        [Fact]
        public void VerifyUsZipCodeSearchDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.UsZipCode-83702.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);

            result.Value[0].FormattedAddress.ShouldBe("Boise, ID 83702, USA");
            result.Value[0].Location.Latitude.Value.ShouldBe(43.6624385, 0.00001);
            result.Value[0].Location.Longitude.Value.ShouldBe(-116.1630431, 0.00001);

            result.Value[0].LocationType.ShouldBe(LocationType.POSTAL_CODE);

            result.Value[0].Viewport.PointOne.Latitude.Value.ShouldBe(43.7000229, 0.00001);
            result.Value[0].Viewport.PointOne.Longitude.Value.ShouldBe(-116.0941219, 0.00001);

            result.Value[0].Viewport.PointTwo.Latitude.Value.ShouldBe(43.6033878, 0.00001);
            result.Value[0].Viewport.PointTwo.Longitude.Value.ShouldBe(-116.2351669, 0.00001);
        }


        [Fact]
        public void VerifyCorrectLocationTypesAttachedToGasStationResult()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GasStation-KwikTrip.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.ShouldBe(1);

            // This is a convenience store, so all three of these should light up
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.BUSINESS);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.GAS_STATION);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.STORE);
            result.Value[0].LocationType.ShouldNotHaveFlag(LocationType.POINT_OF_INTEREST);  // POI is assinged to everything in Google, so it should be screened out
        }


        [Fact]
        public void VerifyCorrectLocationTypesAttachedToMuseumResult()
        {
            // Setup the response
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.Museum-FieldMuseum.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.ShouldBe(1);

            result.Value[0].LocationType.ShouldHaveFlag(LocationType.BUSINESS);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.MUSEUM);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.POINT_OF_INTEREST);  // Based on the Tourist Attraction flag in Google
        }

        [Fact]
        public void VerifyCorrectAirportLocationTypeAttachedToAirport()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.Airport-OHare.json");
            RestResponse restResponse = new RestResponse();
            restResponse.ResponseStatus = ResponseStatus.Completed;
            restResponse.StatusCode = System.Net.HttpStatusCode.OK;
            restResponse.Content = json;

            // Mock the IRestClient to return the response.  The ExecuteAsync() method is the one real method that you need to mock
            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<RestRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.ShouldBe(1);
            result.Value[0].LocationType.ShouldHaveFlag(LocationType.AIRPORT);
        }


    }
}
