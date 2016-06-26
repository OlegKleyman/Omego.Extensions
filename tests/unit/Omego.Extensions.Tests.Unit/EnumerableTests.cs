namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public class EnumerableTests
    {
        [Fact]
        public void AttemptCatchShouldCatchException()
        {
            var enumerable = new[] { 0 }.Select(i => 1 / i);

            var handled = false;

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => handled = true).ToList();
            handled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldHandleAllRequestedExceptionsWhenCalledOnAnotherCatch()
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

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => dividedByZeroExceptionHandled = true)
                .AttemptCatch<int, InvalidOperationException>(ex => invalidOperationExceptionHandled = true)
                .ToList();

            dividedByZeroExceptionHandled.Should().BeTrue();
            invalidOperationExceptionHandled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldReturnElementsWhereTheCaughtExceptionHasNotOccurred()
        {
            var enumerable = new[] { -1, 0, 1 }.Select(i => 1 / i);
            enumerable.AttemptCatch<int, DivideByZeroException>(e => { }).ShouldAllBeEquivalentTo(new[] { -1, 1 });
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var enumerable = new object[0];

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => enumerable.FirstOrThrow(ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var enumerable = new [] {1};

            enumerable.FirstOrThrow(null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNull()
        {
            var enumerable = new object[0];

            Action firstOrThrow = () => enumerable.FirstOrThrow(null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.FirstOrThrow(x => x==1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var enumerable = new[] {1};

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => enumerable.FirstOrThrow(x => x == 0, ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }
    }
}