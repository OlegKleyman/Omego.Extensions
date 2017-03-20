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
        ///     Returns the first element of an <see cref="IQueryable" /> matching the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the object to find.</typeparam>
        /// <param name="queryable">The queryable to find the element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An <see cref="Element{T}" /> of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static Element<T> FirstElement<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return queryable.Where(predicate)
                .Take(1)
                .AsEnumerable()
                .Select(arg => new Element<T>(arg))
                .FirstOrDefault();
        }
    }
}