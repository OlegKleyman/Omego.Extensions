using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Omego.Extensions.NullExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.NullExtensions
{
    public class SmartGetTests
    {
        public class Test
        {
            internal Test2 Test2 { get; set; }
        }

        public class Test2
        {
            internal Test3 Test3;
        }

        public class Test3
        {
            internal string Something;
        }

        [Theory]
        [MemberData(
            nameof(SmartGetTestsTheories.SmartGetShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory),
            MemberType = typeof(SmartGetTestsTheories))]
        public void SmartGetShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalid(
            string message,
            string parameterName,
            Type exceptionType,
            Test target,
            Expression<Func<Test, Test3>> nullCheckExpression,
            Func<Test3, object> returnExpression,
            Func<string, Exception> onNullException)
        {
            Action smartGet = () => target.SmartGet(nullCheckExpression, returnExpression, onNullException);

            smartGet.Should().Throw<ArgumentException>()
                .WithMessage(message)
                .Where(
                    exception => exception.ParamName == parameterName,
                    "the parameter name should be of the problematic parameter")
                .And.Should()
                .BeOfType(exceptionType);
        }

        public class SmartGetTestsTheories
        {
            public static IEnumerable<object[]>
                SmartGetShouldThrowArgumentExceptionWhenRequiredArgumentsAreInvalidTheory =
                    new[]
                    {
                        new object[]
                        {
                            "Value cannot be null.\r\nParameter name: result", "result",
                            typeof(ArgumentNullException), new Test(), null, null, null
                        },
                        new object[]
                        {
                            "Value cannot be null.\r\nParameter name: exception", "exception",
                            typeof(ArgumentNullException), new Test(),
                            (Expression<Func<Test, Test3>>) (test => test.Test2.Test3), null, null
                        }
                    };
        }

        [Fact]
        public void SmartGetShouldReturnResultIfNothingIsNull()
        {
            var target = new Test {Test2 = new Test2 {Test3 = new Test3 {Something = "something"}}};

            target.SmartGet(test => test.Test2.Test3, test3 => test3.Something, null).Should().BeEquivalentTo("something");
        }

        [Fact]
        public void SmartGetShouldThrowExceptionFromCallBackWhenSomethingIsNull()
        {
            var target = new Test {Test2 = new Test2()};

            Action smartGet = () => target.SmartGet(
                test => test.Test2.Test3,
                test3 => test3.Something,
                s => new InvalidOperationException(s));

            smartGet.Should().Throw<InvalidOperationException>().WithMessage("Test2.Test3");
        }

        [Fact]
        public void SmartGetShouldThrowInvalidOperationExceptionWhenExceptionRetrieverFunctionReturnsNull()
        {
            var target = new Test {Test2 = new Test2()};

            Action smartGet = () => target.SmartGet(test => test.Test2.Test3, test3 => test3.Something, s => null);

            smartGet.Should().Throw<InvalidOperationException>().WithMessage("Exception to throw returned null.");
        }
    }
}