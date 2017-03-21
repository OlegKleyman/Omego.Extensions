namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Omego.Extensions.EnumerableExtensions;

    using Xunit;

    public class FirstElementTests
    {
        [Fact]
        public void FirstElementHasValueShouldReturnFalseWhenElementWasNotFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "2").Present.Should().BeFalse();
        }

        [Fact]
        public void FirstElementHasValueShouldReturnTrueWhenElementWasFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "1").Present.Should().BeTrue();
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => new string[] { null }.FirstElement(null);

            firstElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstElementValueShouldReturnElementValueWhenElementWasFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "1").ValueOr(null).ShouldBeEquivalentTo("1");
        }
    }
}