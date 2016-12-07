namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Omego.Extensions.QueryableExtensions;

    using Xunit;

    public class SingleOrDefaultOrThrowTests
    {
        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var queryable = new object[2].AsQueryable();

            Action singleOrDefaultOrThrow = () => queryable.SingleOrDefaultOrThrow(o => o == null, (object)null, ex);

            singleOrDefaultOrThrow.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldReturnElementIfOneExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOrDefaultOrThrow(s => s == "1", (Func<string>)null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOrDefaultOrThrow(o => false, () => "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var queryable = new object[2].AsQueryable();

            Action singleOrDefaultOrThrow = () => queryable.SingleOrDefaultOrThrow(o => o == null, null, ex);

            singleOrDefaultOrThrow.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnElementIfOneExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOrDefaultOrThrow(s => s == "1", (string)null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOrDefaultOrThrow(o => false, "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrDefaultOrThrow =
                () => ((IQueryable<string>)null).SingleOrDefaultOrThrow(null, (string)null, null);

            singleOrDefaultOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("queryable");
        }
    }
}