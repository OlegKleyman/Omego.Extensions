﻿namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Xunit;

    public class SingleQueryableExtensionsTests
    {
        [Fact]
        public void SingleOrThrowShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleOrThrow(x => x == 1, null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenMultipleMatchesFound()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(x => true, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(x => false, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => new int[0].AsQueryable().SingleOrThrow(null, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1 }.AsQueryable().SingleOrThrow(x => x == 0, ex, null);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(x => x == 1, null, ex);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleOrThrow(null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenMultipleMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenNoMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => Enumerable.Empty<object>().AsQueryable().SingleOrThrow(ex, null);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(null, ex);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }
    }
}