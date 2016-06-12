namespace Omego.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AttemptCatchIterator<T, TE> : IEnumerator<T> where TE: Exception
    {
        private readonly Action<TE> handler;

        private readonly IEnumerator<T> enumerator;

        private T current;

        private bool disposed;

        public AttemptCatchIterator(IEnumerable<T> enumerable, Action<TE> handler)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            enumerator = enumerable.GetEnumerator();
            this.handler = handler;
        }

        public void Dispose()
        {
            current = default(T);
            disposed = true;
        }

        public bool MoveNext()
        {
            var success = false;

            if (!disposed)
            {
                try
                {
                    success = enumerator.MoveNext();
                    current = enumerator.Current;
                }
                catch (TE ex)
                {
                    handler(ex);
                }
            }

            return success;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public T Current => current;

        object IEnumerator.Current => current;
    }
}