using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Omego.Extensions.Poco
{
#if NET462
    using System.Diagnostics.Contracts;
#endif

    /// <summary>
    ///     Represents an element.
    /// </summary>
    /// <typeparam name="T">The type this value wraps.</typeparam>
    public readonly struct Element<T> : IEquatable<Element<T>>, IEquatable<T>
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
        ///     Checks whether this instance of the value is equal to another
        ///     <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="Element{T}" /> of <typeparamref name="T" />
        ///     to compare to.
        /// </param>
        /// <returns>Whether the value is equal.</returns>
        public bool Equals(Element<T> other)
        {
            return this == other;
        }

        /// <summary>
        ///     Checks whether this instance of the value is equal to an
        ///     instance of <typeparamref name="T" />.
        /// </summary>
        /// <param name="other">
        ///     The instance of <typeparamref name="T" /> to compare to.
        /// </param>
        /// <returns>Whether the instance is equal.</returns>
        public bool Equals(T other)
        {
            return Present && (value?.Equals(other) ?? other == null);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Element<T> && Equals((Element<T>) obj)
                   || obj is T && Equals((T) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            const int nullHashCode = 1;
            const int notPresentHashCode = 0;
            const int salt = 193;

            int PresentHash(int hash)
            {
                unchecked
                {
                    while (hash == notPresentHashCode || hash == nullHashCode) hash += salt;

                    return hash;
                }
            }

            return Present ? (value == null ? nullHashCode : PresentHash(value.GetHashCode())) : notPresentHashCode;
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
        {
            return first.Present && second.Present
                                 && first.Equals(second.value)
                   || !first.Present && !second.Present;
        }

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
        public static bool operator !=(Element<T> first, Element<T> second)
        {
            return !(first == second);
        }

        /// <summary>
        ///     The implicit cast operator for casting an object of <typeparamref name="T" />
        ///     to <see cref="Element{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="target">The instance of <typeparamref name="T" /> to cast.</param>
        /// <returns>A single value of <see cref="Element{T}" /> of <typeparamref name="T" />.</returns>
        public static implicit operator Element<T>(T target)
        {
            return new Element<T>(target);
        }

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
            return target.ValueOr(
                () => throw new InvalidCastException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "No element present to cast to {0}.",
                        typeof(T).FullName)));
        }

        /// <summary>
        ///     Gets the value of this element or <paramref name="default" /> if no value exists.
        /// </summary>
        /// <param name="default">The default value to return if one does not exist.</param>
        /// <returns>An instance or value of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="default" /> is null.</exception>
        [Pure]
        public T ValueOr(Func<T> @default)
        {
            T DefaultSelector()
            {
                return @default != null ? @default() : throw new ArgumentNullException(nameof(@default));
            }

            return Present ? value : DefaultSelector();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string stringValue;

            if (Present) stringValue = value != null ? value.ToString() : "Exists";
            else stringValue = "Does not exist";

            return stringValue;
        }

        /// <summary>
        ///     Executes <see cref="Action{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void WhenPresent(Action<T> action)
        {
            if (Present) (action ?? throw new ArgumentNullException(nameof(action)))(value);
        }
    }
}