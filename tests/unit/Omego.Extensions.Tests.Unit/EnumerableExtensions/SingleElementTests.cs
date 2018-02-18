using System;
using System.Collections.Generic;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class SingleElementTests
    {
        [Fact]
        public void SingleElementByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.SingleElement(x => x == 1).ValueOr(null).Should().Be(1);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnMultipleMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new[] {1, 2};

            enumerable.SingleElement(x => true).Should().Be(SingleElementResult<int>.MultipleElements);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new int[0];

            enumerable.SingleElement(x => false).Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElement = () => ((IEnumerable<int>) null).SingleElement(i => true);

            singleElement.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElement = () => new int[0].SingleElement(null);

            singleElement.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.SingleElementOrThrowOnMultiple(x => x == 1, null).ValueOr(null).Should().Be(1);
        }
    }
}