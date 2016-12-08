namespace Omego.Extensions.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        ///     Returns a single element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" /> or throws an exception specified
        ///     for the <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrDefaultOrThrow<T>(
            this IEnumerable<T> enumerable,
            Func<T, bool> predicate,
            T @default,
            Exception multipleMatchesFoundException) => enumerable.SingleOrDefaultOrThrow(predicate, () => @default, multipleMatchesFoundException);

        /// <summary>
        ///     Returns a single element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" /> or throws an exception specified
        ///     for the <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrDefaultOrThrow<T>(
            this IEnumerable<T> enumerable,
            Func<T, bool> predicate,
            Func<T> @default,
            Exception multipleMatchesFoundException) => enumerable.SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException).ValueOr(@default);
    }
}