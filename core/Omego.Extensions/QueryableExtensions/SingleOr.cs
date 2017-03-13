namespace Omego.Extensions.QueryableExtensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class Queryable
    {
        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOr<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            T @default) => queryable.SingleOr(predicate, () => @default);

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOr<T>(this IQueryable<T> queryable, T @default) => queryable.SingleOr(
            arg => true,
            @default);

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOr<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, Func<T> @default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return queryable.SingleOrDefaultOrThrow(
                predicate,
                @default,
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOr<T>(this IQueryable<T> queryable, Func<T> @default) => queryable.SingleOr(
            arg => true,
            @default);
    }
}