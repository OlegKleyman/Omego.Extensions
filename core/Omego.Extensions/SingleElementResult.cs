namespace Omego.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct SingleElementResult<T> : IEquatable<SingleElementResult<T>>, IEquatable<T>
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

        private Elements Elements { get; }

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
            => first.Elements == second.Elements && first.value.Equals(second.value);

        public static bool operator !=(SingleElementResult<T> first, SingleElementResult<T> second)
            => !(first == second);

        public static implicit operator SingleElementResult<T>(T target) => new SingleElementResult<T>(target);

        public static explicit operator T(SingleElementResult<T> target) => target.Value;

        public bool Equals(SingleElementResult<T> other) => this == other;

        public bool Equals(T other) => value == other;

        public override bool Equals(object obj) => (obj is SingleElementResult<T> && Equals((SingleElementResult<T>)obj)) || (obj is T && Equals((T)obj));

        private static readonly int ElementsMaxValue = Enum.GetValues(typeof(Elements)).Cast<int>().Max();
        private static readonly int ElementsMinValue = Enum.GetValues(typeof(Elements)).Cast<int>().Min();

        public override int GetHashCode()
        {
            unchecked
            {
                const int salt = 193;

                Func<int, int> getHashCode = hash =>
                    {
                        while (hash >= ElementsMinValue && hash <= ElementsMaxValue && hash != (int)Elements.One)
                        {
                            hash = (hash + ElementsMaxValue + 1) * salt;
                        }

                        return hash;
                    };

                return Elements == Elements.One ? getHashCode(value.GetHashCode()) : Elements.GetHashCode();
            }
        }
    }
}