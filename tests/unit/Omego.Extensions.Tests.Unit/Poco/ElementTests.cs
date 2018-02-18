using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.Poco
{
    public class ElementTests
    {
        [Theory]
        [MemberData(nameof(ElementTestsTheories.ElementEqualityTheory), MemberType = typeof(ElementTestsTheories))]
        public void EqualsShouldReturnWhetherElementsAreEqual(
            Element<object> firstElement,
            Element<object> secondElement,
            bool expected)
        {
            firstElement.Equals(secondElement).Should().Be(expected);
            secondElement.Equals(firstElement).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ElementTestsTheories.ValueEqualityTheory), MemberType = typeof(ElementTestsTheories))]
        public void EqualsShouldReturnWhetherElementValuesAreEqual(Element<object> element, object value, bool expected)
        {
            element.Equals(value).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ElementTestsTheories.ObjectElementEqualityTheory), MemberType =
            typeof(ElementTestsTheories))]
        public void ObjectEqualsShouldReturnWhetherElementsAreEqual(
            Element<object> element,
            object value,
            bool expected)
        {
            ((object) element).Equals(value).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ElementTestsTheories.ObjectGetHashCodeShouldReturnElementHashCodeTheory), MemberType =
            typeof(ElementTestsTheories))]
        public void ObjectGetHashCodeShouldReturnElementHashCode(Element<object> element, int expected)
        {
            element
                .GetHashCode().Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ElementTestsTheories.ElementEqualityTheory), MemberType = typeof(ElementTestsTheories))]
        public void EqualsOperatorForElementShouldReturnWhetherElementsAreEqual(
            Element<object> firstElement,
            Element<object> secondElement,
            bool expected)
        {
            (firstElement == secondElement).Should().Be(expected);
            (secondElement == firstElement).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ElementTestsTheories.ElementEqualityTheory), MemberType = typeof(ElementTestsTheories))]
        public void NotEqualsOperatorForElementShouldReturnWhetherElementsAreEqual(
            Element<object> firstElement,
            Element<object> secondElement,
            bool expected)
        {
            (firstElement != secondElement).Should().Be(!expected);
            (secondElement != firstElement).Should().Be(!expected);
        }

        public class ElementTestsTheories
        {
            public static IEnumerable<object[]> ElementEqualityTheory =
                new[]
                {
                    new object[] {new Element<object>(1), new Element<object>(1), true},
                    new object[] {new Element<object>(1), new Element<object>(2), false},
                    new object[] {new Element<object>(null), new Element<object>(2), false},
                    new object[] {new Element<object>(null), new Element<object>(null), true},
                    new object[] {new Element<object>(), new Element<object>(2), false},
                    new object[] {new Element<object>(), new Element<object>(), true}
                };

            public static IEnumerable<object[]> ValueEqualityTheory =
                new[]
                {
                    new object[] {new Element<object>(1), 1, true},
                    new object[] {new Element<object>(1), 2, false},
                    new object[] {new Element<object>(null), 2, false},
                    new object[] {new Element<object>(null), null, true},
                    new object[] {new Element<object>(), 2, false},
                    new object[] {new Element<object>(), null, false}
                };

            public static IEnumerable<object[]> ObjectElementEqualityTheory =
                new[]
                {
                    new object[] {new Element<object>(1), new Element<object>(1), true},
                    new object[] {new Element<object>(1), new Element<object>(2), false},
                    new object[] {new Element<object>(null), new Element<object>(2), false},
                    new object[] {new Element<object>(null), new Element<object>(null), true},
                    new object[] {new Element<object>(), new Element<object>(2), false},
                    new object[] {new Element<object>(), new Element<object>(), true},
                    new object[] {new Element<object>(), 1, false},
                    new object[] {new Element<object>("test"), "test", true}
                };

            public static IEnumerable<object[]> ObjectGetHashCodeShouldReturnElementHashCodeTheory =
                new[]
                {
                    new object[] {new Element<object>(), 0}, new object[] {new Element<object>(1), 194},
                    new object[] {new Element<object>(0), 193}, new object[] {new Element<object>(null), 1}
                };

            public static IEnumerable<object[]> ToStringShouldReturnStringRepresentationOfTheValueTheory =
                new[] {new object[] {1, "1"}, new object[] {null, "Exists"}};
        }

        [Theory]
        [MemberData(
            nameof(ElementTestsTheories.ToStringShouldReturnStringRepresentationOfTheValueTheory),
            MemberType = typeof(ElementTestsTheories))]
        public void ToStringShouldReturnStringRepresentationOfTheValue(object value, string expectedString)
        {
            new Element<object>(value).ToString().Should().BeEquivalentTo(expectedString);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void WhenPresentShouldExecuteActionOnlyWhenElementIsPresent(bool present, bool expected)
        {
            var element = present ? new Element<object>(default) : new Element<object>();

            var actionExecuted = false;

            element.WhenPresent(value => actionExecuted = true);

            actionExecuted.Should().Be(expected);
        }

        [Fact]
        public void ConstructorShouldCreateElementWithValue()
        {
            var element = new Element<string>("test");

            element.Present.Should().BeTrue();
            element.ValueOr(null).Should().BeEquivalentTo("test");
        }

        [Fact]
        public void ExplicitOperatorFromElementToGenericTypeShouldReturnGenericTypeObjectFact()
        {
            var element = new Element<string>("test");
            var @string = (string) element;

            @string.Should().IsSameOrEqualTo(element);
        }

        [Fact]
        public void ExplicitOperatorFromElementToGenericTypeShouldThrowInvalidCastExceptionWhenConversionCantBeDone()
        {
            Action explicitCast = () => ((string) new Element<string>()).GetType();

            explicitCast.Should().Throw<InvalidCastException>()
                .WithMessage("No element present to cast to System.String.");
        }

        [Fact]
        public void ImplicitOperatorFromGenericTypeToElementShouldReturnElementWrappingGenericTypeObjectFact()
        {
            Element<string> element = "test";

            element.Should().IsSameOrEqualTo("test");
        }

        [Fact]
        public void ToStringShouldReturnDoesNotExistWhenValueDoesNotExist()
        {
            new Element<object>().ToString()
                .Should().BeEquivalentTo("Does not exist");
        }

        [Fact]
        public void ValueOrShouldReturnDefaultValueWhenNoneExists()
        {
            new Element<int>().ValueOr(() => 3)
                .Should().Be(3);
        }

        [Fact]
        public void ValueOrShouldReturnValueWhenOneExists()
        {
            new Element<int>(3).ValueOr(null).Should().Be(3);
        }

        [Fact]
        public void ValueOrShouldThrowArgumentNullExceptionWhenDefaultSelectorIsNull()
        {
            Action value = () => new Element<int>().ValueOr(null);

            value.Should().Throw<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("default");
        }

        [Fact]
        public void WhenPresentShouldPassPresentValueToAction()
        {
            var target = new object();
            var element = new Element<object>(target);

            object expectedValue = null;

            element.WhenPresent(value => expectedValue = value);

            expectedValue.Should().BeSameAs(target);
        }

        [Fact]
        public void WhenPresentShouldThrowArgumentNullExceptionWhenActionArgumentIsNull()
        {
            Action whenPresent = () => new Element<int>(default).WhenPresent(null);

            whenPresent.Should().Throw<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo("action");
        }
    }
}