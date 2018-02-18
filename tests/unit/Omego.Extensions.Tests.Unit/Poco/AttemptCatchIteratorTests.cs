using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.Poco;
using Xunit;

namespace Omego.Extensions.Tests.Unit.Poco
{
    public class AttemptCatchIteratorTests
    {
        [Theory]
        [MemberData(
            nameof(AttemptCatchIteratorTestTheories
                .MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory),
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
        public void MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessful(
            int times,
            IEnumerable<int> enumerable,
            bool expected)
        {
            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(
                enumerable.Select(i => 1 / i),
                exception => { });

            for (var i = 1; i < times; i++) iterator.MoveNext();

            iterator.MoveNext().Should().Be(expected);
        }

        [Theory]
        [MemberData(
            nameof(AttemptCatchIteratorTestTheories.MoveNextShouldUseHandlerIfAnExceptionOccursTheory),
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
        public void MoveNextShouldUseHandlerIfAnExceptionOccurs(int times, IEnumerable<int> enumerable, bool expected)
        {
            var handled = false;

            var iterator = new AttemptCatchIterator<int, DivideByZeroException>(
                enumerable.Select(i => 1 / i),
                t => handled = true);

            for (var i = 0; i < times; i++) iterator.MoveNext();

            handled.Should().Be(expected);
        }

        [Theory]
        [MemberData(
            nameof(AttemptCatchIteratorTestTheories.ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNullTheory),
            MemberType = typeof(AttemptCatchIteratorTestTheories))]
        public void ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNull(
            IEnumerable<int> enumerable,
            Action<Exception> handler,
            string paramName)
        {
            Action constructor = () => new AttemptCatchIterator<int, Exception>(enumerable, handler);

            constructor.Should().Throw<ArgumentNullException>().Which.ParamName.Should().BeEquivalentTo(paramName);
        }

        public class AttemptCatchIteratorTestTheories
        {
            public static IEnumerable<object[]> ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNullTheory =
                new[]
                {
                    new object[]
                    {
                        null,
                        null,
                        "enumerable"
                    },
                    new object[]
                    {
                        new int [0],
                        null,
                        "handler"
                    }
                };

            public static IEnumerable<object[]> MoveNextShouldReturnWhetherMovingToTheNextIterationWasSuccessfulTheory =
                new[]
                {
                    new object[] {1, new[] {1}, true}, new object[] {2, new[] {1}, false},
                    new object[] {1, new[] {0, 1}, true}, new object[] {1, new int[0], false}
                };

            public static IEnumerable<object[]> MoveNextShouldUseHandlerIfAnExceptionOccursTheory = new[]
            {
                new object[]
                {
                    1,
                    new[] {1},
                    false
                },
                new object[]
                {
                    1,
                    new[] {0},
                    true
                }
            };
        }

        [Fact]
        public void DisposeShouldSetCurrentToDefault()
        {
            var iterator =
                (IEnumerator) new AttemptCatchIterator<string, DivideByZeroException>(new[] {string.Empty}, t => { });

            iterator.MoveNext();

            ((IDisposable) iterator).Dispose();

            iterator.Current.Should().Be(default);
        }

        [Fact]
        public void MoveNextShouldNotUseHandlerIfAnUnspecifiedExceptionOccurs()
        {
            var iterator = new AttemptCatchIterator<int, InvalidOperationException>(
                new[] {0}.Select(i => 1 / i),
                t => { });

            Action moveNext = () => iterator.MoveNext();

            moveNext.Should().Throw<DivideByZeroException>();
        }

        [Fact]
        public void MoveNextShouldSetCurrent()
        {
            var iterator = (IEnumerator) new AttemptCatchIterator<int, DivideByZeroException>(new[] {1}, t => { });

            iterator.MoveNext();
            iterator.Current.Should().BeEquivalentTo(1);
        }

        [Fact]
        public void MoveNextShouldSetCurrentToDefaultWhenEnumerableDoesntMove()
        {
            var iterator =
                (IEnumerator) new AttemptCatchIterator<string, DivideByZeroException>(new string[0], exception => { });

            iterator.MoveNext();
            iterator.Current.Should().Be(default);
        }

        [Fact]
        public void MoveNextShouldThrowObjectDisposedExceptionWhenIteratorIsDisposed()
        {
            var iterator = new AttemptCatchIterator<int, Exception>(new int[0], exception => { });
            iterator.Dispose();

            Action moveNext = () => iterator.MoveNext();

            var typeName = typeof(AttemptCatchIterator<int, Exception>).FullName;

            moveNext.Should().Throw<ObjectDisposedException>()
                .WithMessage($"Cannot access a disposed object.\r\nObject name: '{typeName}'.")
                .And.ObjectName.Should().BeEquivalentTo(typeName);
        }

        [Fact]
        public void ResetShouldResetEnumeratorIfNotDisposed()
        {
            var iterator = (IEnumerator) new AttemptCatchIterator<int, DivideByZeroException>(new[] {1, 2}, t => { });

            iterator.MoveNext();
            iterator.MoveNext();

            iterator.Reset();

            iterator.MoveNext();

            iterator.Current.Should().BeEquivalentTo(1);
        }

        [Fact]
        public void ResetShouldThrowObjectDisposedExceptionWhenIteratorIsDisposed()
        {
            var iterator = new AttemptCatchIterator<int, Exception>(new int[0], exception => { });
            iterator.Dispose();

            Action reset = () => iterator.Reset();

            var typeName = typeof(AttemptCatchIterator<int, Exception>).FullName;

            reset.Should().Throw<ObjectDisposedException>()
                .WithMessage($"Cannot access a disposed object.\r\nObject name: '{typeName}'.")
                .And.ObjectName.Should().BeEquivalentTo(typeName);
        }
    }
}