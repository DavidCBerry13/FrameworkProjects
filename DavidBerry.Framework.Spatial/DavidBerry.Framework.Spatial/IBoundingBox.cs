using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Spatial
{
    public interface IBoundingBox
    {

        IGeoCoordinate PointOne { get; set; }

        IGeoCoordinate PointTwo { get; set; }


    }
}
