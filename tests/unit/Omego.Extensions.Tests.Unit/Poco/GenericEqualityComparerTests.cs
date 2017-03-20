namespace Omego.Extensions.Tests.Unit.Poco
{
    using System;
    using System.Collections;

    using FluentAssertions;

    using Omego.Extensions.Poco;

    using Xunit;

    [CLSCompliant(false)]
    public class GenericEqualityComparerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(default(int))]
        public void GetHashCodeShouldReturnHashCodeFromLambda(int hashCode) => GetGenericEqualityComparer<object>(
                o => hashCode)
            .GetHashCode(default(object))
            .ShouldBeEquivalentTo(hashCode);

        [Theory]
        [InlineData(1)]
        [InlineData(default(int))]
        public void EqualsShouldReturnWhetherObjectsAreEquivalentFromLambda(
            bool areEqual) => GetGenericEqualityComparer<object>((o, o1) => areEqual)
            .Equals(default(object), default(object))
            .ShouldBeEquivalentTo(areEqual);

        [Theory]
        [MemberData(
            "ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory",
            MemberType = typeof(GenericEqualityComparerTestsTheories))]
        public void ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            Func<object, object, bool> areEqual,
            Func<object, int> hashCode)
        {
            Action constructor = () => new GenericEqualityComparer<object>(areEqual, hashCode);

            constructor.ShouldThrow<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(
            Func<TSource, TSource, bool> areEqual,
            Func<TSource, int> hashCode) => new GenericEqualityComparer<TSource>(areEqual, hashCode);

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(
            Func<TSource, TSource, bool> areEqual) => GetGenericEqualityComparer(areEqual, source => default(int));

        private GenericEqualityComparer<TSource> GetGenericEqualityComparer<TSource>(
            Func<TSource, int> hashCode) => GetGenericEqualityComparer((source, source1) => default(bool), hashCode);

        public class GenericEqualityComparerTestsTheories
        {
            public static IEnumerable ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory =
                new[]
                    {
                        new object[]
                            {
                                "Value cannot be null.\r\nParameter name: areEqual", "areEqual",
                                typeof(ArgumentNullException), null, null
                            },
                        new object[]
                            {
                                "Value cannot be null.\r\nParameter name: hashCode", "hashCode",
                                typeof(ArgumentNullException), (Func<object, object, bool>)((x, y) => true), null
                            }
                    };
        }
    }
}