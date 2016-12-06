﻿namespace Omego.Extensions.EnumerableExtensions
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
        ///     Returns a single match from an <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="enumerable">The enumerable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="enumerable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElement<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var result = default(SingleElementResult<T>);

            using (var e = enumerable.GetEnumerator())
            {
                while (e.MoveNext())
                    if (predicate(e.Current))
                    {
                        result = new SingleElementResult<T>(e.Current);

                        while (e.MoveNext())
                            if (predicate(e.Current))
                            {
                                result = SingleElementResult<T>.MultipleElements;
                                break;
                            }

                        break;
                    }
            }

            return result;
        }
    }
}