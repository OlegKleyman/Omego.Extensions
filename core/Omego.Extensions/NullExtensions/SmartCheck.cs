namespace Omego.Extensions.NullExtensions
{
    using System;
    using System.Linq.Expressions;

    using Omego.Extensions.Poco;

    public static partial class ObjectExtensions
    {
        public static void SmartCheck<TTarget, TObject>(
            this TTarget target,
            Func<string, Exception> exception,
            params Expression<Func<TTarget, TObject>>[] qualifierPath)
        {
            if (qualifierPath == null) throw new ArgumentNullException(nameof(qualifierPath));

            var visitor = new SmartGetVisitor(target);

            foreach (var expression in qualifierPath)
            {
                visitor.OnNull(
                    expression,
                    s =>
                        {
                            if (exception == null) throw new ArgumentNullException(nameof(exception));

                            var toThrow = exception(s);

                            if (toThrow == null) throw new InvalidOperationException("Exception to throw returned null.");

                            throw toThrow;
                        });

                visitor.ResetWith(target);
            }
        }
    }
}