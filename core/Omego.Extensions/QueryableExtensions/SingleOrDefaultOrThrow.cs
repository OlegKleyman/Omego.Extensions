﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Omego.Extensions.QueryableExtensions
{
    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class Queryable
    {
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
            return queryable.SingleOrDefaultOrThrow(
                predicate,
                () => @default,
                multipleMatchesFoundException);
        }

        /// <summary>
        ///     Returns a single element of an <see cref="IQueryable{T}" /> matching the given predicate or returns
        ///     a requested default object of type <typeparamref name="T" /> or throws an exception specified
        ///     for the <paramref name="multipleMatchesFoundException" /> parameter if multiple are found.
        /// </summary>
        /// <param name="queryable">The queryable to find the single element in.</param>
        /// <param name="predicate">The predicate to use to find a single match.</param>
        /// <param name="default">
        ///     The <see cref="Func{TResult}" /> of <typeparamref name="T" /> to retrieve the
        ///     default object to return if no elements are found.
        /// </param>
        /// <param name="multipleMatchesFoundException">The exception to throw when multiple matches are found.</param>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        public static T SingleOrDefaultOrThrow<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            Func<T> @default,
            Exception multipleMatchesFoundException)
        {
            return queryable
                .SingleElementOrThrowOnMultiple(predicate, multipleMatchesFoundException)
                .ValueOr(@default);
        }
    }
}