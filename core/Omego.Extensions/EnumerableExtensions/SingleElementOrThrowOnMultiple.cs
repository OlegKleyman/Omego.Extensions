namespace Omego.Extensions.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;

    using Omego.Extensions.Poco;

    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        ///     Returns a single element of an <see cref="IEnumerable{T}" /> matching the given predicate
        ///     or throws an exception specified for the
        ///     <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="multipleMatchesFoundException" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElementOrThrowOnMultiple<T>(
            this IEnumerable<T> enumerable,
            Func<T, bool> predicate,
            Exception multipleMatchesFoundException)
        {
            var element = enumerable.SingleElement(predicate);

            if (element == SingleElementResult<T>.MultipleElements)
            {
                if (multipleMatchesFoundException == null) throw new ArgumentNullException(nameof(multipleMatchesFoundException));
                throw multipleMatchesFoundException;
            }

            return element;
        }
    }
}