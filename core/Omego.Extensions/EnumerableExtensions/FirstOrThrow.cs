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
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> or throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Exception exception)
        {
            return enumerable.FirstOrThrow(element => true, exception);
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="exception" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Exception exception)
        {
            var element = enumerable.FirstElement(predicate);

            if (!element.Present)
            {
                if (exception == null) throw new ArgumentNullException(nameof(exception));

                throw exception;
            }

            return element.Value;
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return enumerable.FirstOrThrow(
                predicate.Compile(),
                new InvalidOperationException($"No " + $"matches" + $" found for: {predicate.Body}"));
        }
    }
}
