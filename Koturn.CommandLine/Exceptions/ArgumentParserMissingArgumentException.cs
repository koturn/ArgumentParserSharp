using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace Koturn.CommandLine.Exceptions
{
    /// <summary>
    /// <para>An exception caused in <see cref="ArgumentParser"/>.</para>
    /// <para>This exception is thrown when detect an argument for argument-required option is not found.</para>
    /// </summary>
    [Serializable]
    public class ArgumentParserMissingArgumentException : ArgumentParserException
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserMissingArgumentException"/> class.
        /// </summary>
        public ArgumentParserMissingArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserMissingArgumentException"/> class with a short option name.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        public ArgumentParserMissingArgumentException(char shortOptName)
            : base("Missing argument of short option", shortOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserMissingArgumentException"/> class with a long option name.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        public ArgumentParserMissingArgumentException(string longOptName)
            : base("Missing argument of long option", longOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserMissingArgumentException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("This ctor is only for .NET Framework", DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserMissingArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Static memthods
        /// <summary>
        /// Throws <see cref="ArgumentParserMissingArgumentException"/>.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <exception cref="ArgumentParserMissingArgumentException">Always thrown</exception>
        [DoesNotReturn]
        public static void Throw(char shortOptName)
        {
            throw new ArgumentParserMissingArgumentException(shortOptName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentParserMissingArgumentException"/>.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <exception cref="ArgumentParserMissingArgumentException">Always thrown</exception>
        [DoesNotReturn]
        public static new void Throw(string longOptName)
        {
            throw new ArgumentParserMissingArgumentException(longOptName);
        }
        #endregion
    }
}
