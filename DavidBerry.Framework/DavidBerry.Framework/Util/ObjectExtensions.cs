using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Util;

public static class ObjectExtensions
{


    /// <summary>
    /// Convenience method to perform an "as" cast on an object to the specified type
    /// </summary>
    /// <typeparam name="T">The Type to as cast the object to</typeparam>
    /// <param name="obj">The object to be casted</param>
    /// <returns>The obejct as type T or null if the object cannot be casted to the specified type</returns>
    public static T? As<T>(this object obj) where T : class
    {
        return obj as T;
    }


}

