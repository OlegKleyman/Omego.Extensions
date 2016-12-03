﻿namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public partial class EnumerableTests
    {
        [Fact]
        public void SingleElementByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.SingleElement(x => x == 1).Value.Should().Be(1);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnMultipleMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new[] { 1, 2 };

            enumerable.SingleElement(x => true).Elements.Should().Be(Elements.Multiple);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new int[0];

            enumerable.SingleElement(x => false).Elements.Should().Be(Elements.None);
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElement = () => ((IEnumerable<int>)null).SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElement = () => new int[0].SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.SingleElementOrThrowOnMultiple(x => x == 1, null).Value.Should().Be(1);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var enumerable = new int[0];

            enumerable.SingleElementOrThrowOnMultiple(x => false, null).Elements.Should().Be(Elements.None);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new[] { 1, 2 };

            var ex = new InvalidOperationException();

            Action singleElementOrThrowOnMultiple = () => enumerable.SingleElementOrThrowOnMultiple(i => true, ex);

            singleElementOrThrowOnMultiple.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsExceptionIsNull()
        {
            var enumerable = new[] { 1, 2 };

            Action singleElementOrThrowOnMultiple = () => enumerable.SingleElementOrThrowOnMultiple(i => true, null);

            singleElementOrThrowOnMultiple.ShouldThrow<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple =
                () => ((IEnumerable<int>)null).SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple = () => new int[0].SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrByQueryByQueryShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new object[2];

            Action singleOr = () => enumerable.SingleOr(o => o == null, null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for (o == null).");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(x => x == "1", null).Should().Be("1");
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
            Action singleOr = () => ((IEnumerable<string>)null).SingleOr(x => false, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearching()
        {
            Action singleOr = () => new string[] { null }.SingleOr(null, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var enumerable = new object[2];

            Action singleOrDefaultOrThrow = () => enumerable.SingleOrDefaultOrThrow(o => o == null, null, ex);

            singleOrDefaultOrThrow.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnElementIfOneExists()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOrDefaultOrThrow(s => s == "1", null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var enumerable = new object[0];

            enumerable.SingleOrDefaultOrThrow(o => false, "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrDefaultOrThrow = () => ((IEnumerable<string>)null).SingleOrDefaultOrThrow(null, null, null);

            singleOrDefaultOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrShouldReturnElementIfOneExists()
        {
            var enumerable = new[] { "1" };

            enumerable.SingleOr(null).Should().Be("1");
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
            Action singleOr = () => ((IEnumerable<string>)null).SingleOr(null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var enumerable = new object[2];

            Action singleOr = () => enumerable.SingleOr(null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for true.");
        }

        [Fact]
        public void SingleOrThrowByQueryThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.SingleOrThrow(x => x == 1, null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.SingleOrThrow(null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IEnumerable<int>)null).SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => ((IEnumerable<int>)null).SingleOrThrow(x => false);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenMultipleMatchesFound()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(x => true, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNullWhenQueryIsNotFound()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(x => false, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenMultipleMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenNoMatchExceptionArgumentIsNull()
        {
            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("noMatchFoundException");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => new int[0].SingleOrThrow(null, null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1 }.SingleOrThrow(x => x == 0, ex, null);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenAnElementIsNotFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => Enumerable.Empty<object>().SingleOrThrow(ex, null);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(null, ex);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(x => x == 1, null, ex);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] { 1, 2 };

            enumerable.SingleOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action singleOrThrow = () => ((IEnumerable<int>)null).SingleOrThrow(x => false);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action singleOrThrow = () => new int[0].SingleOrThrow(null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action singleOrThrow = () => new[] { 1 }.SingleOrThrow(x => x == 0);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("No match found for (x == 0).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.SingleOrThrow(x => x == 1);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("More than one match found for (x == 1).");
        }
    }
}