namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Omego.Extensions.QueryableExtensions;

    using Xunit;

    public class SingleOrTests
    {
        public void SingleOrShouldReturnElementIfOneExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(null).Should().Be("1");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(x => x == "1", null).Should().Be("1");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearching()
        {
            Action singleOr = () => new string[] { null }.AsQueryable().SingleOr(null, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNullWhenSearching()
        {
            Action singleOr = () => ((IQueryable<string>)null).SingleOr(x => false, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr(o => o == null, (object)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for (o == null).");
        }

        [Fact]
        public void SingleOrShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOr("3").Should().Be("3");
        }

        [Fact]
        public void SingleOrShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNull()
        {
            Action singleOr = () => ((IQueryable<string>)null).SingleOr(null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr((string)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for true.");
        }
    }
}
