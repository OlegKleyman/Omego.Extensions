using System;
using System.Collections.Generic;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class SingleOrDefaultOrThrowTests
    {
        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldReturnElementIfOneExists()
        {
            var enumerable = new[] {"1"};

            enumerable.SingleOrDefaultOrThrow(s => s == "1", (string) null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var enumerable = new object[0];

            enumerable.SingleOrDefaultOrThrow(o => false, "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrDefaultOrThrow =
                () => ((IEnumerable<string>) null).SingleOrDefaultOrThrow(s => true, (string) null, null);

            singleOrDefaultOrThrow.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var enumerable = new object[2];

            Action singleOrDefaultOrThrow = () => enumerable.SingleOrDefaultOrThrow(o => o == null, (object) null, ex);

            singleOrDefaultOrThrow.Should().Throw<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldReturnElementIfOneExists()
        {
            var enumerable = new[] {"1"};

            enumerable.SingleOrDefaultOrThrow(s => s == "1", (Func<string>) null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var enumerable = new object[0];

            enumerable.SingleOrDefaultOrThrow(o => false, () => "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowLazyByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var enumerable = new object[2];

            Action singleOrDefaultOrThrow = () => enumerable.SingleOrDefaultOrThrow(o => o == null, null, ex);

            singleOrDefaultOrThrow.Should().Throw<InvalidOperationException>().Which.Should().Be(ex);
        }
    }
}