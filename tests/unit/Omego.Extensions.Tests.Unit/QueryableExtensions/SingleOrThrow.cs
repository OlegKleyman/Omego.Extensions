using System;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.QueryableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    public class SingleOrThrow
    {
        [Fact]
        public void SingleOrThrowByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.SingleOrThrow(x => x == 1, null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowByQueryShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IQueryable<int>) null).SingleOrThrow(x => false);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("queryable");
        }

        [Fact]
        public void
            SingleOrThrowByQueryShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenMultipleMatchesFound()
        {
            Action singleOrThrow = () => new[] {1, 1}.AsQueryable().SingleOrThrow(x => true, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowByQueryShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(x => false, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleOrThrow = () => new int[0].AsQueryable().SingleOrThrow(null, null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowByQueryShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1}.AsQueryable().SingleOrThrow(x => x == 0, ex, null);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1, 1}.AsQueryable().SingleOrThrow(x => x == 1, null, ex);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowByQueryWithGenericExceptionShouldReturnElementWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.SingleOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.SingleOrThrow(null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IQueryable<int>) null).SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenMultipleMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => new[] {1, 1}.AsQueryable().SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenNoMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(null, null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(ex, null);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] {1, 1}.AsQueryable().SingleOrThrow(null, ex);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionByQueryShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IQueryable<int>) null).SingleOrThrow(x => false);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("queryable");
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleOrThrow = () => new int[0].AsQueryable().SingleOrThrow(null);

            singleOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionByQueryShouldThrowExceptionWhenAnElementIsNotFound()
        {
            Action singleOrThrow = () => new[] {1}.AsQueryable().SingleOrThrow(x => x == 0);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("No match found for (x == 0).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            Action singleOrThrow = () => new[] {1, 1}.AsQueryable().SingleOrThrow(x => x == 1);

            singleOrThrow.Should().ThrowExactly<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("More than one match found for (x == 1).");
        }
    }
}