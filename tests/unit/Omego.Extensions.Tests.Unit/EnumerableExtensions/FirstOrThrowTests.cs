using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Omego.Extensions.EnumerableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    public class FirstOrThrowTests
    {
        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.FirstOrThrow(x => x == 1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.FirstOrThrow((Exception) null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IEnumerable<int>) null).FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNull()
        {
            var enumerable = new object[0];

            Action firstOrThrow = () => enumerable.FirstOrThrow((Exception) null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action firstOrThrow = () => new object[0].FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new[] {1}.FirstOrThrow(null, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var enumerable = new[] {1};

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => enumerable.FirstOrThrow(x => x == 0, ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
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
        public void FirstOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] {1};

            enumerable.FirstOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IEnumerable<int>) null).FirstOrThrow(x => false);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new int[0].FirstOrThrow((Expression<Func<int, bool>>) null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action firstOrThrow = () => new[] {1}.FirstOrThrow(x => x == 0);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("No matches found for: (x == 0)");
        }
    }
}