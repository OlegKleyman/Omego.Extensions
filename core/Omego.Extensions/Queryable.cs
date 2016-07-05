namespace Omego.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class Queryable
    {
        public static T FirstOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, Exception exception)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (!queryable.Any(predicate))
            {
                if (exception == null) throw new ArgumentNullException(nameof(exception));

                throw exception;
            }

            return queryable.First(predicate);
        }

        public static T FirstOrThrow<T>(this IQueryable<T> queryable, Exception exception)
        {
            return queryable.FirstOrThrow(element => true, exception);
        }
    }
}
