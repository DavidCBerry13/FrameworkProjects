using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Util
{
    public static class CollectionExtensions
    {

        /// <summary>
        /// Convenience extension method to check if an ICollection is null or empty (i.e. has any items)
        /// </summary>
        /// <typeparam name="T">The generic type parameter.  This is required for the extension method definition so the method can work on any ICollection of type T</typeparam>
        /// <param name="collection">An ICollection to check</param>
        /// <returns>True if the collection is null or has zero elements.  False otherwise</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null || collection.Count == 0);
        }


    }
}
