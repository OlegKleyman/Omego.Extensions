namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        ///     Attempts to catch an exception an enumeration might throw and continue the enumeration.
        /// </summary>
        /// <typeparam name="T">The type to iterate.</typeparam>
        /// <typeparam name="TE">The type of <see cref="Exception" /> to catch.</typeparam>
        /// <param name="target">The <see cref="IEnumerable{T}" /> of <see cref="T" /> to iterate on.</param>
        /// <param name="handler">
        ///     The <see cref="Action{T}" /> of <see cref="TE" /> to call when a(n) <see cref="TE" /> exception
        ///     occurrs.
        /// </param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="T" /> instance.</returns>
        public static IEnumerable<T> AttemptCatch<T, TE>(this IEnumerable<T> target, Action<TE> handler) where TE : Exception
        {
            var iterator = new AttemptCatchIterator<T, TE>(target, handler);

            while (iterator.MoveNext() || iterator.ExceptionOccured)
            {
                if (iterator.ExceptionOccured)
                {
                    continue;
                }

                yield return iterator.Current;
            }
        }

        /// <summary>
        /// Returns the first element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Exception exception)
        {
            return enumerable.FirstOrThrow(element => true, exception);
        }

        /// <summary>
        /// Returns the first element of an <see cref="IEnumerable{T}"/> matching the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Exception exception)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            foreach (var element in enumerable)
            {
                if (predicate == null) throw new ArgumentNullException(nameof(predicate));

                if (predicate(element))
                {
                    return element;
                }
            }

            if (exception == null) throw new ArgumentNullException(nameof(exception));

            throw exception;
        }
    }
}