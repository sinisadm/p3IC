using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dropbox.Application.Common
{
    public static class QueryableExtensions
    {
        private const int _maxTakeLimit = 500;

        public static IQueryable<TSource> WithQueryOptions<TSource, TKey>(this IQueryable<TSource> queryable, QueryOptions queryOptions, Expression<Func<TSource, TKey>> defaultSort, bool defaultSortDesc = false)
        {
            if (queryOptions == null)
            {
                return queryable;
            }

            var sort = queryOptions.Sort;

            IOrderedQueryable<TSource> res;

            if (string.IsNullOrWhiteSpace(sort))
            {
                res = defaultSortDesc ? queryable.OrderByDescending(defaultSort) : queryable.OrderBy(defaultSort);
            }
            else
            {
                var sortBy = sort.Split(',');
                var first = sortBy.First();
                res = first.StartsWith("-") ? queryable.OrderByDescending(first.TrimStart('-')) : queryable.OrderBy(first.TrimStart('+'));

                foreach (var item in sortBy.Skip(1))
                {
                    res = item.StartsWith("-") ? res.ThenByDescending(item.TrimStart('-')) : res.ThenBy(item.TrimStart('+'));
                }
            }

            int skip = ((queryOptions.Page < 1 ? 1 : queryOptions.Page) - 1) * (queryOptions.Take ?? _maxTakeLimit);
            return res.Skip(skip).Take(queryOptions.Take ?? _maxTakeLimit);
        }


        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
        }

        /// <summary>
        /// Builds the Queryable functions using a TSource property name.
        /// </summary>
        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName,
                IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }
    }
}
