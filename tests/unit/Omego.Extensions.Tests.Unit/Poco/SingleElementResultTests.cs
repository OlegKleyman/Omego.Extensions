namespace Omego.Extensions.Tests.Unit.Poco
{
    using System;
    using System.Collections;

    using FluentAssertions;
    using FluentAssertions.Common;

    using Omego.Extensions.Poco;

    using Xunit;

    [CLSCompliant(false)]
    public class SingleElementResultTests
    {
        [Theory]
        [MemberData("ElementEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void EqualsShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            firstElement.Equals(secondElement).ShouldBeEquivalentTo(expected);
            secondElement.Equals(firstElement).ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [MemberData("ValueEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void EqualsShouldReturnWhetherSingleElementValuesAreEqual(
            SingleElementResult<object> element,
            object value,
            bool expected) => element.Equals(value).ShouldBeEquivalentTo(expected);

        [Theory]
        [MemberData("ObjectSingleElementEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void ObjectEqualsShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> element,
            object value,
            bool expected) => ((object)element).Equals(value).ShouldBeEquivalentTo(expected);

        [Theory]
        [MemberData(
            "ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory",
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void ObjectGetHashCodeShouldReturnSingleElementHashCode(
            SingleElementResult<object> element,
            int expected) => element.GetHashCode().ShouldBeEquivalentTo(expected);

        [Theory]
        [MemberData("ElementEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void EqualsOperatorShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            (firstElement == secondElement).ShouldBeEquivalentTo(expected);
            (secondElement == firstElement).ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [MemberData("ElementEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void NotEqualsOperatorShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> firstElement,
            SingleElementResult<object> secondElement,
            bool expected)
        {
            (firstElement != secondElement).ShouldBeEquivalentTo(!expected);
            (secondElement != firstElement).ShouldBeEquivalentTo(!expected);
        }

        public class SingleElementResultTestsTheories
        {
            public static IEnumerable ElementEqualityTheory = new object[]
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

            public static IEnumerable ValueEqualityTheory = new object[]
                                                                {
                                                                    new object[]
                                                                        { new SingleElementResult<object>(1), 1, true },
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
                                                                        { new SingleElementResult<object>(), 2, false },
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

            public static IEnumerable ObjectSingleElementEqualityTheory = new object[]
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

            public static IEnumerable ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory = new[]
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

            public static IEnumerable ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDoneTheory =
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

            public static IEnumerable ToStringShouldReturnStringRepresentationOfTheValueTheory =
                new[] { new object[] { 1, "1" }, new object[] { null, "Exists" } };
        }

        [Theory]
        [MemberData(
            "ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDoneTheory",
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void ExplicitOperatorFromSingleElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDone(
            SingleElementResult<string> element,
            string expectedMessage)
        {
            Action explicitCast = () => ((string)element).GetType();

            explicitCast.ShouldThrow<InvalidCastException>().WithMessage(expectedMessage);
        }

        [Theory]
        [MemberData(
            "ToStringShouldReturnStringRepresentationOfTheValueTheory",
            MemberType = typeof(SingleElementResultTestsTheories))]
        public void ToStringShouldReturnStringRepresentationOfTheValue(
            object value,
            string expectedString) => new SingleElementResult<object>(value).ToString()
            .ShouldBeEquivalentTo(expectedString);

        [Fact]
        public void ExplicitOperatorFromSingleElementToGenericTypeShouldReturnGenericTypeObjectFact()
        {
            var element = new SingleElementResult<string>("test");
            var @string = (string)element;

            @string.Should().IsSameOrEqualTo(element);
        }

        [Fact]
        public void ImplicitOperatorFromGenericTypeToSingleElementShouldReturnSingleElementWrappingGenericTypeObjectFact()
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
        public void ToStringShouldReturnStringRepresentationOfTheElementWhenElementDoesNotExist() => SingleElementResult<object>
            .NoElements.ToString()
            .ShouldBeEquivalentTo("Does not exist");

        [Fact]
        public void ToStringShouldReturnStringRepresentationOfTheElementWhenMultipleElementsExist() => SingleElementResult<object>
            .MultipleElements.ToString()
            .ShouldBeEquivalentTo("Multiple");

        [Fact]
        public void ValueConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(2);

            result.Value.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void ValueOrShouldReturnDefaultValueWhenNoneExists() => new SingleElementResult<int>().ValueOr(() => 3)
            .ShouldBeEquivalentTo(3);

        [Fact]
        public void ValueOrShouldReturnValueWhenOneExists() => new SingleElementResult<int>(3).ValueOr(null)
            .ShouldBeEquivalentTo(3);

        [Fact]
        public void ValueOrShouldThrowArgumentNullExceptionWhenDefaultSelectorIsNull()
        {
            Action valueOr = () => new SingleElementResult<int>().ValueOr(null);

            valueOr.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("default");
        }

        [Fact]
        public void ValueOrShouldThrowInvalidOperationExceptionWhenMultipleElementsExist()
        {
            var result = SingleElementResult<int>.MultipleElements;

            Action value = () => result.ValueOr(null);

            value.ShouldThrow<InvalidOperationException>().WithMessage("Multiple elements found.");
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

        [Fact]
        public void ValueShouldThrowInvalidOperationExceptionWhenNoElementsExist()
        {
            var result = SingleElementResult<int>.MultipleElements;

            Action value = () => SingleElementResult<int>.NoElements.Value.ToString();

            value.ShouldThrow<InvalidOperationException>().WithMessage("Element does not exist.");
        }
    }
}