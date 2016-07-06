﻿namespace Omego.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

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
        public static T FirstOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception exception)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (!queryable.Any(predicate))
            {
                if (exception == null) throw new ArgumentNullException(nameof(exception));

                throw exception;
            }

            return queryable.First(predicate);
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
        public static T SingleOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Exception noMatchFoundException,
            Exception multipleMatchesFoundException)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var count = queryable.Count(predicate);

            if (count > 1)
            {
                if (multipleMatchesFoundException == null) throw new ArgumentNullException(nameof(multipleMatchesFoundException));

                throw multipleMatchesFoundException;
            }

            if (count == 0)
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
        public static T SingleOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SingleOrThrow(
                queryable,
                predicate,
                new InvalidOperationException($"No match found for {predicate.Body}."),
                new InvalidOperationException($"More than one match found for {predicate.Body}."));
        }
    }
}