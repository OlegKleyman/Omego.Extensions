namespace Omego.Extensions
{
    using System;

    public struct Element<T> : IEquatable<Element<T>>, IEquatable<T>
    {
        private readonly T value;

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

        public bool Equals(Element<T> other)
        {
            return Present == other.Present && (!Present || Equals(other.Value));
        }

        public bool Equals(T other)
        {
            return Present && (Value != null ? Value.Equals(other) : value == null && other == null);
        }

        public override bool Equals(object obj)
        {
            return (obj is Element<T> && Equals((Element<T>)obj)) || (obj is T && Equals((T)obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int nullHashCode = 1;
                const int notPresentHashCode = 0;
                const int salt = 193;

                Func<int, int> presentHash = hash =>
                    {
                        while (hash == notPresentHashCode || hash == nullHashCode)
                        {
                            hash = (nullHashCode + hash + 1) * salt;
                        }

                        return hash;
                    };

                return Present ? (value == null ? nullHashCode : presentHash(value.GetHashCode())) : notPresentHashCode;
            }
        }

        public static bool operator ==(Element<T> first, Element<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Element<T> first, Element<T> second)
        {
            return !(first == second);
        }

        public static implicit operator Element<T>(T target)
        {
            return new Element<T>(target);
        }

        public static explicit operator T(Element<T> target)
        {
            return target.Value;
        }
    }
}