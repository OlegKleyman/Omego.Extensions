namespace Omego.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AttemptCatchIterator<T, TE> : IEnumerator<T> where TE: Exception
    {
        private readonly Action<TE> handler;

        private readonly IEnumerator<T> enumerator;

        public AttemptCatchIterator(IEnumerable<T> enumerable, Action<TE> handler)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            enumerator = enumerable.GetEnumerator();
            this.handler = handler;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveNext()
        {
            var success = false;

            try
            {
                success = enumerator.MoveNext();
            }
            catch (TE ex)
            {
                handler(ex);
            }

            return success;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public T Current { get; }

        object IEnumerator.Current => Current;
    }
}