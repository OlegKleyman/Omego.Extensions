using System;
using System.Collections.Generic;
using Omego.Extensions.Poco;

namespace Omego.Extensions.EnumerableExtensions
{
    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the object to find.</typeparam>
        /// <param name="enumerable">The enumerable to find the element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An <see cref="Element{T}" /> of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="enumerable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static Element<T> FirstElement<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            Element<T> foundElement = default;

            foreach (var element in enumerable)
                if (predicate(element))
                {
                    foundElement = element;
                    break;
                }

            return foundElement;
        }
    }
}