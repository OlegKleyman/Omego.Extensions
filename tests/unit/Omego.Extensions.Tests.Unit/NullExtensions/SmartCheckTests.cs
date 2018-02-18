using System;
using FluentAssertions;
using Omego.Extensions.NullExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.NullExtensions
{
    public class SmartCheckTests
    {
        public class SmartCheckTestsTheory
        {
        }

        [Fact]
        public void SmartCheckShouldNotThrowIfNothingIsNull()
        {
            var target = new {Test2 = new {Test3 = new {Something = "something"}}};

            Action smartCheck = () => target.SmartCheck(null, test => test.Test2.Test3);

            smartCheck.Should().NotThrow();
        }

        [Fact]
        public void SmartCheckShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNull()
        {
            var target = new {Test2 = new {Test3 = (string) null}};

            Action smartCheck = () => target.SmartCheck(null, t => t.Test2.Test3);

            smartCheck.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: exception")
                .And.ParamName.Should().BeEquivalentTo("exception");
        }

        [Fact]
        public void SmartCheckShouldThrowArgumentNullExceptionWhenQualifierPathArgumentIsNull()
        {
            Action smartCheck = () => string.Empty.SmartCheck(null, null);

            smartCheck.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: qualifierPath")
                .And.ParamName.Should().BeEquivalentTo("qualifierPath");
        }

        [Fact]
        public void SmartCheckShouldThrowExceptionFromCallBackWhenSecondQualifierExpressionIsNullIsNull()
        {
            var target = new {Test2 = new {Test3 = string.Empty}, Test4 = (string) null};

            Action smartCheck = () => target.SmartCheck(
                s => new InvalidOperationException(s),
                test => test.Test2.Test3,
                arg => arg.Test4);

            smartCheck.Should().Throw<InvalidOperationException>().WithMessage("Test4");
        }

        [Fact]
        public void SmartCheckShouldThrowExceptionFromCallBackWhenSomethingIsNull()
        {
            var target = new {Test2 = new {Test3 = (string) null}};

            Action smartCheck = () => target.SmartCheck(
                s => new InvalidOperationException(s),
                test => test.Test2.Test3);

            smartCheck.Should().Throw<InvalidOperationException>().WithMessage("Test2.Test3");
        }

        [Fact]
        public void SmartCheckShouldThrowInvalidOperationExceptionWhenExceptionRetrieverFunctionReturnsNull()
        {
            var target = new {Test2 = new {Test3 = (string) null}};

            Action smartCheck = () => target.SmartCheck(s => null, test => test.Test2.Test3);

            smartCheck.Should().Throw<InvalidOperationException>().WithMessage("Exception to throw returned null.");
        }
    }
}