namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public class FirstQueryableExtensionsTests
    {
        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstOrThrow(x => x == 1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<int>)null).FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action firstOrThrow = () => new object[0].AsQueryable().FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new[] { 1 }.AsQueryable().FirstOrThrow(null, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(x => x == 0, ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstOrThrow((Exception)null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var queryable = new object[0].AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }
    }
}
