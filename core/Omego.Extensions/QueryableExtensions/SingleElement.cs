namespace Omego.Extensions.QueryableExtensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class Queryable
    {
        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElement<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var results = queryable.Where(predicate)
                .Take(2)
                .Select(arg => new SingleElementResult<T>(arg))
                .DefaultIfEmpty(SingleElementResult<T>.NoElements);

            return results.Count() > 1 ? SingleElementResult<T>.MultipleElements : results.First();
        }
    }
}