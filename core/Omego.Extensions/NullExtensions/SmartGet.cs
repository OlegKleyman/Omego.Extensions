namespace Omego.Extensions.NullExtensions
{
    using System;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    /// <summary>
    ///     Contains extension methods for <see cref="object" />.
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        ///     Gets an instance or value specified by <paramref name="result" /> or throws
        ///     an exception if part of the <paramref name="qualifierPath" /> is null.
        /// </summary>
        /// <typeparam name="TTarget">The type check qualifying path on.</typeparam>
        /// <typeparam name="TObject">The final type of the qualifying path to check.</typeparam>
        /// <typeparam name="TResult">The type to return if nothing is null.</typeparam>
        /// <param name="target">The target object or value to check qualifying path on.</param>
        /// <param name="qualifierPath">The qualifying path to check.</param>
        /// <param name="result">The object or value to retrieve when the qualifying path is not null.</param>
        /// <param name="exception">
        ///     The exception to throw when the qualifying path contains null.
        /// </param>
        /// <returns>The <typeparamref name="TResult" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Occurs when <paramref name="result" /> or <paramref name="exception" /> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Occurs when <paramref name="exception" /> returns null.
        /// </exception>
        public static TResult SmartGet<TTarget, TObject, TResult>(
            this TTarget target,
            Expression<Func<TTarget, TObject>> qualifierPath,
            Func<TObject, TResult> result,
            Func<string, Exception> exception)
        {
            var visitor = new SmartGetVisitor(target);

            visitor.OnNull(
                qualifierPath,
                nullQualifier =>
                    {
                        if (exception == null) throw new ArgumentNullException(nameof(exception));

                        var toThrow = exception(nullQualifier);

                        if (toThrow == null) throw new InvalidOperationException("Exception to throw returned null.");

                        throw toThrow;
                    });

            if (result == null) throw new ArgumentNullException(nameof(result));
            
            return result((TObject)visitor.Current);
        }
    }
}