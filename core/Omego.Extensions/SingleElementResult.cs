namespace Omego.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a single element.
    /// </summary>
    /// <typeparam name="T">The type this value wraps.</typeparam>
    public struct SingleElementResult<T> : IEquatable<SingleElementResult<T>>, IEquatable<T>
    {
        private readonly Element<T> value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleElementResult{T}" /> of <typeparamref name="T"/> struct.
        /// </summary>
        /// <param name="value">The value to initialize with.</param>
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

        /// <summary>
        /// Gets <see cref="Value"/>.
        /// </summary>
        /// <value>The <typeparamref name="T"/> that this element represents.</value>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there are multiple elements of the value.
        /// </exception>
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

        /// <summary>
        /// Gets <see cref="MultipleElements"/>.
        /// </summary>
        /// <value>
        /// A <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// that represents multiple elements.
        /// </value>
        public static SingleElementResult<T> MultipleElements => MultipleElementResult.Value;

        /// <summary>
        /// Gets <see cref="NoElements"/>.
        /// </summary>
        /// <value>
        /// A <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// that represents no elements.
        /// </value>
        public static SingleElementResult<T> NoElements => NoElementResult.Value;

        /// <summary>
        /// The equal operator for two <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="first">The first <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to compare.</param>
        /// <param name="second">The second <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to compare.</param>
        /// <returns>A <see cref="bool"/> indicating whether values are equal.</returns>
        public static bool operator ==(SingleElementResult<T> first, SingleElementResult<T> second)
            => (first.Elements == second.Elements) && first.value.Equals(second.value);

        /// <summary>
        /// The not equal operator for two <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="first">The first <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to compare.</param>
        /// <param name="second">The second <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to compare.</param>
        /// <returns>A <see cref="bool"/> indicating whether values are not equal.</returns>
        public static bool operator !=(SingleElementResult<T> first, SingleElementResult<T> second)
            => !(first == second);

        /// <summary>
        /// The implicit cast operator for casting an object of <typeparamref name="T"/>
        /// to <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="target">The instance of <typeparamref name="T"/> to cast.</param>
        /// <returns>A single value of <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>.</returns>
        public static implicit operator SingleElementResult<T>(T target) => new SingleElementResult<T>(target);

        /// <summary>
        /// The explicit cast operator for casting a value of <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/> to cast.
        /// </param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown when the element does not represent one element.
        /// </exception>
        public static explicit operator T(SingleElementResult<T> target)
        {
            if (target.Elements != Elements.One)
            {
                throw new InvalidCastException(
                          string.Format(
                              CultureInfo.InvariantCulture,
                              "{0} element(s) cannot be cast to {1}.",
                              target.Elements,
                              typeof(T).FullName));
            }

            return target.Value;
        }

        /// <summary>
        /// Checks whether this instance of the value is equal to another
        /// <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="SingleElementResult{T}"/> of <typeparamref name="T"/>
        /// to compare to.
        /// </param>
        /// <returns>Whether the value is equal.</returns>
        public bool Equals(SingleElementResult<T> other) => this == other;

        /// <summary>
        /// Checks whether this instance of the value is equal to an
        /// instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other">
        /// The instance of <typeparamref name="T"/> to compare to.
        /// </param>
        /// <returns>Whether the instance is equal.</returns>
        public bool Equals(T other) => value == other;

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
            => (obj is SingleElementResult<T> && Equals((SingleElementResult<T>)obj)) || (obj is T && Equals((T)obj));

        private static readonly int ElementsMaxValue = Enum.GetValues(typeof(Elements)).Cast<int>().Max();

        private static readonly int ElementsMinValue = Enum.GetValues(typeof(Elements)).Cast<int>().Min();

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                const int salt = 193;

                Func<int, int> getHashCode = hash =>
                    {
                        while ((hash >= ElementsMinValue) && (hash <= ElementsMaxValue) && (hash != (int)Elements.One)) hash = (hash + ElementsMaxValue + 1) * salt;

                        return hash;
                    };

                return Elements == Elements.One ? getHashCode(value.GetHashCode()) : Elements.GetHashCode();
            }
        }
    }
}