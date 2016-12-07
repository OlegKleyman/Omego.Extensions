namespace Omego.Extensions.Tests.Unit.EnumerableExtensions
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Omego.Extensions.EnumerableExtensions;

    using Xunit;

    public class SingleOrTests
    {
        [Fact]
        public void SingleOrByQueryByQueryShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new object[2];

            Action singleOr = () => enumerable.SingleOr(o => o == null, (object)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for (o == null).");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(x => x == "1", (string)null).Should().Be("1");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearching()
        {
            Action singleOr = () => ((IEnumerable<string>)null).SingleOr(x => false, (string)null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearching()
        {
            Action singleOr = () => new string[] { null }.SingleOr(null, (string)null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrShouldReturnElementIfOneExists()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr((string)null).Should().Be("1");
        }

        [Fact]
        public void SingleOrShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var enumerable = new object[0];

            enumerable.SingleOr("3").Should().Be("3");
        }

        [Fact]
        public void SingleOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOr = () => ((IEnumerable<string>)null).SingleOr((string)null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new object[2];

            Action singleOr = () => enumerable.SingleOr((object)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for true.");
        }

        [Fact]
        public void SingleOrLazyByQueryShouldArgumentNullExceptionWhenDefaultFuncIsNull()
        {
            var enumerable = new[] { "1" };

            Action singleOr = () => enumerable.SingleOr(x => x == "2", (Func<string>)null);

            singleOr.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("default");
        }

        [Fact]
        public void SingleOrLazyByQueryShouldArgumentNullExceptionWhenPredicateIsNull()
        {
            var enumerable = new[] { "1" };

            Action singleOr = () => enumerable.SingleOr(null, (Func<string>)null);

            singleOr.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrLazyByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(x => x == "1", (Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void SingleOrLazyByQueryShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.SingleOr(s => false, () => "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrLazyByQueryShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(x => x == "2", () => "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrLazyShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr((Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void SingleOrLazyShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.SingleOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrLazyShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new string[0];

            enumerable.SingleOr(() => "3").Should().Be("3");
        }
    }
}
