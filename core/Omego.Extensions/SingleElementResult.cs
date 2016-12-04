namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;

    public struct SingleElementResult<T> : IEquatable<SingleElementResult<T>>
    {
        private readonly Element<T> value;

        public SingleElementResult(T value)
        {
            Elements = Elements.One;
            this.value = new Element<T>(value);
        }

        private SingleElementResult(Elements elements)
        {
            Elements = elements;
            value = default(Element<T>);
        }

        public Elements Elements { get; }

        public T Value
        {
            get
            {
                if (Elements == Elements.Multiple) throw new InvalidOperationException("Multiple elements found.");

                return value.Value;
            }
        }

        private static readonly Lazy<SingleElementResult<T>> MultipleElementResult =
            new Lazy<SingleElementResult<T>>(() => new SingleElementResult<T>(Elements.Multiple));

        private static readonly Lazy<SingleElementResult<T>> NoElementResult =
            new Lazy<SingleElementResult<T>>(() => new SingleElementResult<T>(Elements.None));

        public static SingleElementResult<T> MultipleElements => MultipleElementResult.Value;

        public static SingleElementResult<T> NoElements => NoElementResult.Value;

        public static bool operator ==(SingleElementResult<T> first, SingleElementResult<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(SingleElementResult<T> first, SingleElementResult<T> second)
        {
            return !(first == second);
        }

        public bool Equals(SingleElementResult<T> other)
        => Elements == other.Elements && (value.Equals(other.value));

        public override bool Equals(object obj) => obj is SingleElementResult<T> && Equals((SingleElementResult<T>)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return Elements.GetHashCode() + value.GetHashCode();
            }
        }
    }
}