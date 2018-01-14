using System;
using System.Linq;
using System.Linq.Expressions;
using Omego.Extensions.Poco;

namespace Omego.Extensions.QueryableExtensions
{
    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class Queryable
    {
        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate
        ///     or throws an exception specified for the
        ///     <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElementOrThrowOnMultiple<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception multipleMatchesFoundException)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var results = queryable.Where(predicate)
                .Take(2)
                .AsEnumerable()
                .Select(arg => new SingleElementResult<T>(arg))
                .DefaultIfEmpty(SingleElementResult<T>.NoElements)
                .ToArray();

            return results.Length == 1
                ? results.First()
                : throw multipleMatchesFoundException
                        ?? new ArgumentNullException(nameof(multipleMatchesFoundException));
        }
    }
}