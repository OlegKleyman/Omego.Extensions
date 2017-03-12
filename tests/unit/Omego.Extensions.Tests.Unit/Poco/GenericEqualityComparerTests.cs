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
        {
            Func<object, int> hashCodeGenerator = o => hashCode;
            object target = null;

            GetGenericEqualityComparer(hashCodeGenerator).GetHashCode(target).ShouldBeEquivalentTo(hashCode);
        }

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(Func<TSource, int> hashCode)
        {
            return new GenericEqualityComparer<TSource>(hashCode);
        }
    }
}
