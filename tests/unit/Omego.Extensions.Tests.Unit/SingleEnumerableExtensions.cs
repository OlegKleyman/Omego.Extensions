namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Xunit;

    public partial class EnumerableTests
    {
        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.SingleOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action SingleOrThrow = () => new[] { 1 }.SingleOrThrow(x => x == 0);

            SingleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Message.ShouldBeEquivalentTo("No match found for (x == 0).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            Action SingleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(x => x == 1);

            SingleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Message.ShouldBeEquivalentTo("More than one match found for (x == 1).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action SingleOrThrow = () => new int[0].SingleOrThrow((Expression<Func<int, bool>>)null);

            SingleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action SingleOrThrow = () => ((IEnumerable<int>)null).SingleOrThrow(x => false);

            SingleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }
    }
}
