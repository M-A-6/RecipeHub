using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHub.Business
{
    public static class Extensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
