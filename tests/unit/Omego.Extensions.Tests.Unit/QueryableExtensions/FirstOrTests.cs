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

            queryable.FirstOr(x => x == "1", (string)null).Should().Be("1");
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

            queryable.FirstOr((string)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr((string)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr(x => false, (string)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOr = () => new string[] { null }.AsQueryable().FirstOr(null, (string)null);

            firstOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }
        
        [Fact]
        public void FirstOrLazyByQueryShouldArgumentNullExceptionWhenDefaultFuncIsNull()
        {
            var queryable = new[] { "1" }.AsQueryable();

            Action firstOr = () => queryable.FirstOr(x => x == "2", (Func<string>)null);

            firstOr.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("default");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr(x => x == "1", (Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var queryable = new string[0].AsQueryable();

            queryable.FirstOr(s => false, () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr(x => x == "2", () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnElementWhenFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.FirstOr((Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var queryable = new string[0].AsQueryable();

            queryable.FirstOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var queryable = new string[0].AsQueryable();

            queryable.FirstOr(() => "3").Should().Be("3");
        }
    }
}