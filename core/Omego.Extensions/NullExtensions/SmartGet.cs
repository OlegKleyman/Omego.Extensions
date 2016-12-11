namespace Omego.Extensions.NullExtensions
{
    using System;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    public static class ObjectExtensions
    {
        public static TResult SmartGet<TTarget, TOrig, TResult>(
            this TTarget target,
            Expression<Func<TTarget, TOrig>> qualifierPath,
            Func<TOrig, TResult> result,
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

                        throw exception(nullQualifier);
                    });

            if(result == null) throw new ArgumentNullException(nameof(result));

            return result((TOrig)visitor.Current);
        }
    }
}
