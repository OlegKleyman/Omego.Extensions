using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.Poco
{
    public class SingleElementResultTests
    {
        [Theory]
        [MemberData(nameof(SingleElementResultTestsTheories.ElementEqualityTheory), MemberType =
            typeof(SingleElementResultTestsTheories))]
        public void EqualsShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            firstElement.Equals(secondElement).Should().Be(expected);
            secondElement.Equals(firstElement).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SingleElementResultTestsTheories.ValueEqualityTheory), MemberType =
            typeof(SingleElementResultTestsTheories))]
        public void EqualsShouldReturnWhetherSingleElementValuesAreEqual(
            SingleElementResult<object> element,
            object value,
            bool expected)
        {
            element.Equals(value).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SingleElementResultTestsTheories.ObjectSingleElementEqualityTheory), MemberType =
            typeof(SingleElementResultTestsTheories))]
        public void ObjectEqualsShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> element,
            object value,
            bool expected)
        {
            ((object) element).Equals(value).Should().Be(expected);
        }

        [Theory]
        [MemberData(
            nameof(SingleElementResultTestsTheories.ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory),
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void ObjectGetHashCodeShouldReturnSingleElementHashCode(
            SingleElementResult<object> element,
            int expected)
        {
            element.GetHashCode().Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SingleElementResultTestsTheories.ElementEqualityTheory), MemberType =
            typeof(SingleElementResultTestsTheories))]
        public void EqualsOperatorShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            (firstElement == secondElement).Should().Be(expected);
            (secondElement == firstElement).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SingleElementResultTestsTheories.ElementEqualityTheory), MemberType =
            typeof(SingleElementResultTestsTheories))]
        public void NotEqualsOperatorShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            (firstElement != secondElement).Should().Be(!expected);
            (secondElement != firstElement).Should().Be(!expected);
        }

        public class SingleElementResultTestsTheories
        {
            public static IEnumerable<object[]> ElementEqualityTheory = new[]
            {
                new object[]
                {
                    new SingleElementResult<object>(1),
                    new SingleElementResult<object>(1), true
                },
                new object[]
                {
                    new SingleElementResult<object>(1),
                    new SingleElementResult<object>(2), false
                },
                new object[]
                {
                    new SingleElementResult<object>(null),
                    new SingleElementResult<object>(2), false
                },
                new object[]
                {
                    new SingleElementResult<object>(null),
                    new SingleElementResult<object>(null),
                    true
                },
                new object[]
                {
                    new SingleElementResult<object>(),
                    new SingleElementResult<object>(2), false
                },
                new object[]
                {
                    new SingleElementResult<object>(),
                    new SingleElementResult<object>(), true
                },
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    SingleElementResult<object>
                        .MultipleElements,
                    true
                },
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    SingleElementResult<object>.NoElements,
                    false
                },
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    new SingleElementResult<object>(1), false
                }
            };

            public static IEnumerable<object[]> ValueEqualityTheory = new[]
            {
                new object[]
                    {new SingleElementResult<object>(1), 1, true},
                new object[]
                {
                    new SingleElementResult<object>(1), 2, false
                },
                new object[]
                {
                    new SingleElementResult<object>(null), 2,
                    false
                },
                new object[]
                {
                    new SingleElementResult<object>(null), null,
                    true
                },
                new object[]
                    {new SingleElementResult<object>(), 2, false},
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    null, false
                },
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    1, false
                }
            };

            public static IEnumerable<object[]> ObjectSingleElementEqualityTheory = new[]
            {
                new object[]
                {
                    new SingleElementResult<object>(
                        1),
                    new SingleElementResult<object>(
                        1),
                    true
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        1),
                    new SingleElementResult<object>(
                        2),
                    false
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        null),
                    new SingleElementResult<object>(
                        2),
                    false
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        null),
                    new SingleElementResult<object>(
                        null),
                    true
                },
                new object[]
                {
                    new SingleElementResult<object>(),
                    new SingleElementResult<object>(
                        2),
                    false
                },
                new object[]
                {
                    new SingleElementResult<object>(),
                    new SingleElementResult<object>(),
                    true
                },
                new object[]
                {
                    new SingleElementResult<object>(),
                    1, false
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        "test"),
                    "test", true
                }
            };

            public static IEnumerable<object[]> ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory = new[]
            {
                new object[]
                {
                    new SingleElementResult<object>(),
                    0
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        1),
                    194
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        0),
                    193
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        null),
                    1
                },
                new object[]
                {
                    SingleElementResult<object>
                        .MultipleElements,
                    2
                },
                new object[]
                {
                    new SingleElementResult<object>(
                        2),
                    195
                }
            };

            public static IEnumerable<object[]>
                ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDoneTheory
                    =
                    new[]
                    {
                        new object[]
                        {
                            SingleElementResult<string>.MultipleElements,
                            "Multiple element(s) cannot be cast to System.String."
                        },
                        new object[]
                        {
                            SingleElementResult<string>.NoElements,
                            "None element(s) cannot be cast to System.String."
                        }
                    };

            public static IEnumerable<object[]> ToStringShouldReturnStringRepresentationOfTheValueTheory =
                new[] {new object[] {1, "1"}, new object[] {null, "Exists"}};
        }

        [Theory]
        [MemberData(
            nameof(SingleElementResultTestsTheories
                .ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDoneTheory
            ),
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void
            ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDone(
                SingleElementResult<string> element,
                string expectedMessage)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed - looking for exception to occur
            Action explicitCast = () => ((string) element).GetType();

            explicitCast.Should().Throw<InvalidCastException>().WithMessage(expectedMessage);
        }

        [Theory]
        [MemberData(
            nameof(SingleElementResultTestsTheories.ToStringShouldReturnStringRepresentationOfTheValueTheory),
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void ToStringShouldReturnStringRepresentationOfTheValue(
            object value,
            string expectedString)
        {
            new SingleElementResult<object>(value).ToString()
                .Should().BeEquivalentTo(expectedString);
        }

        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(2);

            result.ValueOr(null).Should().Be(2);
        }

        [Fact]
        public void DefaultValueOrShouldReturnDefaultValueWhenMultipleExists()
        {
            SingleElementResult<int>
                .MultipleElements.ValueOr(() => 3)
                .Should().Be(3);
        }

        [Fact]
        public void DefaultValueOrShouldReturnDefaultValueWhenNoneExists()
        {
            new SingleElementResult<int>()
                .ValueOr(() => 3)
                .Should().Be(3);
        }

        [Fact]
        public void DefaultValueOrShouldReturnDefaultValueWhenOneExists()
        {
            new SingleElementResult<int>(3)
                .ValueOr(null)
                .Should().Be(3);
        }

        [Fact]
        public void ExplicitOperatorFromSingleElementToGenericTypeShouldReturnGenericTypeObjectFact()
        {
            var element = new SingleElementResult<string>("test");
            var @string = (string) element;

            @string.Should().IsSameOrEqualTo(element);
        }

        [Fact]
        public void
            ImplicitOperatorFromGenericTypeToSingleElementShouldReturnSingleElementWrappingGenericTypeObjectFact()
        {
            SingleElementResult<string> element = "test";

            element.Should().IsSameOrEqualTo("test");
        }

        [Fact]
        public void MultipleElementsShouldReturnMultipleSingleElementResult()
        {
            var result = SingleElementResult<int>.MultipleElements;

            result.Should().Be(SingleElementResult<int>.MultipleElements);
        }

        [Fact]
        public void NoElementsShouldReturnNoneSingleElementResult()
        {
            var result = SingleElementResult<int>.NoElements;

            result.Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void ToStringShouldReturnStringRepresentationOfTheElementWhenElementDoesNotExist()
        {
            SingleElementResult<object>
                .NoElements.ToString()
                .Should().BeEquivalentTo("Does not exist");
        }

        [Fact]
        public void ToStringShouldReturnStringRepresentationOfTheElementWhenMultipleElementsExist()
        {
            SingleElementResult<object>
                .MultipleElements.ToString()
                .Should().BeEquivalentTo("Multiple");
        }

        [Fact]
        public void ValueOrShouldReturnDefaultValueWhenMultipleExists()
        {
            new SingleElementResult<int>(7)
                .ValueOr(() => 3, () => 7)
                .Should().Be(7);
        }

        [Fact]
        public void ValueOrShouldReturnDefaultValueWhenNoneExists()
        {
            new SingleElementResult<int>().ValueOr(() => 3)
                .Should().Be(3);
        }

        [Fact]
        public void ValueOrShouldReturnValueWhenOneExists()
        {
            new SingleElementResult<int>(3).ValueOr(null)
                .Should().Be(3);
        }

        [Fact]
        public void ValueOrShouldThrowArgumentNullExceptionWhenDefaultMultipleElementsIsNull()
        {
            Action valueOr = () => SingleElementResult<int>.MultipleElements.ValueOr(null, null);

            valueOr.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("defaultMultipleElements");
        }

        [Fact]
        public void ValueOrShouldThrowArgumentNullExceptionWhenDefaultSelectorIsNull()
        {
            Action valueOr = () => new SingleElementResult<int>().ValueOr(null);

            valueOr.Should().Throw<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("default");
        }
    }
}