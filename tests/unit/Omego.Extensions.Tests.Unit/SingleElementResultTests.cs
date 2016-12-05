namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections;

    using FluentAssertions;
    using FluentAssertions.Common;

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
            bool expected)
        {
            element.Equals(value).ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [MemberData("ObjectSingleElementEqualityTheory", MemberType = typeof(SingleElementResultTestsTheories))]
        public void ObjectEqualsShouldReturnWhetherSingleElementsAreEqual(
            SingleElementResult<object> element,
            object value,
            bool expected)
        {
            ((object)element).Equals(value).ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [MemberData("ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory",
             MemberType = typeof(SingleElementResultTestsTheories))]
        public void ObjectGetHashCodeShouldReturnSingleElementHashCode(
            SingleElementResult<object> element,
            int expected)
        {
            element.GetHashCode().ShouldBeEquivalentTo(expected);
        }

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
                                                                        {
                                                                            new SingleElementResult<object>(1), 1,
                                                                            true
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            new SingleElementResult<object>(1), 2,
                                                                            false
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            new SingleElementResult<object>(null), 2,
                                                                            false
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            new SingleElementResult<object>(null),
                                                                            null, true
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            new SingleElementResult<object>(), 2,
                                                                            false
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            SingleElementResult<object>.MultipleElements,
                                                                            null, false
                                                                        },
                                                                    new object[]
                                                                        {
                                                                            SingleElementResult<object>.MultipleElements,
                                                                            1, false
                                                                        }
                                                                };

            public static IEnumerable ObjectSingleElementEqualityTheory = new object[]
                                                                              {
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(1),
                                                                                          new SingleElementResult
                                                                                              <object>(1),
                                                                                          true
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(1),
                                                                                          new SingleElementResult
                                                                                              <object>(2),
                                                                                          false
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(null),
                                                                                          new SingleElementResult
                                                                                              <object>(2),
                                                                                          false
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(null),
                                                                                          new SingleElementResult
                                                                                              <object>(null),
                                                                                          true
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(),
                                                                                          new SingleElementResult
                                                                                              <object>(2),
                                                                                          false
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(),
                                                                                          new SingleElementResult
                                                                                              <object>(),
                                                                                          true
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>(),
                                                                                          1, false
                                                                                      },
                                                                                  new object[]
                                                                                      {
                                                                                          new SingleElementResult
                                                                                              <object>("test"),
                                                                                          "test",
                                                                                          true
                                                                                      }
                                                                              };

            public static IEnumerable ObjectGetHashCodeShouldReturnSingleElementHashCodeTheory = new[]
                                                                                                     {
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new SingleElementResult
                                                                                                                 <
                                                                                                                     object
                                                                                                                 >(),
                                                                                                                 0
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new SingleElementResult
                                                                                                                 <
                                                                                                                     object
                                                                                                                 >(1),
                                                                                                                 579
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new SingleElementResult
                                                                                                                 <
                                                                                                                     object
                                                                                                                 >(0),
                                                                                                                 386
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new SingleElementResult
                                                                                                                 <
                                                                                                                     object
                                                                                                                 >(null),
                                                                                                                 1
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 SingleElementResult
                                                                                                                     <
                                                                                                                         object
                                                                                                                     >
                                                                                                                     .MultipleElements,
                                                                                                                 2
                                                                                                             }
                                                                                                     };
        }

        [Fact]
        public void ExplicitOperatorFromSingleElementToGenericTypeShouldReturnGenericTypeObjectFact()
        {
            var element = new SingleElementResult<string>("test");
            var @string = (string)element;

            @string.Should().IsSameOrEqualTo(element);
        }

        [Fact]
        public void ImplicitOperatorFromGenericTypeToSingleElementShouldReturnSingleElementWrappingGenericTypeObjectFact
            ()
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
        public void ValueConstructorShouldSetProperties()
        {
            var result = new SingleElementResult<int>(2);

            result.Value.ShouldBeEquivalentTo(2);
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