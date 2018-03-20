using System;
using FluentAssertions;
using NSubstitute;
using Omego.Extensions.OptionExtensions;
using Xunit;
using Option = Optional.Option;

namespace Omego.Extensions.Tests.Unit.OptionExtensions
{
    public class OptionTests
    {
        [Fact]
        public void MatchNoneContinueShouldExecuteActionWithTheSameValue()
        {
            var option = Option.None<object>();

            var action = Substitute.For<Action>();

            option.MatchNoneContinue(action);

            action.Received()();
        }

        [Fact]
        public void MatchNoneContinueShouldReturnTheSameOptionValue()
        {
            var option = Option.None<object>();

            option.MatchNoneContinue(Substitute.For<Action>()).Should().BeEquivalentTo(option);
        }

        [Fact]
        public void MatchSomeContinueShouldExecuteActionWithTheSameValue()
        {
            var testValue = new object();

            var option = Option.Some(testValue);

            var action = Substitute.For<Action<object>>();

            option.MatchSomeContinue(action);

            action.Received()(testValue);
        }

        [Fact]
        public void MatchSomeContinueShouldReturnTheSameOptionValue()
        {
            var option = Option.Some(default(object));

            option.MatchSomeContinue(Substitute.For<Action<object>>()).Should().BeEquivalentTo(option);
        }
    }
}