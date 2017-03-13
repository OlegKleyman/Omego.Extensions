namespace Omego.Extensions.Poco
{
    using System;
    using System.Collections.Generic;

    public class GenericEqualityComparer<TSource> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TSource, bool> areEqual;

        private readonly Func<TSource, int> hashCode;

        public GenericEqualityComparer(Func<TSource, TSource, bool> areEqual, Func<TSource, int> hashCode)
        {
            this.areEqual = areEqual ?? throw new ArgumentNullException(nameof(areEqual));
            this.hashCode = hashCode ?? throw new ArgumentNullException(nameof(hashCode));
        }

        public bool Equals(TSource x, TSource y) => areEqual(x, y);

        public int GetHashCode(TSource obj) => hashCode(obj);
    }
}