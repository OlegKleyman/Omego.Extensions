using System;

namespace Omego.Extensions.Tests.Unit.Poco
{
    using FluentAssertions;

    using Omego.Extensions.Poco;

    using Xunit;

    [CLSCompliant(false)]
    public class GenericEqualityComparerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(default(int))]
        public void GetHashCodeShouldReturnHashCodeFromLambda(int hashCode)
            =>
                GetGenericEqualityComparer<object>(o => hashCode)
                    .GetHashCode(default(object))
                    .ShouldBeEquivalentTo(hashCode);

        [Theory]
        [InlineData(1)]
        [InlineData(default(int))]
        public void EqualsShouldReturnWhetherObjectsAreEquivalentFromLambda(bool areEqual)
            =>
                GetGenericEqualityComparer<object>((o, o1) => areEqual)
                    .Equals(default(object), default(object))
                    .ShouldBeEquivalentTo(areEqual);

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(
            Func<TSource, TSource, bool> areEqual,
            Func<TSource, int> hashCode) => new GenericEqualityComparer<TSource>(areEqual, hashCode);

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(
            Func<TSource, TSource, bool> areEqual) => GetGenericEqualityComparer(areEqual, source => default(int));

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(Func<TSource, int> hashCode)
            => GetGenericEqualityComparer((source, source1) => default(bool), hashCode);
    }
}
