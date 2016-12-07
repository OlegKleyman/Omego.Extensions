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
        ///     Returns a single element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOr<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> predicate, T @default)
        {
            return enumerable.SingleOr(predicate, () => @default);
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IEnumerable{T}" /> or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOr<T>(this IEnumerable<T> enumerable, T @default)
        {
            return enumerable.SingleOr(arg => true, @default);
        }

        public static T SingleOr<T>(
            this IEnumerable<T> enumerable,
            Expression<Func<T, bool>> predicate,
            Func<T> @default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return enumerable.SingleOrDefaultOrThrow(
                predicate.Compile(),
                @default,
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }

        public static T SingleOr<T>(this IEnumerable<T> enumerable, Func<T> @default)
        {
            return enumerable.SingleElement(arg => true).ValueOr(@default);
        }
    }
}