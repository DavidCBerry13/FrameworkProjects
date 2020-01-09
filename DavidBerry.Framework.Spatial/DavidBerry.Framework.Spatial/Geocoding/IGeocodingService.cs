using DavidBerry.Framework.Functional;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Spatial.Geocoding
{
    public interface IGeocodingService
    {



        Result<List<GeocodingResult>> GeocodeAddress(string address);



    }
}
