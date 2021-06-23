using System;

namespace NL.Exceptions {

    /// <summary>
    ///     <see cref="Exception"/> thrown when a parameter contains a value that is not 
    ///     valid in the context of the throwing method.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value that caused the <see cref="Exception"/>.
    /// </typeparam>
    public class InvalidValueException<T> : Exception {
        /// <summary>
        ///     The value of the parameter that caused the <see cref="InvalidValueException{T}"/>.
        /// </summary>
        public T FoundValue { get; private set; }

        /// <summary>
        ///     Create an instance of <see cref="InvalidValueException{T}"/> that stores the
        ///     <paramref name="throwingValue"/> for public access.
        /// </summary>
        /// <param name="throwingValue">
        ///     The value of the parameter that caused the <see cref="InvalidValueException{T}"/>
        /// </param>
        /// <param name="throwingParamName">
        ///     The name of the parameter that caused the <see cref="InvalidValueException{T}"/>.
        /// </param>
        /// <example>
        ///     Example of proper use:
        ///     <code>
        ///     throw new InvalidValueException(throwingValue, nameof(throwingValue));
        ///     </code>
        /// </example>
        public InvalidValueException(T throwingValue, string throwingParamName)
            : base($"The found value \"{throwingValue}\" is not valid for the parameter {throwingParamName}.") {
            FoundValue = throwingValue;
        }

        /// <inheritdoc cref="InvalidValueException{T}"/>
        /// <param name="reason">
        ///     The reason why the value is invalid in the context this was thrown.
        /// </param>
        public InvalidValueException(T throwingValue, string throwingParamName, string reason)
            : base($"The found value \"{throwingValue}\" is not valid for the parameter {throwingParamName}.\nReason: {reason}") {
            FoundValue = throwingValue;
        }
    }
}
