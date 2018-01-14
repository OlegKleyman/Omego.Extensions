using System;
using System.Collections.Generic;

namespace Omego.Extensions.Poco
{
    /// <summary>
    ///     Represents a generic equality comparer comparer.
    /// </summary>
    /// <typeparam name="TSource">The type to use for comparison.</typeparam>
    public class GenericEqualityComparer<TSource> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TSource, bool> areEqual;

        private readonly Func<TSource, int> hashCode;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericEqualityComparer{TSource}" /> of
        ///     <typeparamref name="TSource" /> struct.
        /// </summary>
        /// <param name="areEqual">
        ///     The <see cref="Func{T1, T2, TResult}" /> of <typeparamref name="TSource" />,
        ///     <typeparamref name="TSource" />, and <see cref="bool" /> to use for equality comparison.
        /// </param>
        /// <param name="hashCode">
        ///     The <see cref="Func{T1, TResult}" /> of <typeparamref name="TSource" />,
        ///     and <see cref="int" /> to use to get the hash code.
        /// </param>
        public GenericEqualityComparer(Func<TSource, TSource, bool> areEqual, Func<TSource, int> hashCode)
        {
            this.areEqual = areEqual ?? throw new ArgumentNullException(nameof(areEqual));
            this.hashCode = hashCode ?? throw new ArgumentNullException(nameof(hashCode));
        }

        /// <inheritdoc />
        public bool Equals(TSource x, TSource y)
        {
            return areEqual(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(TSource obj)
        {
            return hashCode(obj);
        }
    }
}