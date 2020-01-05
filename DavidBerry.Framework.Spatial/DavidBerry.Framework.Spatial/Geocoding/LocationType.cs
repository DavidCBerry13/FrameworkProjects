using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Spatial.Geocoding
{


    // https://developers.google.com/maps/documentation/geocoding/intro#Types

    /// <summary>
    /// Summarizes the types of locations that can be returned by a Geocoder
    /// </summary>
    /// <remarks>
    /// Geocoders can process more than just street addresses and as such, can return different
    /// types of data depending on what location string is passed to them.  For example, it is perfectly
    /// valid to pass the string "Chicago, IL" to a geocoder and you will get back a result, most likely a
    /// point in the central business district and a viewport/bounding box that will encompassas the city. 
    /// In the same way, one could also enter 'California', '11201' (US Zip Code), 'Belgium' or 'Eifel Tower'
    /// and get valid results back.
    /// <para>
    /// What is important to business logic code is the ability to interpret the results it gets back.  Certain logic
    /// may require a location that is a street address or premise address but not a city, state or country.  Having these
    /// standardized types attached to the returned result helps this code determine what the geocoder returned and
    /// if it is suitable for the purposes that code needs.
    /// </para>
    /// </remarks>
    [Flags]
    public enum LocationType
    {
        NONE = 0,
        STREET_ADDRESS = 1,
        ROAD = 2,
        INTERSECTION = 4,
        COUNTRY = 8,
        STATE_PROVINCE = 16,
        COUNTY = 32,
        CITY = 64,
        NEIGHBORHOOD = 128,
        POSTAL_CODE = 256,
        BUILDING = 512,
        POINT_OF_INTEREST = 1024,
        PARK = 2048,
        AIRPORT = 4096,
        MUSEUM = 8092,
        BUSINESS = 16_384,
        RESTAURANT = 32_768,
        STORE = 65_536,
        HOSPITAL = 131_072,
        BANK = 262_144,
        GAS_STATION = 524_288,
        SUPERMARKET = 1_048_576,
        GOVERNMENT_OFFICE = 2_097_152,
        POST_OFFICE = 4_194_304,
        LIBRARY = 8_388_608,
        SCHOOL = 16_777_216,





        //private LocationType(string code, string name)
        //{
        //    Code = code;
        //    Name = name;
        //}

        //public string Code { get; private set; }

        //public string Name { get; private set; }



        //public static readonly LocationType STREET_ADDRESS = new LocationType("ADDR", "Street Address");

        //public static readonly LocationType ROAD = new LocationType("ROAD", "Street/Road");

        //public static readonly LocationType INTERSECTION = new LocationType("INTR", "Intersection");

        //public static readonly LocationType BUILDING = new LocationType("BLDG", "Building");

        //public static readonly LocationType PARK = new LocationType("PARK", "Park");

        //public static readonly LocationType POINT_OF_INTEREST = new LocationType("POI", "Point of Interest");

        //public static readonly LocationType AIRPORT = new LocationType("AIRP", "Airport");

        //public static readonly LocationType POSTAL_CODE = new LocationType("POST", "Postal Code");

        //public static readonly LocationType STATE_PROVINCE = new LocationType("PROV", "State/Province");

        //public static readonly LocationType COUNTY = new LocationType("CNTY", "County");

        //public static readonly LocationType CITY = new LocationType("CITY", "City");

        //public static readonly LocationType NEIGHBORHOOD = new LocationType("NBRH", "Neighborhood");

        //public static readonly LocationType COUNTRY = new LocationType("NATN", "Country");

        //public static readonly LocationType OTHER = new LocationType("OTHER", "Other");


    }
}
