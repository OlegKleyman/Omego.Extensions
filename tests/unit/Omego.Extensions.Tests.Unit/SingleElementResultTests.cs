namespace Omego.Extensions.Tests.Unit
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class SingleElementResultTests
    {
        [Fact]
        public void MultipleElementsShouldReturnMultipleSingleElementResult()
        {
            var result = SingleElementResult<int>.MultipleElements;

            result.Elements.ShouldBeEquivalentTo(Elements.Multiple);
        }

        [Fact]
        public void NoElementsShouldReturnNoneSingleElementResult()
        {
            var result = SingleElementResult<int>.NoElements;

            result.Elements.ShouldBeEquivalentTo(Elements.None);
        }

        [Fact]
        public void ValueConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(2);

            result.Value.ShouldBeEquivalentTo(2);
            result.Elements.ShouldBeEquivalentTo(Elements.One);
        }

        [Fact]
        public void ValueShouldReturnValueWhenExists()
        {
            var result = new SingleElementResult<string>("test");

            result.Value.ShouldBeEquivalentTo("test");
        }

        [Fact]
        public void ValueShouldThrowInvalidOperationExceptionWhenMultipleElementsExist()
        {
            var result = SingleElementResult<int>.MultipleElements;

            Action value = () => result.Value.ToString();

            value.ShouldThrow<InvalidOperationException>().WithMessage("Multiple elements found.");
        }
    }
}