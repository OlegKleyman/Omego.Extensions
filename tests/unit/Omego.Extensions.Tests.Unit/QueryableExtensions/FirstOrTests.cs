namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Omego.Extensions.QueryableExtensions;

    using Xunit;

    public class FirstOrTests
    {
        [Fact]
        public void FirstOrShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr(x => x == "1", null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var queryable = new string[0].AsQueryable();

            queryable.FirstOr("3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnTheFirstElementWhenExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr(null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr(null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOr = () => new string[] { null }.AsQueryable().FirstOr(null, null);

            firstOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }
    }
}