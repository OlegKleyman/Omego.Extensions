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
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FirstOr<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            T @default) => queryable.FirstOr(predicate, () => @default);

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOr<T>(this IQueryable<T> queryable, T @default) => queryable.FirstOr(
            arg => true,
            @default);

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FirstOr<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Func<T> @default) => queryable.FirstElement(predicate).ValueOr(@default);

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOr<T>(this IQueryable<T> queryable, Func<T> @default) => queryable.FirstOr(
            arg => true,
            @default);
    }
}