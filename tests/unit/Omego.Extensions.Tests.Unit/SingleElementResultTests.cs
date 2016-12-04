namespace Omego.Extensions.Tests.Unit
{
    using System;

    using FluentAssertions;

    using Xunit;

    [CLSCompliant(false)]
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

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "test1", false)]
        public void EqualsShouldReturnWhetherSingleElementObjectsAreEqual(string firstValue, string secondValue, bool expectedResult)
        {
            var firstResult = new SingleElementResult<string>(firstValue);
            var secondResult = new SingleElementResult<string>(secondValue);

            firstResult.Equals(secondResult).ShouldBeEquivalentTo(expectedResult);
            secondResult.Equals(firstResult).ShouldBeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "test1", false)]
        public void EqualsOperatorShouldReturnWhetherSingleElementObjectsAreEqual(string firstValue, string secondValue, bool expectedResult)
        {
            var firstResult = new SingleElementResult<string>(firstValue);
            var secondResult = new SingleElementResult<string>(secondValue);

            (firstResult == secondResult).ShouldBeEquivalentTo(expectedResult);
            (secondResult == firstResult).ShouldBeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "test1", false)]
        public void EqualsGenericShouldReturnWhetherSingleElementObjectsAreEqual(string firstValue, string secondValue, bool expectedResult)
        {
            var firstResult = new SingleElementResult<string>(firstValue);
            var secondResult = new SingleElementResult<string>(secondValue);

            firstResult.Equals(secondResult).ShouldBeEquivalentTo(expectedResult);
            secondResult.Equals(firstResult).ShouldBeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(1, 1)]
        public void GetHashCodeShouldReturnHashCodeRegardlessOfValue(int value, int expectedHashCoe)
        {
            new SingleElementResult<int>(value).GetHashCode().ShouldBeEquivalentTo(expectedHashCoe);
        }

        [Fact]
        public void EqualsGenericShouldReturnAreEqualWhenValueIsNotPresent()
        {
            var firstResult = new SingleElementResult<string>();
            var secondResult = new SingleElementResult<string>();

            firstResult.Equals(secondResult).Should().BeTrue();
            secondResult.Equals(firstResult).Should().BeTrue();
        }

        [Fact]
        public void EqualsGenericShouldReturnAreNotEqualWhenValueIsNotPresent()
        {
            SingleElementResult<string>.MultipleElements.Equals(SingleElementResult<string>.NoElements)
                .Should()
                .BeFalse();
            SingleElementResult<string>.NoElements.Equals(SingleElementResult<string>.MultipleElements)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void EqualsGenericShouldReturnAreNotEqualWhenOneValueIsNotPresentAndTheOtherIs()
        {
            var presentResult = new SingleElementResult<string>("test");

            SingleElementResult<string>.MultipleElements.Equals(presentResult)
                .Should()
                .BeFalse();
            presentResult.Equals(SingleElementResult<string>.MultipleElements)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeIfValueIsNotPresent()
        {
            new SingleElementResult<int>().GetHashCode().ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeIfValueIsNull()
        {
            new SingleElementResult<string>(null).GetHashCode().ShouldBeEquivalentTo(0);
        }
    }
}