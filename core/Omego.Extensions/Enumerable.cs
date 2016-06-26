namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Exception exception)
        {
            if (!enumerable.Any())
            {
                if (exception == null) throw new ArgumentNullException(nameof(exception));

                throw exception;
            }

            return enumerable.First();
        }

        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Exception exception)
        {
            var elements = enumerable.Where(predicate);

            return elements.FirstOrThrow(exception);
        }
    }
}