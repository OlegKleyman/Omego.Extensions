namespace Omego.Extensions.Tests.Unit
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class SingleElementResultTests
    {
        [Fact]
        public void ValueConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(2);

            result.Value.ShouldBeEquivalentTo(2);
            result.Elements.ShouldBeEquivalentTo(Elements.One);
        }

        [Fact]
        public void MatchesConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(Elements.Multiple);
            
            result.Elements.ShouldBeEquivalentTo(Elements.Multiple);
        }

        [Fact]
        public void ValueShouldThrowInvalidOperationExceptionWhenMultipleElementsExist()
        {
            var result = new SingleElementResult<int>(Elements.Multiple);

            Action value = () => result.Value.ToString();

            value.ShouldThrow<InvalidOperationException>().WithMessage("Multiple elements found.");
        }

        [Fact]
        public void ValueShouldReturnValueWhenExists()
        {
            var result = new SingleElementResult<string>("test");

            result.Value.ShouldBeEquivalentTo("test");
        }
    }
}
