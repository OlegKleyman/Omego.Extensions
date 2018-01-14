using System;
using System.Linq;
using System.Linq.Expressions;

namespace Omego.Extensions.QueryableExtensions
{
    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class Queryable
    {
        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="exception" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception exception)
        {
            return queryable.FirstElement(predicate)
                .ValueOr(() => throw exception ?? throw new ArgumentNullException(nameof(exception)));
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOrThrow<T>(this IQueryable<T> queryable, Exception exception)
        {
            return queryable.FirstOrThrow(
                element => true,
                exception);
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate)
        {
            return queryable.FirstOrThrow(
                predicate ?? throw new ArgumentNullException(nameof(predicate)),
                new InvalidOperationException($"No matches found for: {predicate.Body}"));
        }
    }
}