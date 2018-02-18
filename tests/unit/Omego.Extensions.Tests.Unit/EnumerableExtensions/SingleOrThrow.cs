using System;
using System.Collections.Generic;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Xunit;
using Enumerable = System.Linq.Enumerable;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class SingleOrThrow
    {
        [Fact]
        public void SingleOrThrowByQueryThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.SingleOrThrow(x => x == 1, null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.SingleOrThrow(null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IEnumerable<int>) null).SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => ((IEnumerable<int>) null).SingleOrThrow(x => false);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenMultipleMatchesFound()
        {
            Action singleOrThrow = () => new[] {1, 1}.SingleOrThrow(x => true, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(x => false, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenMultipleMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => new[] {1, 1}.SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenNoMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => new int[0].SingleOrThrow(null, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1}.SingleOrThrow(x => x == 0, ex, null);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(ex, null);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1, 1}.SingleOrThrow(null, ex);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1, 1}.SingleOrThrow(x => x == 1, null, ex);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] {1, 2};

            enumerable.SingleOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => ((IEnumerable<int>) null).SingleOrThrow(x => false);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => new int[0].SingleOrThrow(null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action singleOrThrow = () => new[] {1}.SingleOrThrow(x => x == 0);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("No match found for (x == 0).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            Action singleOrThrow = () => new[] {1, 1}.SingleOrThrow(x => x == 1);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("More than one match found for (x == 1).");
        }
    }
}