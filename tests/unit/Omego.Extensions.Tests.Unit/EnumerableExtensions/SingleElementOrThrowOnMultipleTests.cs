using System;
using System.Collections.Generic;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class SingleElementOrThrowOnMultipleTests
    {
        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new int[0];

            enumerable.SingleElementOrThrowOnMultiple(x => false, null)
                .Should()
                .Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new[] {1, 2};

            var ex = new InvalidOperationException();

            Action singleElementOrThrowOnMultiple = () => enumerable.SingleElementOrThrowOnMultiple(i => true, ex);

            singleElementOrThrowOnMultiple.Should().Throw<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsExceptionIsNull()
        {
            var enumerable = new[] {1, 2};

            Action singleElementOrThrowOnMultiple = () => enumerable.SingleElementOrThrowOnMultiple(i => true, null);

            singleElementOrThrowOnMultiple.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple =
                () => ((IEnumerable<int>) null).SingleElementOrThrowOnMultiple(i => true, null);

            singleElementOrThrowOnMultiple.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple = () => new int[0].SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("predicate");
        }
    }
}