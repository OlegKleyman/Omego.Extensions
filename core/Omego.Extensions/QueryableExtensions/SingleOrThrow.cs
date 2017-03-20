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
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="noMatchFoundException" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException) => queryable
            .SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException)
            .ValueOr(
                () => throw noMatchFoundException ?? throw new ArgumentNullException(nameof(noMatchFoundException)));

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrThrow<T>(
            this IQueryable<T> queryable,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException) => queryable.SingleOrThrow(
            element => true,
            noMatchFoundException,
            multipleMatchesFoundException);

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SingleOrThrow(
                queryable,
                predicate,
                new InvalidOperationException($"No match found for {predicate.Body}."),
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }
    }
}