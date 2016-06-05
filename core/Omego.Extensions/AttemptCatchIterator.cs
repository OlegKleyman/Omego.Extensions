namespace Omego.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    public class AttemptCatchIterator<T> : IEnumerator<T>
    {
        public AttemptCatchIterator(IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));


        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public T Current { get; }

        object IEnumerator.Current => Current;
    }
}