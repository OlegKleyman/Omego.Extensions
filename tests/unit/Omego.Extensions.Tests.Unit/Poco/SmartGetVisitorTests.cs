using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.Poco
{
    public class SmartGetVisitorTests
    {
        public class SmartGetVisitorTestsTheories
        {
            public static IEnumerable<object[]> OnNullShouldSetNullQualifiedNameToNullForPropertiesTheory = new[]
            {
                new object[]
                {
                    new Test
                    {
                        Test1 =
                            new Test1
                            {
                                Test2 =
                                    new Test2()
                            }
                    },
                    (Expression<Func<Test, object>>) (test => test
                        .Test1
                        .Test2
                        .TestString),
                    "Test1.Test2.TestString"
                },
                new object[]
                {
                    new Test
                    {
                        Test1 =
                            new Test1()
                    },
                    (Expression<Func<Test, object>>) (test => test
                        .Test1
                        .Test2
                        .TestString),
                    "Test1.Test2"
                },
                new object[]
                {
                    new Test
                    {
                        Test1 =
                            new Test1()
                    },
                    (Expression<Func<Test, object>>) (test => test
                        .Test1
                        .Test2Field),
                    "Test1.Test2Field"
                }
            };

            public static IEnumerable<object[]> OnNullShouldSetCurrentObjectTheory = new[]
            {
                new object[]
                {
                    new Test
                    {
                        Test1 =
                            new Test1
                            {
                                Test2 =
                                    new Test2
                                    {
                                        TestString =
                                            "testing2"
                                    }
                            }
                    },
                    (Expression<Func<Test, object>>) (test => test
                        .Test1
                        .Test2
                        .TestString),
                    "testing2"
                }
            };

            public static IEnumerable<object[]> OnNullShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory
                = new[]
                {
                    new object[]
                    {
                        "Value cannot be null.\r\nParameter name: onNullCallBack",
                        "onNullCallBack",
                        typeof(ArgumentNullException),
                        new Test
                        {
                            Test1 =
                                new Test1
                                {
                                    Test2 =
                                        new Test2()
                                }
                        },
                        (Expression<Func<Test, object>>) (test => test
                            .Test1
                            .Test2
                            .TestString),
                        null
                    }
                };

            public static IEnumerable<object[]>
                VisitMemberShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory =
                    new[]
                    {
                        new object[]
                        {
                            "Value cannot be null.\r\nParameter name: node", "node", typeof(ArgumentNullException),
                            null
                        }
                    };

            public static IEnumerable<object[]>
                ResetWithShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory =
                    new[]
                    {
                        new object[]
                        {
                            "Value cannot be null.\r\nParameter name: target", "target",
                            typeof(ArgumentNullException), null
                        }
                    };

            public static IEnumerable<object[]>
                ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory =
                    new[]
                    {
                        new object[]
                        {
                            "Value cannot be null.\r\nParameter name: target", "target",
                            typeof(ArgumentNullException), null
                        }
                    };
        }

        [Theory]
        [MemberData(
            nameof(SmartGetVisitorTestsTheories.OnNullShouldSetNullQualifiedNameToNullForPropertiesTheory),
            MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldSetNullQualifiedNameToNullForProperties(
            Test target,
            Expression expression,
            string expected)
        {
            var visitor = GetVisitor(target);

            var handler = Substitute.For<Action<string>>();

            visitor.OnNull(expression, handler);

            handler.Received(1)(Arg.Is(expected));
        }

        [Theory]
        [MemberData(nameof(SmartGetVisitorTestsTheories.OnNullShouldSetCurrentObjectTheory), MemberType =
            typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldSetCurrentObject(Test target, Expression expression, string expected)
        {
            var visitor = GetVisitor(target);

            visitor.OnNull(expression, null);

            visitor.Current.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(
            nameof(SmartGetVisitorTestsTheories
                .OnNullShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory),
            MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            object target,
            Expression expression,
            Action<string> callBack)
        {
            var visitor = GetVisitor(target);

            Action onNull = () => visitor.OnNull(expression, callBack);

            onNull.Should().Throw<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        [Theory]
        [MemberData(
            nameof(SmartGetVisitorTestsTheories
                .VisitMemberShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory),
            MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void VisitMemberShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            MemberExpression expression)
        {
            var visitor = new MockVisitor(new object());

            Action visitMember = () => visitor.VisitMember(expression);

            visitMember.Should().Throw<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        [Theory]
        [MemberData(
            nameof(SmartGetVisitorTestsTheories
                .ResetWithShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory),
            MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void ResetWithShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            object target)
        {
            var visitor = GetVisitor(new object());

            Action resetWith = () => visitor.ResetWith(target);

            resetWith.Should().Throw<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        [Theory]
        [MemberData(
            nameof(SmartGetVisitorTestsTheories
                .ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory),
            MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void ConstructorShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            object target)
        {
            Action resetWith = () => new SmartGetVisitor(target);

            resetWith.Should().Throw<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        private SmartGetVisitor GetVisitor(object target)
        {
            return new SmartGetVisitor(target);
        }

        public class Test
        {
            internal Test1 Test1 { get; set; }
        }

        public class Test1
        {
            public Test2 Test2Field;

            internal Test2 Test2 { get; set; }
        }

        public class Test2
        {
            internal int Test2Field;

            internal string TestString { get; set; }
        }

        public class MockVisitor : SmartGetVisitor
        {
            public MockVisitor(object target)
                : base(target)
            {
            }

            public new Expression VisitMember(MemberExpression node)
            {
                return base.VisitMember(node);
            }
        }

        [Fact]
        public void ResetWithShouldClearQualifyingPath()
        {
            var visitor = new MockVisitor(new Test2 {TestString = "testing"});

            Expression<Func<Test2, string>> expression = test2 => test2.TestString;

            visitor.VisitMember((MemberExpression) expression.Body);

            var secondTarget = new Test {Test1 = new Test1 {Test2 = new Test2()}};

            visitor.ResetWith(secondTarget);

            var handler = Substitute.For<Action<string>>();

            Expression<Func<Test, string>> secondExpression = test => test.Test1.Test2.TestString;

            visitor.OnNull(secondExpression, handler);

            handler.Received(1)(Arg.Is("Test1.Test2.TestString"));
        }

        [Fact]
        public void ResetWithShouldResetCurrentWithTargetObject()
        {
            var visitor = GetVisitor(new object());

            var target = new object();

            visitor.ResetWith(target);

            visitor.Current.Should().Be(target);
        }

        [Fact]
        public void VisitMemberShouldReturnNodeWhenCurrentIsNotNull()
        {
            var visitor = new MockVisitor(new Test2 {TestString = "testing"});

            Expression<Func<Test2, string>> expression = test2 => test2.TestString;

            visitor.VisitMember((MemberExpression) expression.Body).Should().BeEquivalentTo(expression.Body);
        }

        [Fact]
        public void VisitMemberShouldReturnNodeWhenCurrentIsNull()
        {
            var visitor = new MockVisitor(new Test2());
            Expression<Func<Test2, string>> expression = test2 => test2.TestString;

            var memberExpression = (MemberExpression) visitor.VisitMember((MemberExpression) expression.Body);

            visitor.Current.Should().BeNull();
            visitor.VisitMember(memberExpression).Should().BeEquivalentTo(expression.Body);
        }

        [Fact]
        public void VisitMemberShouldSetCurrentToFieldValue()
        {
            var visitor = new MockVisitor(new Test2 {Test2Field = 20});

            Expression<Func<Test2, int>> expression = test2 => test2.Test2Field;

            visitor.VisitMember((MemberExpression) expression.Body);
            visitor.Current.Should().BeEquivalentTo(20);
        }

        [Fact]
        public void VisitMemberShouldSetCurrentToPropertyValue()
        {
            var visitor = new MockVisitor(new Test2 {TestString = "testing"});

            Expression<Func<Test2, string>> expression = test2 => test2.TestString;

            visitor.VisitMember((MemberExpression) expression.Body);
            visitor.Current.Should().BeEquivalentTo("testing");
        }
    }
}