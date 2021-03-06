﻿using System;
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
        ///     Attempts to catch an exception an enumeration might throw and continue the enumeration.
        /// </summary>
        /// <typeparam name="T">The type to iterate.</typeparam>
        /// <typeparam name="TE">The type of <see cref="Exception" /> to catch.</typeparam>
        /// <param name="target">The <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> to iterate on.</param>
        /// <param name="handler">
        ///     The <see cref="Action{T}" /> of <typeparamref name="TE" /> to call when a(n) <typeparamref name="TE" /> exception
        ///     occurrs.
        /// </param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> instance.</returns>
        public static IEnumerable<T> AttemptCatch<T, TE>(this IEnumerable<T> target, Action<TE> handler)
            where TE : Exception
        {
            var iterator = new AttemptCatchIterator<T, TE>(target, handler);

            while (iterator.MoveNext()) yield return iterator.Current;
        }
    }
}