using System;
using System.Linq;
using FluentAssertions;
using Omego.Extensions.QueryableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    public class FirstElementTests
    {
        [Fact]
        public void FirstElementShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.FirstElement(x => x == 1).ValueOr(null).Should().Be(1);
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => new int[0].AsQueryable().FirstElement(null);

            firstElement.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => ((IQueryable<int>) null).FirstElement(null);

            firstElement.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("queryable");
        }
    }
}