using System;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.QueryableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    public class SingleOrTests
    {
        [Fact]
        public void SingleOrByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] {"1"}.AsQueryable();

            queryable.SingleOr(x => x == "1", (string) null).Should().Be("1");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var queryable = new[] {"1"}.AsQueryable();

            queryable.SingleOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearching()
        {
            Action singleOr = () => new string[] {null}.AsQueryable().SingleOr(null, (string) null);

            singleOr.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNullWhenSearching()
        {
            Action singleOr = () => ((IQueryable<string>) null).SingleOr(x => false, (string) null);

            singleOr.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr(o => o == null, (object) null);

            singleOr.Should().Throw<InvalidOperationException>().WithMessage("More than one match found for (o == null).");
        }

        [Fact]
        public void SingleOrLazyShouldReturnElementIfOneExists()
        {
            var queryable = new[] {"1"}.AsQueryable();

            queryable.SingleOr((Func<string>) null).Should().Be("1");
        }

        [Fact]
        public void SingleOrLazyShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrLazyShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNull()
        {
            Action singleOr = () => ((IQueryable<string>) null).SingleOr((Func<string>) null);

            singleOr.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrLazyShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr((string) null);

            singleOr.Should().Throw<InvalidOperationException>().WithMessage("More than one match found for true.");
        }

        [Fact]
        public void SingleOrShouldReturnElementIfOneExists()
        {
            var queryable = new[] {"1"}.AsQueryable();

            queryable.SingleOr((string) null).Should().Be("1");
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
            Action singleOr = () => ((IQueryable<string>) null).SingleOr((string) null);

            singleOr.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr((string) null);

            singleOr.Should().Throw<InvalidOperationException>().WithMessage("More than one match found for true.");
        }
    }
}