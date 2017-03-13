namespace Omego.Extensions.Poco
{
    using System;
    using System.Globalization;

    /// <summary>
    ///     Represents an element.
    /// </summary>
    /// <typeparam name="T">The type this value wraps.</typeparam>
    public struct Element<T> : IEquatable<Element<T>>, IEquatable<T>
    {
        private readonly T value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Element{T}" /> of <typeparamref name="T" /> struct.
        /// </summary>
        /// <param name="value">The value to initialize with.</param>
        public Element(T value)
        {
            this.value = value;
            Present = true;
        }

        /// <summary>
        ///     Gets <see cref="Present" />.
        /// </summary>
        /// <value>Indicates whether the element exists.</value>
        public bool Present { get; }

        /// <summary>
        ///     Gets <see cref="Value" />.
        /// </summary>
        /// <value>The <typeparamref name="T" /> that this element represents.</value>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the element is not present.
        /// </exception>
        public T Value
        {
            get
            {
                if (!Present) throw new InvalidOperationException("Element does not exist.");

                return value;
            }
        }

        /// <summary>
        ///     Checks whether this instance of the value is equal to another
        ///     <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare to.
        /// </param>
        /// <returns>Whether the value is equal.</returns>
        public bool Equals(Element<T> other) => this == other;

        /// <summary>
        ///     Checks whether this instance of the value is equal to an
        ///     instance of <typeparamref name="T" />.
        /// </summary>
        /// <param name="other">
        ///     The instance of <typeparamref name="T" /> to compare to.
        /// </param>
        /// <returns>Whether the instance is equal.</returns>
        public bool Equals(T other) => Present && (Value != null ? Value.Equals(other) : other == null);

        /// <inheritdoc />
        public override bool Equals(object obj)
            => obj is Element<T> && Equals((Element<T>)obj) || obj is T && Equals((T)obj);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                const int nullHashCode = 1;
                const int notPresentHashCode = 0;
                const int salt = 193;

                Func<int, int> presentHash = hash =>
                    {
                        while (hash == notPresentHashCode || hash == nullHashCode) hash += salt;

                        return hash;
                    };

                return Present ? (value == null ? nullHashCode : presentHash(value.GetHashCode())) : notPresentHashCode;
            }
        }

        /// <summary>
        ///     The equal operator for two <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="first">
        ///     The first <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <param name="second">
        ///     The second <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <returns>A <see cref="bool" /> indicating whether values are equal.</returns>
        public static bool operator ==(Element<T> first, Element<T> second)
            => first.Present ? second.Present && first.Equals(second.Value) : !second.Present;

        /// <summary>
        ///     The not equal operator for two <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="first">
        ///     The first <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <param name="second">
        ///     The second <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <returns>A <see cref="bool" /> indicating whether values are not equal.</returns>
        public static bool operator !=(Element<T> first, Element<T> second) => !(first == second);

        /// <summary>
        ///     The implicit cast operator for casting an object of <typeparamref name="T" />
        ///     to <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="target">The instance of <typeparamref name="T" /> to cast.</param>
        /// <returns>A single value of <see cref="Element{T}" /> of <typeparamref name="T" />.</returns>
        public static implicit operator Element<T>(T target) => new Element<T>(target);

        /// <summary>
        ///     The explicit cast operator for casting a value of <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to <typeparamref name="T" />.
        /// </summary>
        /// <param name="target">
        ///     The <see cref="Element{T}" /> of <typeparamref name="T" /> to cast.
        /// </param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidCastException">
        ///     Thrown when the element is not present.
        /// </exception>
        public static explicit operator T(Element<T> target)
        {
            if (!target.Present)
                throw new InvalidCastException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "No element present to cast to {0}.",
                        typeof(T).FullName));

            return target.Value;
        }

        /// <summary>
        ///     Gets the value of this element or <paramref name="default" /> if no value exists.
        /// </summary>
        /// <param name="default">The default value to return if one does not exist.</param>
        /// <returns>An instance or value of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="default" /> is null.</exception>
        public T ValueOr(Func<T> @default)
        {
            Func<T> defaultSelector = () =>
                {
                    if (@default == null) throw new ArgumentNullException(nameof(@default));

                    return @default();
                };

            return Present ? Value : defaultSelector();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string stringValue;

            if (Present) stringValue = value != null ? value.ToString() : "Exists";
            else stringValue = "Does not exist";

            return stringValue;
        }
    }
}