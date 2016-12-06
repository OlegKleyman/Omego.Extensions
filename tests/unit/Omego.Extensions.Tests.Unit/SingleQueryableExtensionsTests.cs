namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Omego.Extensions.Poco;
    using Omego.Extensions.QueryableExtensions;

    using Xunit;

    public class SingleQueryableExtensionsTests
    {
        public void SingleOrShouldReturnElementIfOneExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(null).Should().Be("1");
        }

        [Fact]
        public void SingleElementByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleElement(x => x == 1).Value.Should().Be(1);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnMultipleMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new[] { 1, 2 }.AsQueryable();

            queryable.SingleElement(x => true).Should().Be(SingleElementResult<int>.MultipleElements);
        }

        [Fact]
        public void SingleElementByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new int[0].AsQueryable();

            queryable.SingleElement(x => false).Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElement = () => new int[0].AsQueryable().SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleElementByQueryWhenFoundShouldThrowArgumentNullExceptionWhenqueryableArgumentIsNull()
        {
            Action singleElement = () => ((IQueryable<int>)null).SingleElement(null);

            singleElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleElementOrThrowOnMultiple(x => x == 1, null).Value.Should().Be(1);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldReturnNoMatchesFlagWhenElementIsNotFound()
        {
            var queryable = new int[0].AsQueryable();

            queryable.SingleElementOrThrowOnMultiple(x => false, null).Should().Be(SingleElementResult<int>.NoElements);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new[] { 1, 2 }.AsQueryable();

            var ex = new InvalidOperationException();

            Action singleElementOrThrowOnMultiple = () => queryable.SingleElementOrThrowOnMultiple(i => true, ex);

            singleElementOrThrowOnMultiple.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleElementOrThrowOnMultipleByQueryShouldThrowExceptionWhenMultipleElementsExceptionIsNull()
        {
            var queryable = new[] { 1, 2 }.AsQueryable();

            Action singleElementOrThrowOnMultiple = () => queryable.SingleElementOrThrowOnMultiple(i => true, null);

            singleElementOrThrowOnMultiple.ShouldThrow<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("multipleMatchesFoundException");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple =
                () => ((IQueryable<int>)null).SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void
            SingleElementOrThrowOnMultipleByQueryWhenFoundShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNull()
        {
            Action singleElementOrThrowOnMultiple =
                () => new int[0].AsQueryable().SingleElementOrThrowOnMultiple(null, null);

            singleElementOrThrowOnMultiple.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnElementWhenFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(x => x == "1", null).Should().Be("1");
        }

        [Fact]
        public void SingleOrByQueryShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearching()
        {
            Action singleOr = () => new string[] { null }.AsQueryable().SingleOr(null, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNullWhenSearching()
        {
            Action singleOr = () => ((IQueryable<string>)null).SingleOr(x => false, null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrByQueryShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr(o => o == null, (object)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for (o == null).");
        }

        [Fact]
        public void SingleOrDefaultOrThrowByQueryShouldThrowExceptionWhenMultipleElementsAreFound()
        {
            var ex = new InvalidOperationException();

            var queryable = new object[2].AsQueryable();

            Action singleOrDefaultOrThrow = () => queryable.SingleOrDefaultOrThrow(o => o == null, (object)null, ex);

            singleOrDefaultOrThrow.ShouldThrow<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnElementIfOneExists()
        {
            var queryable = new[] { "1" }.AsQueryable();

            queryable.SingleOrDefaultOrThrow(s => s == "1", null, null).Should().Be("1");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOrDefaultOrThrow(o => false, "3", null).Should().Be("3");
        }

        [Fact]
        public void SingleOrDefaultOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrDefaultOrThrow = () => ((IQueryable<string>)null).SingleOrDefaultOrThrow(null, null, null);

            singleOrDefaultOrThrow.ShouldThrowExactly<ArgumentNullException>()
                .Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrShouldReturnRequestedDefaultObjectWhenNoElementsAreFound()
        {
            var queryable = new object[0].AsQueryable();

            queryable.SingleOr("3").Should().Be("3");
        }

        [Fact]
        public void SingleOrShouldThrowArgumentNullExceptionWhenQueryableArgumentIsNull()
        {
            Action singleOr = () => ((IQueryable<string>)null).SingleOr(null);

            singleOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrShouldThrowInvalidOperationExceptionWhenMultipleElementsAreFound()
        {
            var queryable = new object[2].AsQueryable();

            Action singleOr = () => queryable.SingleOr((string)null);

            singleOr.ShouldThrow<InvalidOperationException>().WithMessage("More than one match found for true.");
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleOrThrow(x => x == 1, null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldReturnElementWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleOrThrow(null, null).Should().Be(1);
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action singleOrThrow = () => ((IQueryable<int>)null).SingleOrThrow(null, null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void SingleOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action singleOrThrow = () => ((IQueryable<int>)null).SingleOrThrow(x => false);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
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

        [Fact]
        public void SingleOrThrowShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            var ex = new InvalidOperationException();

            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(x => x == 1, null, ex);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>().Which.Should().Be(ex);
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldReturnElementByQueryWhenFound()
        {
            var queryable = new[] { 1 }.AsQueryable();

            queryable.SingleOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action singleOrThrow = () => ((IQueryable<int>)null).SingleOrThrow(x => false);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("queryable");
        }

        [Fact]
        public void
            SingleOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action singleOrThrow = () => new int[0].AsQueryable().SingleOrThrow(null);

            singleOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action singleOrThrow = () => new[] { 1 }.AsQueryable().SingleOrThrow(x => x == 0);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("No match found for (x == 0).");
        }

        [Fact]
        public void SingleOrThrowWithGenericExceptionShouldThrowExceptionWhenMultipleElementsByQueryAreFound()
        {
            Action singleOrThrow = () => new[] { 1, 1 }.AsQueryable().SingleOrThrow(x => x == 1);

            singleOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("More than one match found for (x == 1).");
        }
    }
}