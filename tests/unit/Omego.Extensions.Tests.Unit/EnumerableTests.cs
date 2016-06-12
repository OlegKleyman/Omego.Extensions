namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public class EnumerableTests
    {
        [Fact]
        public void CatchShouldCatchException()
        {
            var enumerable = new[] { 0 }.Select(i => 1 / i);

            var handled = false;

            enumerable.Catch<int, DivideByZeroException>(ex => handled = true).ToList();
            handled.Should().BeTrue();
        }

        [Fact]
        public void CatchShouldReturnElementsWhereTheCaughtExceptionHasNotOccurred()
        {
            var enumerable = new[] { -1, 0, 1 }.Select(i => 1 / i);
            enumerable.Catch<int, DivideByZeroException>(e => { }).ShouldAllBeEquivalentTo(new[] { -1, 1 });
        }

        [Fact]
        public void CatchShouldHandleAllRequestedExceptionsWhenCalledOnAnotherCatch()
        {
            var enumerable = new[] { -1, 0, 1 }.Select(
                i =>
                    {
                        if (i == 1)
                        {
                            throw new InvalidOperationException();
                        }

                        return 1 / i;
                    });

            var dividedByZeroExceptionHandled = false;
            var invalidOperationExceptionHandled = false;

            enumerable.Catch<int, DivideByZeroException>(ex => dividedByZeroExceptionHandled = true)
                .Catch<int, InvalidOperationException>(ex => invalidOperationExceptionHandled = true).ToList();

            dividedByZeroExceptionHandled.Should().BeTrue();
            invalidOperationExceptionHandled.Should().BeTrue();
        }
    }
}
