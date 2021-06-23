using System;

namespace NL.Exceptions {

    /// <summary>
    ///		Similar to <see cref="NullReferenceException"/>, but specific for the type
    ///		<see cref="Delegate"/> and the extensions <see cref="Action"/> and 
    ///		<see cref="Func{TResult}"/>.
    /// </summary>
    public class NullDelegateException : Exception {
        /// <summary>
        ///		Sets a generic message for this kind of <see cref="Exception"/>.
        /// </summary>
        public NullDelegateException()
            : base($"The delegate could not be invoked because it was null.") { }

        /// <summary>
        ///		Sets a generic message for this kind of <see cref="Exception"/> and
        ///		specifies the <see langword="null"/> parameter's name.
        /// </summary>
        public NullDelegateException(string parameterName)
            : base($"The delegate \"{parameterName}\" could not be invoked because it was null.") { }
    }

}
