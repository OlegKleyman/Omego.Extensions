namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Xunit;

    public class AttemptCatchIteratorTests
    {
        [Fact]
        public void ConstructorShouldThrowExceptionWhenRequiredArgumentsAreNull()
        {
            Action constructor = () => new AttemptCatchIterator<int>(null);

            constructor.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }
    }
}
