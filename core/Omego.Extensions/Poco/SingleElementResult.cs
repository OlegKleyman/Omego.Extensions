using System;
using System.Globalization;
using System.Linq;

namespace Omego.Extensions.Poco
{
    /// <summary>
    ///     Represents a single element.
    /// </summary>
    /// <typeparam name="T">The type this value wraps.</typeparam>
    public struct SingleElementResult<T> : IEquatable<SingleElementResult<T>>, IEquatable<T>
    {
        private readonly Element<T> value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SingleElementResult{T}" /> of <typeparamref name="T" /> struct.
        /// </summary>
        /// <param name="value">The value to initialize with.</param>
        public SingleElementResult(T value)
        {
            Elements = ElementCategory.One;
            this.value = new Element<T>(value);
        }

        private SingleElementResult(ElementCategory elements)
        {
            Elements = elements;
            value = default(Element<T>);
        }

        private ElementCategory Elements { get; }

        private static readonly Lazy<SingleElementResult<T>> MultipleElementResult = new Lazy<SingleElementResult<T>>(
            () => new SingleElementResult<T>(ElementCategory.Multiple));

        private static readonly Lazy<SingleElementResult<T>> NoElementResult = new Lazy<SingleElementResult<T>>(
            () => new SingleElementResult<T>(ElementCategory.None));

        /// <summary>
        ///     Gets <see cref="MultipleElements" />.
        /// </summary>
        /// <value>
        ///     A <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     that represents multiple elements.
        /// </value>
        public static SingleElementResult<T> MultipleElements => MultipleElementResult.Value;

        /// <summary>
        ///     Gets <see cref="NoElements" />.
        /// </summary>
        /// <value>
        ///     A <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     that represents no elements.
        /// </value>
        public static SingleElementResult<T> NoElements => NoElementResult.Value;

        /// <summary>
        ///     The equal operator for two <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="first">
        ///     The first <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <param name="second">
        ///     The second <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <returns>A <see cref="bool" /> indicating whether values are equal.</returns>
        public static bool operator ==(
            SingleElementResult<T> first,
            SingleElementResult<T> second)
        {
            return first.Elements == second.Elements && first.value.Equals(second.value);
        }

        /// <summary>
        ///     The not equal operator for two <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="first">
        ///     The first <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <param name="second">
        ///     The second <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     to compare.
        /// </param>
        /// <returns>A <see cref="bool" /> indicating whether values are not equal.</returns>
        public static bool operator !=(
            SingleElementResult<T> first,
            SingleElementResult<T> second)
        {
            return !(first == second);
        }

        /// <summary>
        ///     The implicit cast operator for casting an object of <typeparamref name="T" />
        ///     to <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="target">The instance of <typeparamref name="T" /> to cast.</param>
        /// <returns>A single value of <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />.</returns>
        public static implicit operator SingleElementResult<T>(T target)
        {
            return new SingleElementResult<T>(target);
        }

        /// <summary>
        ///     The explicit cast operator for casting a value of <see cref="SingleElementResult{T}" /> of
        ///     <typeparamref name="T" />
        ///     to <typeparamref name="T" />.
        /// </summary>
        /// <param name="target">
        ///     The <see cref="SingleElementResult{T}" /> of <typeparamref name="T" /> to cast.
        /// </param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidCastException">
        ///     Thrown when the element does not represent one element.
        /// </exception>
        public static explicit operator T(SingleElementResult<T> target)
        {
            return target.value.ValueOr(
                () => throw new InvalidCastException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} element(s) cannot be cast to {1}.",
                        target.Elements,
                        typeof(T).FullName)));
        }

        /// <summary>
        ///     Checks whether this instance of the value is equal to another
        ///     <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="SingleElementResult{T}" /> of <typeparamref name="T" />
        ///     to compare to.
        /// </param>
        /// <returns>Whether the value is equal.</returns>
        public bool Equals(SingleElementResult<T> other)
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
            return value == other;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise,
        ///     false.
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is SingleElementResult<T> && Equals((SingleElementResult<T>) obj)
                   || obj is T && Equals((T) obj);
        }

        private static readonly int ElementsMaxValue = Enum.GetValues(typeof(ElementCategory)).Cast<int>().Max();

        private static readonly int ElementsMinValue = Enum.GetValues(typeof(ElementCategory)).Cast<int>().Min();

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            const int salt = 193;

            int HashCode(int hash)
            {
                unchecked
                {
                    while (hash >= ElementsMinValue && hash <= ElementsMaxValue && hash != (int) ElementCategory.One)
                        hash += salt;

                    return hash;
                }
            }

            return Elements == ElementCategory.One ? HashCode(value.GetHashCode()) : Elements.GetHashCode();
        }

        private enum ElementCategory
        {
            None,

            One,

            Multiple
        }

        /// <summary>
        ///     Gets the value of this element or <paramref name="default" /> if no value exists.
        /// </summary>
        /// <param name="default">The default value to return if one does not exist.</param>
        /// <returns>An instance or value of <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple elements exist.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="default" /> is null.</exception>
        public T ValueOr(Func<T> @default)
        {
            return ValueOr(@default, @default);
        }

        /// <summary>
        ///     Gets the value of this element, the <paramref name="defaultNoElement" /> if no value exists,
        ///     or <paramref name="defaultMultipleElements" />.
        /// </summary>
        /// <param name="defaultNoElement">The default value to return if one does not exist.</param>
        /// <param name="defaultMultipleElements">
        ///     The default value to return if multiple elements exist.
        /// </param>
        /// <returns>An instance or value of <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple elements exist.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="defaultNoElement" /> or
        ///     <paramref name="defaultMultipleElements" /> is null.
        /// </exception>
        public T ValueOr(Func<T> defaultNoElement, Func<T> defaultMultipleElements)
        {
            T DefaultMultipleElementSelector()
            {
                return defaultMultipleElements != null
                    ? defaultMultipleElements()
                    : throw new ArgumentNullException(
                        nameof(defaultMultipleElements));
            }

            return Elements != ElementCategory.Multiple
                ? value.ValueOr(defaultNoElement)
                : DefaultMultipleElementSelector();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Elements == ElementCategory.One || Elements == ElementCategory.None
                ? value.ToString()
                : Elements.ToString();
        }
    }
}