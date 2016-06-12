namespace Omego.Extensions.Tests.Unit
{
    using System;

    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    using Enumerable = Omego.Extensions.Enumerable;

    [CLSCompliant(false)]
    public class AttemptCatchIteratorTests
    {
        [Theory]
        [MemberData("MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory", null,
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
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
        [MemberData("MoveNextShouldUseHandlerIfAnExceptionOccursTheory", null,
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
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
        public void MoveNextShouldNotUseHandlerIfAnUnspecifiedExceptionOccurs()
        {
            var iterator = new AttemptCatchIterator<int, InvalidOperationException>(
                new[] { 0 }.Select(i => 1 / i),
                t => { });

            Action moveNext = () => iterator.MoveNext();

            moveNext.ShouldThrow<DivideByZeroException>();
        }

        [Theory]
        [MemberData("ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNullTheory", null,
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
        public void ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNull(IEnumerable<int> enumerable, Action<Exception> handler, string paramName)
        {
            Action constructor = () => new AttemptCatchIterator<int, Exception>(enumerable, handler);

            constructor.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo(paramName);
        }

        [Fact]
        public void ResetShouldThrowNotSupportedException()
        {
            var iterator = new AttemptCatchIterator<int, Exception>(new int[0], exception => { });

            Action reset = () => iterator.Reset();
            reset.ShouldThrow<NotSupportedException>();
        }

        [Fact]
        public void MoveNextShouldSetCurrent()
        {
            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(new[] { 1 }, t => { });

            iterator.MoveNext();
            iterator.Current.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public void DisposeShouldSetCurrentToDefault()
        {
            var iterator = new AttemptCatchIterator<string, DivideByZeroException>(new[] { String.Empty }, t => { });

            iterator.MoveNext();

            iterator.Dispose();

            iterator.Current.ShouldBeEquivalentTo(default(string));
        }

        [Fact]
        public void MoveNextShouldReturnFalseWhenDisposed()
        {
            var iterator = new AttemptCatchIterator<string, DivideByZeroException>(new[] { String.Empty }, t => { });
            
            iterator.Dispose();

            iterator.MoveNext().Should().BeFalse();
        }

        [Theory]
        [MemberData("ExceptionOccuredShouldReturnWhetherAnExceptionHasOccuredInTheLastIterationTheory", null, MemberType = typeof(AttemptCatchIteratorTestTheories))]
        public void ExceptionOccuredShouldReturnWhetherAnExceptionHasOccuredInTheLastIteration(IEnumerable<int> enumerable, bool expected)
        {
            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(enumerable.Select(i => 1/i), t => { });

            iterator.MoveNext();
            iterator.ExceptionOccured.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ExceptionOccuredShouldBeFalseAfterAnIterationHasAnExceptionButTheNextIterationDoesnt()
        {
            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(new []{0,1}.Select(i => 1/i), t => { });

            iterator.MoveNext();
            iterator.MoveNext();

            iterator.ExceptionOccured.Should().BeFalse();
        }

        public class AttemptCatchIteratorTestTheories
        {
            public static IEnumerable ExceptionOccuredShouldReturnWhetherAnExceptionHasOccuredInTheLastIterationTheory =
                new object[] { new object[] { System.Linq.Enumerable.Range(0, 1), true }, new object[] { System.Linq.Enumerable.Range(1, 1), false } };

            public static IEnumerable ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNullTheory = new object[]
                                                                                                              {
                                                                                                                  new object
                                                                                                                      []
                                                                                                                      {
                                                                                                                          null,
                                                                                                                          null,
                                                                                                                          "enumerable"
                                                                                                                      },
                                                                                                                  new object
                                                                                                                      []
                                                                                                                      {
                                                                                                                          new int[0],
                                                                                                                          null,
                                                                                                                          "handler"
                                                                                                                      }
                                                                                                              };

            public static IEnumerable MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory =
                new object[]
                    {
                        new object[] { 1, new[] { 1 }, true }, new object[] { 2, new[] { 1 }, false },
                        new object[] { 2, new[] { 0, 1 }, true }, new object[] {1, new int[0], false}
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
                                                                                                          1, new[] { 0 },
                                                                                                          true
                                                                                                      }
                                                                                              };
        }
    }
}
