namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    [CLSCompliant(false)]
    public class AttemptCatchIteratorTests
    {
        public static IEnumerable MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory =
            new object[]
                {
                    new object[] { 1, new[] { 1 }, true }, new object[] { 2, new[] { 1 }, false },
                    new object[] { 2, new[] { 0, 1 }, true }
                };

        public static IEnumerable MoveNextShouldUseHandlerIfAnExceptionOccursTheory = new object[]
                                                                                          {
                                                                                              new object[]
                                                                                                  {
                                                                                                      1, new[] { 1 },
                                                                                                      false
                                                                                                  },
                                                                                              new object[]
                                                                                                  {
                                                                                                      1, new[] { 0 }, true
                                                                                                  }
                                                                                          };

        [Theory]
        [MemberData("MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory")]
        public void MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessful(
            int times,
            IEnumerable<int> enumerable,
            bool expected)
        {
            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(
                enumerable.Select(i => 1 / i),
                exception => { });

            for (var i = 1; i < times; i++)
            {
                iterator.MoveNext();
            }

            iterator.MoveNext().ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [MemberData("MoveNextShouldUseHandlerIfAnExceptionOccursTheory")]
        public void MoveNextShouldUseHandlerIfAnExceptionOccurs(int times, IEnumerable<int> enumerable, bool expected)
        {
            var handled = false;

            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(
                enumerable.Select(i => 1 / i),
                t => handled = true);

            for (var i = 0; i < times; i++)
            {
                iterator.MoveNext();
            }

            handled.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNull()
        {
            Action constructor = () => new AttemptCatchIterator<int, Exception>(null, exception => { });

            constructor.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }
    }
}
