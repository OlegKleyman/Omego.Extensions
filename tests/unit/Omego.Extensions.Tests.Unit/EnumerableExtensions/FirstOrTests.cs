using System;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class FirstOrTests
    {
        [Fact]
        public void FirstOrLazyByQueryShouldArgumentNullExceptionWhenDefaultFuncIsNull()
        {
            var enumerable = new[] {"1"};

            Action firstOr = () => enumerable.FirstOr(x => x == "2", (Func<string>) null);

            firstOr.Should().Throw<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("default");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr(x => x == "1", (Func<string>) null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(s => false, () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr(x => x == "2", () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnElementWhenFound()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr((Func<string>) null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr(x => x == "1", (string) null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr("3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnTheFirstElementWhenExists()
        {
            var enumerable = new[] {"1"};

            enumerable.FirstOr((string) null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOr = () => new string[] {null}.FirstOr(null, (string) null);

            firstOr.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("predicate");
        }
    }
}