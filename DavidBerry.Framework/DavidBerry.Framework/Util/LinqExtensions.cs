using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DavidBerry.Framework.Util
{
    public static class LinqExtensions
    {

        /// <summary>
        /// Gets the elements that appear in the source collection but do not appear in the target collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNotExists<T, K>(this IEnumerable<T> source, IEnumerable<K> target, Func<T, K, bool> comparer)
        {
            return source.Where(s => !target.Any(t => comparer(s, t)));
        }


        public static IEnumerable<T> WhereExists<T, K>(this IEnumerable<T> source, IEnumerable<K> target, Func<T, K, bool> comparer)
        {
            return source.Where(s => target.Any(t => comparer(s, t)));
        }

    }
}
