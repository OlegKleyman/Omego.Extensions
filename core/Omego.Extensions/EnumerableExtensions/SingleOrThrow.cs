namespace Omego.Extensions.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        ///     Returns a single match from an <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrThrow<T>(
            this IEnumerable<T> enumerable,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException) => enumerable.SingleOrThrow(
            element => true,
            noMatchFoundException,
            multipleMatchesFoundException);

        /// <summary>
        ///     Returns a single match from an <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(
            this IEnumerable<T> enumerable,
            Expression<Func<T, bool>> predicate) => SingleOrThrow(
            enumerable,
            predicate?.Compile(),
            new InvalidOperationException($"No match found for {predicate?.Body}."),
            new InvalidOperationException($"More than one match found for {predicate?.Body}."));

        /// <summary>
        ///     Returns a single match from an <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="noMatchFoundException" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(
            this IEnumerable<T> enumerable,
            Func<T, bool> predicate,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException) => enumerable
            .SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException)
            .ValueOr(
                () => throw noMatchFoundException ?? throw new ArgumentNullException(nameof(noMatchFoundException)));
    }
}