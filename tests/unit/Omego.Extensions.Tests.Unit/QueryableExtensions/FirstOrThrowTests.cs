﻿using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Omego.Extensions.QueryableExtensions;
using Xunit;

namespace Omego.Extensions.Tests.Unit.QueryableExtensions
{
    public class FirstOrThrowTests
    {
        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.FirstOrThrow(x => x == 1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.FirstOrThrow((Exception) null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<int>) null).FirstOrThrow(x => false, null);

            firstOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action firstOrThrow = () => new object[0].AsQueryable().FirstOrThrow(x => false, null);

            firstOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new[] {1}.AsQueryable().FirstOrThrow(null, null);

            firstOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var queryable = new[] {1}.AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(x => x == 0, ex);

            firstOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var queryable = new object[0].AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(ex);

            firstOrThrow.Should().ThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] {1}.AsQueryable();

            queryable.FirstOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new int[0].AsQueryable().FirstOrThrow((Expression<Func<int, bool>>) null);

            firstOrThrow.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should()
                .BeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action firstOrThrow = () => new[] {1}.AsQueryable().FirstOrThrow(x => x == 0);

            firstOrThrow.Should().ThrowExactly<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("No matches found for: (x == 0)");
        }
    }
}