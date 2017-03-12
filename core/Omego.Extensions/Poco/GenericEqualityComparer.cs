namespace Omego.Extensions.Poco
{
    using System;
    using System.Collections.Generic;

    public class GenericEqualityComparer<TSource> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, int> hashCode;

        public GenericEqualityComparer(Func<TSource, int> hashCode)
        {
            this.hashCode = hashCode;
        }

        public bool Equals(TSource x, TSource y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(TSource obj) => hashCode(obj);
    }
}