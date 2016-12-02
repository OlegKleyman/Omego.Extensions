namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

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
        public static IEnumerable<T> AttemptCatch<T, TE>(this IEnumerable<T> target, Action<TE> handler)
            where TE : Exception
        {
            var iterator = new AttemptCatchIterator<T, TE>(target, handler);

            while (iterator.MoveNext())
            {
                yield return iterator.Current;
            }
        }

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

        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOrThrow<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return enumerable.FirstOrThrow(
                predicate.Compile(),
                new InvalidOperationException($"No matches found for: {predicate.Body}"));
        }

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
            Exception multipleMatchesFoundException)
        {
            return enumerable.SingleOrThrow(element => true, noMatchFoundException, multipleMatchesFoundException);
        }

        /// <summary>
        ///     Returns a single match from an <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrThrow<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SingleOrThrow(
                enumerable,
                predicate,
                new InvalidOperationException($"No match found for {predicate.Body}."),
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }

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
        public static T SingleOrThrow<T>(
            this IEnumerable<T> enumerable,
            Expression<Func<T, bool>> predicate,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var compiledPredicate = predicate.Compile();

            using (var e = enumerable.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    var result = e.Current;

                    if (compiledPredicate(result))
                    {
                        while (e.MoveNext())
                        {
                            if (compiledPredicate(e.Current))
                            {
                                if (multipleMatchesFoundException == null) throw new ArgumentNullException(nameof(multipleMatchesFoundException));

                                throw multipleMatchesFoundException;
                            }
                        }

                        return result;
                    }
                }
            }

            if (noMatchFoundException == null) throw new ArgumentNullException(nameof(noMatchFoundException));

            throw noMatchFoundException;
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IEnumerable{T}" /> matching the given predicate or returns
        /// a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">The enumerable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FirstOr<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T @default)
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

            return @default;
        }

        public static FirstElementResults<T> FirstElement<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            foreach (var element in enumerable)
            {
                if (predicate == null) throw new ArgumentNullException(nameof(predicate));

                if (predicate(element))
                {
                    return new FirstElementResults<T>(element);
                }
            }

            return default(FirstElementResults<T>);
        }
    }
}