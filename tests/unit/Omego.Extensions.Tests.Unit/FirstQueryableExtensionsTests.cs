﻿namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Xunit;

    public class FirstQueryableExtensionsTests
    {
        [Fact]
        public void FirstElementShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstElement(x => x == 1).Value.Should().Be(1);
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => new int[0].AsQueryable().FirstElement(null);

            firstElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => ((IQueryable<int>)null).FirstElement(null);

            firstElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { "1" };

            queryable.FirstOr(x => x == "1", null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var queryable = new string[0];

            queryable.FirstOr("3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var queryable = new[] { "1" };

            queryable.FirstOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnTheFirstElementWhenExists()
        {
            var queryable = new[] { "1" };

            queryable.FirstOr(null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr(null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<string>)null).FirstOr(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOr = () => new string[] { null }.FirstOr(null, null);

            firstOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstOrThrow(x => x == 1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstOrThrow((Exception)null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IQueryable<int>)null).FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action firstOrThrow = () => new object[0].AsQueryable().FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("exception");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => new[] { 1 }.AsQueryable().FirstOrThrow(null, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(x => x == 0, ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var queryable = new object[0].AsQueryable();

            var ex = new InvalidOperationException();

            Action firstOrThrow = () => queryable.FirstOrThrow(ex);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.FirstOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action firstOrThrow = () => new int[0].AsQueryable().FirstOrThrow((Expression<Func<int, bool>>)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action firstOrThrow = () => new[] { 1 }.AsQueryable().FirstOrThrow(x => x == 0);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("No matches found for: (x == 0)");
        }
    }
}