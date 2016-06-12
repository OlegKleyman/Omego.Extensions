namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Enumerable
    {
        public static IEnumerable<T> Catch<T, TE>(this IEnumerable<T> target, Action<TE> handler) where TE: Exception
        {
            var iterator = new AttemptCatchIterator<T, TE>(target, handler);

            while (iterator.MoveNext() || iterator.ExceptionOccured)
            {
                if (iterator.ExceptionOccured)
                {
                    continue;
                }

                yield return iterator.Current;
            }
        }
    }
}
