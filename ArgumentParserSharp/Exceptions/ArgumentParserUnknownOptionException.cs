using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace ArgumentParserSharp.Exceptions
{
    /// <summary>
    /// <para>An exception caused in <see cref="ArgumentParser"/>.</para>
    /// <para>This exception is thrown when detect unknown option.</para>
    /// </summary>
    [Serializable]
    public class ArgumentParserUnknownOptionException : ArgumentParserException
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserUnknownOptionException"/> class.
        /// </summary>
        public ArgumentParserUnknownOptionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserUnknownOptionException"/> class with a short option name.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        public ArgumentParserUnknownOptionException(char shortOptName)
            : base("Unknown short option", shortOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserUnknownOptionException"/> class with a long option name.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        public ArgumentParserUnknownOptionException(string longOptName)
            : base("Unknown long option", longOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserUnknownOptionException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("This ctor is only for .NET Framework", DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserUnknownOptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Static memthods
        /// <summary>
        /// Throws <see cref="ArgumentParserUnknownOptionException"/>.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <exception cref="ArgumentParserUnknownOptionException">Always thrown</exception>
        [DoesNotReturn]
        public static void Throw(char shortOptName)
        {
            throw new ArgumentParserUnknownOptionException(shortOptName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentParserUnknownOptionException"/>.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <exception cref="ArgumentParserUnknownOptionException">Always thrown</exception>
        [DoesNotReturn]
        public static new void Throw(string longOptName)
        {
            throw new ArgumentParserUnknownOptionException(longOptName);
        }
        #endregion
    }
}
