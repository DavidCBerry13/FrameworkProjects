using System;
using System.Collections.Generic;
using System.Text;
using UnitsNet;

namespace DavidBerry.Framework.Spatial
{


    /// <summary>
    /// Defines an interface for a geographic point
    /// </summary>
    /// <remarks>
    /// This interface is defined so that anything can implement the Interface and be a point.  
    /// </remarks>
    public interface IGeoCoordinate
    {

        Angle Latitude { get; }

        Angle Longitude { get; }

    }
}
