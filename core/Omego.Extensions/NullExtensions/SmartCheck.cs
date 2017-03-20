namespace Omego.Extensions.NullExtensions
{
    using System;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    public static partial class ObjectExtensions
    {
        /// <summary>
        ///     Throws an exception if part of any of the <paramref name="qualifierPath" /> is null.
        /// </summary>
        /// <typeparam name="TTarget">The type check qualifying path on.</typeparam>
        /// <param name="target">The target object or value to check qualifying path on.</param>
        /// <param name="exception">
        ///     The exception to throw when the qualifying path contains null.
        /// </param>
        /// <param name="qualifierPath">The qualifying elements path to check.</param>
        /// <exception cref="ArgumentNullException">
        ///     Occurs when <paramref name="exception" /> or <paramref name="qualifierPath" /> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Occurs when <paramref name="exception" /> returns null.
        /// </exception>
        public static void SmartCheck<TTarget>(
            this TTarget target,
            Func<string, Exception> exception,
            params Expression<Func<TTarget, object>>[] qualifierPath)
        {
            var visitor = new SmartGetVisitor(target);

            foreach (var expression in qualifierPath ?? throw new ArgumentNullException(nameof(qualifierPath)))
            {
                visitor.OnNull(
                    expression,
                    s => throw (exception != null
                                    ? exception(s) ?? new InvalidOperationException("Exception to throw returned null.")
                                    : throw new ArgumentNullException(nameof(exception))));

                visitor.ResetWith(target);
            }
        }
    }
}