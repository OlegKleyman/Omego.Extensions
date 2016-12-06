namespace Omego.Extensions.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Omego.Extensions.EnumerableExtensions;

    using Xunit;

    public partial class EnumerableTests
    {
        [Fact]
        public void AttemptCatchShouldCatchException()
        {
            var enumerable = new[] { 0 }.Select(i => 1 / i);

            var handled = false;

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => handled = true).ToList();
            handled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldHandleAllRequestedExceptionsWhenCalledOnAnotherCatch()
        {
            var enumerable = new[] { -1, 0, 1 }.Select(
                i =>
                    {
                        if (i == 1) throw new InvalidOperationException();

                        return 1 / i;
                    });

            var dividedByZeroExceptionHandled = false;
            var invalidOperationExceptionHandled = false;

            enumerable.AttemptCatch<int, DivideByZeroException>(ex => dividedByZeroExceptionHandled = true)
                .AttemptCatch<int, InvalidOperationException>(ex => invalidOperationExceptionHandled = true)
                .ToList();

            dividedByZeroExceptionHandled.Should().BeTrue();
            invalidOperationExceptionHandled.Should().BeTrue();
        }

        [Fact]
        public void AttemptCatchShouldReturnElementsWhereTheCaughtExceptionHasNotOccurred()
        {
            var enumerable = new[] { -1, 0, 1 }.Select(i => 1 / i);
            enumerable.AttemptCatch<int, DivideByZeroException>(e => { }).ShouldAllBeEquivalentTo(new[] { -1, 1 });
        }

        [Fact]
        public void FirstElementHasValueShouldReturnFalseWhenElementWasNotFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "2").Present.Should().BeFalse();
        }

        [Fact]
        public void FirstElementHasValueShouldReturnTrueWhenElementWasFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "1").Present.Should().BeTrue();
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => ((IEnumerable<string>)null).FirstElement(x => false);

            firstElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void FirstElementShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstElement = () => new string[] { null }.FirstElement(null);

            firstElement.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstElementValueShouldReturnElementValueWhenElementWasFoundWhenSearchingByQuery()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstElement(x => x == "1").Value.ShouldBeEquivalentTo("1");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldArgumentNullExceptionWhenDefaultFuncIsNull()
        {
            var enumerable = new[] { "1" };

            Action firstOr = () => enumerable.FirstOr(x => x == "2", (Func<string>)null);

            firstOr.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("default");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr(x => x == "1", (Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(s => false, () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyByQueryShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr(x => x == "2", () => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnElementWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr((Func<string>)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrLazyShouldReturnRequestedDefaultObjectWhenNotFound()
        {
            var enumerable = new string[0];

            enumerable.FirstOr(() => "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr(x => x == "1", (string)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenEnumerableIsEmpty()
        {
            var enumerable = new string[0];

            enumerable.FirstOr("3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnRequestedDefaultObjectWhenQueryIsNotFound()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr(x => x == "2", "3").Should().Be("3");
        }

        [Fact]
        public void FirstOrShouldReturnTheFirstElementWhenExists()
        {
            var enumerable = new[] { "1" };

            enumerable.FirstOr((string)null).Should().Be("1");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNull()
        {
            Action firstOrThrow = () => ((IEnumerable<string>)null).FirstOr((string)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IEnumerable<string>)null).FirstOr(x => false, (string)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void FirstOrShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOr = () => new string[] { null }.FirstOr(null, (string)null);

            firstOr.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementByQueryWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.FirstOrThrow(x => x == 1, null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldReturnElementWhenFound()
        {
            var enumerable = new[] { 1 };

            enumerable.FirstOrThrow((Exception)null).Should().Be(1);
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery()
        {
            Action firstOrThrow = () => ((IEnumerable<int>)null).FirstOrThrow(x => false, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void FirstOrThrowShouldThrowArgumentNullExceptionWhenExceptionArgumentIsNull()
        {
            var enumerable = new object[0];

            Action firstOrThrow = () => enumerable.FirstOrThrow((Exception)null);

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
            Action firstOrThrow = () => new[] { 1 }.FirstOrThrow(null, null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            var enumerable = new[] { 1 };

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
            var enumerable = new[] { 1 };

            enumerable.FirstOrThrow(x => x == 1).Should().Be(1);
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenEnumerableArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action firstOrThrow = () => ((IEnumerable<int>)null).FirstOrThrow(x => false);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("enumerable");
        }

        [Fact]
        public void
            FirstOrThrowWithGenericExceptionShouldThrowArgumentNullExceptionWhenPredicateArgumentIsNullWhenSearchingByQuery
            ()
        {
            Action firstOrThrow = () => new int[0].FirstOrThrow((Expression<Func<int, bool>>)null);

            firstOrThrow.ShouldThrowExactly<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("predicate");
        }

        [Fact]
        public void FirstOrThrowWithGenericExceptionShouldThrowExceptionWhenAnElementByQueryIsNotFound()
        {
            Action firstOrThrow = () => new[] { 1 }.FirstOrThrow(x => x == 0);

            firstOrThrow.ShouldThrowExactly<InvalidOperationException>()
                .Which.Message.ShouldBeEquivalentTo("No matches found for: (x == 0)");
        }
    }
}