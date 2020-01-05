using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using DavidBerry.Framework.Util;
using DavidBerry.Framework.Spatial.Geocoding;
using DavidBerry.Framework.Spatial.Geocoding.Google;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using RestSharp;
using Moq;

namespace DavidBerry.Framework.Spatial.Tests.Geocoding.Google
{
    public class GoogleGeocoderTests
    {

        [Fact]
        public void GoogleResponseObjectMapsvaluesCorrectlyUsingJsonAttributes()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");

            var googleResponse = JsonConvert.DeserializeObject<GoogleGeocodingResponse>(json);

            googleResponse.Status.Should().Be("OK");
            googleResponse.ErrorMessage.Should().BeNull();
            googleResponse.Results.Should().HaveCount(1);

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("street_number")).ShortValue.Should().Be("1600");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("street_number")).LongValue.Should().Be("1600");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("route")).ShortValue.Should().Be("Amphitheatre Pkwy");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("route")).LongValue.Should().Be("Amphitheatre Pkwy");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("locality")).ShortValue.Should().Be("Mountain View");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("locality")).LongValue.Should().Be("Mountain View");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("administrative_area_level_1")).ShortValue.Should().Be("CA");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("administrative_area_level_1")).LongValue.Should().Be("California");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("country")).ShortValue.Should().Be("US");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("country")).LongValue.Should().Be("United States");

            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("postal_code")).ShortValue.Should().Be("94043");
            googleResponse.Results[0].AddressComponents.First(c => c.ValueTypes.Contains("postal_code")).LongValue.Should().Be("94043");

            googleResponse.Results[0].FormattedAddress.Should().Be("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");

            googleResponse.Results[0].Geometry.Location.Latitude.Should().Be(37.4224764);
            googleResponse.Results[0].Geometry.Location.Longitude.Should().Be(-122.0842499);
            googleResponse.Results[0].Geometry.LocationType.Should().Be("ROOFTOP");
            googleResponse.Results[0].Geometry.Viewport.NortheastPoint.Latitude.Should().Be(37.4238253802915);
            googleResponse.Results[0].Geometry.Viewport.NortheastPoint.Longitude.Should().Be(-122.0829009197085);
            googleResponse.Results[0].Geometry.Viewport.SouthwestPoint.Latitude.Should().Be(37.4211274197085);
            googleResponse.Results[0].Geometry.Viewport.SouthwestPoint.Longitude.Should().Be(-122.0855988802915);

            googleResponse.Results[0].GoogleMapsPlaceId.Should().Be("ChIJ2eUgeAK6j4ARbn5u_wAGqWA");
        }


        [Fact]
        public void VerifyFormattedLocationStringIsMappedToGeocodingResponseObject()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.Value[0].FormattedAddress.Should().Be("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");
        }

        [Fact]
        public void VerifyLocationLatitudeLongitudeMappedCorrectlyInReturnObject()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.Value[0].Location.Latitude.Value.Should().BeApproximately(37.422476, 0.00001);
            result.Value[0].Location.Longitude.Value.Should().BeApproximately(-122.0842499, 0.00001);
        }




        [Fact]
        public void VerifyFullCallResponseWhenGoogleReturnsSingleAddress()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GoogleHeadquarters.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(1);

            result.Value[0].FormattedAddress.Should().Be("1600 Amphitheatre Parkway, Mountain View, CA 94043, USA");
            result.Value[0].Location.Latitude.Value.Should().BeApproximately(37.422476, 0.00001);
            result.Value[0].Location.Longitude.Value.Should().BeApproximately(-122.0842499, 0.00001);
        }


        [Fact]
        public void VerifyFailureResultReturnedWhenGoogleReturnsError()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.BillingDisabled.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.Should().BeFalse();
            result.Error.Message.Should().Be("Call to Google geocoding service failed");
            result.Value.Should().BeNull();
        }


        [Fact]
        public void VerifySingleCitySearchDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.City-BoiseId.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(1);

            result.Value[0].FormattedAddress.Should().Be("Boise, ID, USA");
            result.Value[0].Location.Latitude.Value.Should().BeApproximately(43.6150186, 0.00001);
            result.Value[0].Location.Longitude.Value.Should().BeApproximately(-116.2023137, 0.00001);

            result.Value[0].LocationType.Should().Be(LocationType.CITY);

            result.Value[0].Viewport.PointOne.Latitude.Value.Should().BeApproximately(43.6898951, 0.00001);
            result.Value[0].Viewport.PointOne.Longitude.Value.Should().BeApproximately(-116.1019091, 0.00001);

            result.Value[0].Viewport.PointTwo.Latitude.Value.Should().BeApproximately(43.511717, 0.00001);
            result.Value[0].Viewport.PointTwo.Longitude.Value.Should().BeApproximately(-116.3658869, 0.00001);
        }


        [Fact]
        public void VerifyCitySearchWithMultipleResultsDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.MultipleCity-Springfield.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(3);

            // Springfield, MO
            result.Value[0].FormattedAddress.Should().Be("Springfield, MO, USA");
            result.Value[0].Location.Latitude.Value.Should().BeApproximately(37.2089572, 0.00001);
            result.Value[0].Location.Longitude.Value.Should().BeApproximately(-93.29229889999999, 0.00001);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.CITY);

            // Springfield, IL
            result.Value[1].FormattedAddress.Should().Be("Springfield, IL, USA");
            result.Value[1].Location.Latitude.Value.Should().BeApproximately(39.781721300, 0.00001);
            result.Value[1].Location.Longitude.Value.Should().BeApproximately(-89.6501481, 0.00001);
            result.Value[1].LocationType.Should().HaveFlag(LocationType.CITY);

            // Springfield, OH
            result.Value[2].FormattedAddress.Should().Be("Springfield, OH, USA");
            result.Value[2].Location.Latitude.Value.Should().BeApproximately(39.9242266, 0.00001);
            result.Value[2].Location.Longitude.Value.Should().BeApproximately(-83.80881711, 0.00001);
            result.Value[2].LocationType.Should().HaveFlag(LocationType.CITY);
        }



        [Fact]
        public void VerifyUsZipCodeSearchDecodesCorrectly()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.UsZipCode-83702.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(1);

            result.Value[0].FormattedAddress.Should().Be("Boise, ID 83702, USA");
            result.Value[0].Location.Latitude.Value.Should().BeApproximately(43.6624385, 0.00001);
            result.Value[0].Location.Longitude.Value.Should().BeApproximately(-116.1630431, 0.00001);

            result.Value[0].LocationType.Should().Be(LocationType.POSTAL_CODE);

            result.Value[0].Viewport.PointOne.Latitude.Value.Should().BeApproximately(43.7000229, 0.00001);
            result.Value[0].Viewport.PointOne.Longitude.Value.Should().BeApproximately(-116.0941219, 0.00001);

            result.Value[0].Viewport.PointTwo.Latitude.Value.Should().BeApproximately(43.6033878, 0.00001);
            result.Value[0].Viewport.PointTwo.Longitude.Value.Should().BeApproximately(-116.2351669, 0.00001);
        }


        [Fact]
        public void VerifyCorrectLocationTypesAttachedToGasStationResult()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.GasStation-KwikTrip.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.Should().Be(1);

            // This is a convenience store, so all three of these should light up
            result.Value[0].LocationType.Should().HaveFlag(LocationType.BUSINESS);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.GAS_STATION);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.STORE);
            result.Value[0].LocationType.Should().NotHaveFlag(LocationType.POINT_OF_INTEREST);  // POI is assinged ot everything in Google, so it should screen out
        }


        [Fact]
        public void VerifyCorrectLocationTypesAttachedToMuseumResult()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.Museum-FieldMuseum.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.Should().Be(1);

            result.Value[0].LocationType.Should().HaveFlag(LocationType.BUSINESS);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.MUSEUM);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.POINT_OF_INTEREST);  // Based on the Tourist Attraction flag in Google
        }

        [Fact]
        public void VerifyCorrectAirportLocationTypeAttachedToAirport()
        {
            String json = Assembly.GetExecutingAssembly().ReadEmbeddedResourceTextFile("Geocoding.Google.GoogleResponse.Airport-OHare.json");
            Mock<IRestResponse> restResponse = new Mock<IRestResponse>();
            restResponse.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            restResponse.Setup(r => r.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            restResponse.Setup(r => r.Content).Returns(json);

            Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>()))
                .Returns(restResponse.Object);

            GoogleGeocodingService service = new GoogleGeocodingService("test", mockRestClient.Object);
            var result = service.GeocodeAddress(It.IsAny<string>());

            // Assert
            result.Value.Count.Should().Be(1);
            result.Value[0].LocationType.Should().HaveFlag(LocationType.AIRPORT);
        }


    }
}
