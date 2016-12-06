namespace Omego.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static class Queryable
    {
        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="exception" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception exception)
        {
            var element = queryable.FirstElement(predicate);

            if (!element.Present)
            {
                if (exception == null) throw new ArgumentNullException(nameof(exception));

                throw exception;
            }

            return element.Value;
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="exception">The exception to throw when the element is not found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOrThrow<T>(this IQueryable<T> queryable, Exception exception)
        {
            return queryable.FirstOrThrow(element => true, exception);
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T FirstOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return queryable.FirstOrThrow(
                predicate,
                new InvalidOperationException($"No matches found for: {predicate.Body}"));
        }

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="noMatchFoundException" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException)
        {
            var element = queryable.SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException);

            if (element == SingleElementResult<T>.NoElements)
            {
                if (noMatchFoundException == null) throw new ArgumentNullException(nameof(noMatchFoundException));

                throw noMatchFoundException;
            }

            return queryable.SingleOrDefault(predicate);
        }

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="noMatchFoundException">The exception to throw when the element is not found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrThrow<T>(
            this IQueryable<T> queryable,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException)
        {
            return queryable.SingleOrThrow(element => true, noMatchFoundException, multipleMatchesFoundException);
        }

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}" /> of <typeparamref name="T" /> to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SingleOrThrow(
                queryable,
                predicate,
                new InvalidOperationException($"No match found for {predicate.Body}."),
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the object to find.</typeparam>
        /// <param name="queryable">The queryable to find the element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <returns>An <see cref="Element{T}" /> of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static Element<T> FirstElement<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var results = queryable.Where(predicate).Take(1);

            return results.Any() ? new Element<T>(results.First()) : default(Element<T>);
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate
        ///     or throws an exception specified for the
        ///     <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElementOrThrowOnMultiple<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception multipleMatchesFoundException)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var results =
                queryable.Where(predicate)
                    .Take(2)
                    .Select(arg => new SingleElementResult<T>(arg))
                    .DefaultIfEmpty(SingleElementResult<T>.NoElements);

            var count = results.Count();

            if (count > 1)
            {
                if (multipleMatchesFoundException == null) throw new ArgumentNullException(nameof(multipleMatchesFoundException));

                throw multipleMatchesFoundException;
            }

            return results.First();
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" /> or throws an exception specified
        ///     for the <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrDefaultOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            T @default,
            Exception multipleMatchesFoundException)
        {
            var element = queryable.SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException);

            return element == SingleElementResult<T>.NoElements ? @default : element.Value;
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="predicate">The predicate to use to find the first element.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FirstOr<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, T @default)
        {
            var element = queryable.FirstElement(predicate);

            return element.Present ? element.Value : @default;
        }

        /// <summary>
        ///     Returns the first element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the first element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T FirstOr<T>(this IQueryable<T> queryable, T @default)
        {
            var element = queryable.FirstElement(arg => true);

            return element.Present ? element.Value : @default;
        }

        /// <summary>
        ///     Returns a single match from an <see cref="IQueryable{T}" /> of <typeparamref name="T" /> or throws an
        ///     <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="queryable" /> or <paramref name="predicate" /> argument is null.
        /// </exception>
        public static SingleElementResult<T> SingleElement<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var results =
                queryable.Where(predicate)
                    .Take(2)
                    .Select(arg => new SingleElementResult<T>(arg))
                    .DefaultIfEmpty(SingleElementResult<T>.NoElements);

            return results.Count() > 1 ? SingleElementResult<T>.MultipleElements : results.First();
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the <paramref name="predicate" /> argument is null.
        /// </exception>
        public static T SingleOr<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, T @default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return queryable.SingleOrDefaultOrThrow(
                predicate,
                @default,
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> or returns
        ///     a requested default object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="default">The object to return if no elements are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOr<T>(this IQueryable<T> queryable, T @default)
        {
            return queryable.SingleOr(arg => true, @default);
        }
    }
}