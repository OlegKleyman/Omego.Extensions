﻿using System;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class AttemptCatchTests
    {
        [Fact]
        public void AttemptCatchShouldCatchException()
        {
            var enumerable = new[] {0}.Select(i => 1 / i);

            var handled = false;

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => handled = true).ToList();
            handled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldHandleAllRequestedExceptionsWhenCalledOnAnotherCatch()
        {
            var enumerable = new[] {-1, 0, 1}.Select(i => i == 1 ? throw new InvalidOperationException() : 1 / i);

            var dividedByZeroExceptionHandled = false;
            var invalidOperationExceptionHandled = false;

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => dividedByZeroExceptionHandled = true)
                .AttemptCatch<int, InvalidOperationException>(ex => invalidOperationExceptionHandled = true)
                .ToList();

            dividedByZeroExceptionHandled.Should().BeTrue();
            invalidOperationExceptionHandled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldReturnElementsWhereTheCaughtExceptionHasNotOccurred()
        {
            var enumerable = new[] {-1, 0, 1}.Select(i => 1 / i);
            enumerable.AttemptCatch<int, DivideByZeroException>(e => { }).Should().BeEquivalentTo(new[] {-1, 1});
        }
    }
}