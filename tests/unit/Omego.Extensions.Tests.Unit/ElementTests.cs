namespace Omego.Extensions.Tests.Unit
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class ElementTests
    {
        [Fact]
        public void ConstructorShouldCreateElementWithValue()
        {
            var element = new Element<string>("test");

            element.Present.Should().BeTrue();
            element.Value.ShouldBeEquivalentTo("test");
        }

        [Fact]
        public void ValueShouldThrowInvalidOperationExceptionWhenElementDoesNotExist()
        {
            var element = new Element<int>();

            Action value = () => element.Value.ToString();

            value.ShouldThrow<InvalidOperationException>().WithMessage("Element does not exist");
        }
    }
}
