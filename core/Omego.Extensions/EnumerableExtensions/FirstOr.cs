namespace Omego.Extensions.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FirstOr<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T @default)
        {
            return enumerable.FirstOr(predicate, () => @default);
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOr<T>(this IEnumerable<T> enumerable, T @default)
        {
            return enumerable.FirstOr(arg => true, @default);
        }

        public static T FirstOr<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Func<T> @default)
        {
            return enumerable.FirstElement(predicate).ValueOr(@default);
        }

        public static T FirstOr<T>(this IEnumerable<T> enumerable, Func<T> @default)
        {
            return enumerable.FirstOr(x => true, @default);
        }
    }
}