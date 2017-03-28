namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Omego.Extensions.Poco;
    using Omego.Extensions.QueryableExtensions;

    using Xunit;

    public class SingleElementTests
    {
        [Fact]
        public void SingleElementByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleElement(x => x == 1).ValueOr(null).Should().Be(1);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnMultipleMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new[] { 1, 2 }.AsQueryable();

            queryable.SingleElement(x => true).Should().Be(SingleElementResult<int>.MultipleElements);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new int[0].AsQueryable();

            queryable.SingleElement(x => false).Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElement = () => new int[0].AsQueryable().SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenqueryableArgumentIsNull()
        {
            Action singleElement = () => ((IQueryable<int>)null).SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }
    }
}