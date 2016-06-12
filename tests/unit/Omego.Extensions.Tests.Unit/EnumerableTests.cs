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
    }
}
