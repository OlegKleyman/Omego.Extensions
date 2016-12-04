namespace Omego.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///     Represents an enumerator that attempts to catch exceptions.
    /// </summary>
    /// <typeparam name="T">The type to iterate.</typeparam>
    /// <typeparam name="TE">The type of <see cref="Exception" /> to catch.</typeparam>
    public class AttemptCatchIterator<T, TE> : IEnumerator<T>
        where TE : Exception
    {
        private readonly IEnumerator<T> enumerator;

        private readonly Action<TE> handler;

        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AttemptCatchIterator{T,TE}" /> class.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> of <see cref="T" /> to iterate on.</param>
        /// <param name="handler">
        ///     The <see cref="Action{T}" /> of <see cref="TE" /> to call when a(n) <see cref="TE" /> exception
        ///     occurrs.
        /// </param>
        public AttemptCatchIterator(IEnumerable<T> enumerable, Action<TE> handler)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            enumerator = enumerable.GetEnumerator();
            this.handler = handler;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Current = default(T);
            disposed = true;
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (disposed) throw new ObjectDisposedException(GetType().FullName);

            var success = false;
            bool exceptionOccured;

            do
            {
                try
                {
                    success = enumerator.MoveNext();

                    Current = success ? enumerator.Current : default(T);

                    exceptionOccured = false;
                }
                catch (TE ex)
                {
                    exceptionOccured = true;
                    handler(ex);
                }
            }
            while (exceptionOccured);

            return success;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <remarks>Not supported.</remarks>
        public void Reset()
        {
            if (disposed) throw new ObjectDisposedException(GetType().FullName);

            enumerator.Reset();
            Current = default(T);
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public T Current { get; private set; }

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => Current;
    }
}