using System;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.Poco;
using Omego.Extensions.QueryableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    public class SingleElementOrThrowOnMultipleTests
    {
        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.SingleElementOrThrowOnMultiple(x => x == 1, null).ValueOr(null).Should().Be(1);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new int[0].AsQueryable();

            queryable.SingleElementOrThrowOnMultiple(x => false, null).Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new[] {1, 2}.AsQueryable();

            var ex = new InvalidOperationException();

            Action singleElementOrThrowOnMultiple = () => queryable.SingleElementOrThrowOnMultiple(i => true, ex);

            singleElementOrThrowOnMultiple.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsExceptionIsNull()
        {
            var queryable = new[] {1, 2}.AsQueryable();

            Action singleElementOrThrowOnMultiple = () => queryable.SingleElementOrThrowOnMultiple(i => true, null);

            singleElementOrThrowOnMultiple.ShouldThrow<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple =
                () => ((IQueryable<int>) null).SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple = () => new int[0].AsQueryable()
                .SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("predicate");
        }
    }
}