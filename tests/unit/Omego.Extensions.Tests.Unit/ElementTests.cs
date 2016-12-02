namespace Omego.Extensions.Tests.Unit
{
    using FluentAssertions;

    using Xunit;

    public class ElementTests
    {
        [Fact]
        public void ConstructorShouldCreateElementWithValue()
        {
            var element = new Element<string>("test");

            element.Present.Should().BeTrue();
            element.Value.ShouldBeEquivalentTo("test");
        }
    }
}
