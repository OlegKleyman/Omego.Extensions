using System;
using Optional;

namespace Omego.Extensions.OptionExtensions
{
    public static class Option
    {
        /// <summary>
        ///     Evaluates a specified action if a value is present.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped va;ue.</typeparam>
        /// <param name="option">The <see cref="Option{T}" /> to match on.</param>
        /// <param name="action">The action to evaluate if the value is present.</param>
        /// <returns>The same <see cref="Option{T}" /> that was matched on.</returns>
        public static Option<T> MatchSomeContinue<T>(this Option<T> option, Action<T> action)
        {
            option.MatchSome(action);

            return option;
        }

        /// <summary>
        ///     Evaluates a specified action if a value is present.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped va;ue.</typeparam>
        /// <param name="option">The <see cref="Option{T}" /> to match on.</param>
        /// <param name="action">The action to evaluate if the value is present.</param>
        /// <returns>The same <see cref="Option{T}" /> that was matched on.</returns>
        public static Option<T> MatchNoneContinue<T>(this Option<T> option, Action action)
        {
            option.MatchNone(action);

            return option;
        }
    }
}