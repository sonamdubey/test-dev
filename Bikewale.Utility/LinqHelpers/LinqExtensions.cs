using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Utility.LinqHelpers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 feb 2017
    /// Description :   Linq Extensions methods
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Page extension method
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        /// <summary>
        /// Created By:- Subodh Jain 17 Feb 2017
        /// Summary :- Process orderby filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool asc = true)
        {
            return asc ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }
    }
}
