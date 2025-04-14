using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace ArgumentParserSharp.Exceptions
{
    /// <summary>
    /// This exception is throwed when an argument is given to a non argument-required option.
    /// </summary>
    [Serializable]
    public class ArgumentParserDoesNotTakeArgumentException : ArgumentParserException
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserDoesNotTakeArgumentException"/> class.
        /// </summary>
        public ArgumentParserDoesNotTakeArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserDoesNotTakeArgumentException"/> class with a long option name.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="value">Value for long option.</param>
        public ArgumentParserDoesNotTakeArgumentException(string longOptName, string value)
            : base("An argument is given to non-argument required long option", longOptName + "=" + value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserDoesNotTakeArgumentException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("This ctor is only for .NET Framework", DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserDoesNotTakeArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Static memthods
        /// <summary>
        /// Throws <see cref="ArgumentParserDoesNotTakeArgumentException"/>.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="value">Value for long option.</param>
        /// <exception cref="ArgumentParserDoesNotTakeArgumentException">Always thrown</exception>
        [DoesNotReturn]
        public static new void Throw(string longOptName, string value)
        {
            throw new ArgumentParserDoesNotTakeArgumentException(longOptName, value);
        }
        #endregion
    }
}
