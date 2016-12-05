namespace Omego.Extensions
{
    using System;

    /// <summary>
    /// Represents an element.
    /// </summary>
    /// <typeparam name="T">The type this value wraps.</typeparam>
    public struct Element<T> : IEquatable<Element<T>>, IEquatable<T>
    {
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Element{T}" /> of <typeparamref name="T"/> struct.
        /// </summary>
        /// <param name="value">The value to initialize with.</param>
        public Element(T value)
        {
            this.value = value;
            Present = true;
        }

        public bool Present { get; }

        public T Value
        {
            get
            {
                if (!Present) throw new InvalidOperationException("Element does not exist.");

                return value;
            }
        }

        public bool Equals(Element<T> other) => this == other;

        public bool Equals(T other) => Present && (Value != null ? Value.Equals(other) : other == null);

        public override bool Equals(object obj)
            => (obj is Element<T> && Equals((Element<T>)obj)) || (obj is T && Equals((T)obj));

        public override int GetHashCode()
        {
            unchecked
            {
                const int nullHashCode = 1;
                const int notPresentHashCode = 0;
                const int salt = 193;

                Func<int, int> presentHash = hash =>
                    {
                        while ((hash == notPresentHashCode) || (hash == nullHashCode)) hash = (nullHashCode + hash + 1) * salt;

                        return hash;
                    };

                return Present ? (value == null ? nullHashCode : presentHash(value.GetHashCode())) : notPresentHashCode;
            }
        }

        public static bool operator ==(Element<T> first, Element<T> second)
            => first.Present ? second.Present && first.Equals(second.Value) : !second.Present;

        public static bool operator !=(Element<T> first, Element<T> second) => !(first == second);

        public static implicit operator Element<T>(T target) => new Element<T>(target);

        public static explicit operator T(Element<T> target) => target.Value;
    }
}