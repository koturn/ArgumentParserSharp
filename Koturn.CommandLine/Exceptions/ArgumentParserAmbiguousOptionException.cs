using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace Koturn.CommandLine.Exceptions
{
    /// <summary>
    /// <para>An exception caused in <see cref="ArgumentParser"/>.</para>
    /// <para>This exception is thrown when the omitted option name can not be resolved uniquely.</para>
    /// <para>For example, suppose that ArgumentParser can recognize long option <c>--foobarbuz</c> and <c>--foobazbar</c>.</para>
    /// <para>A command line argument <c>--foobar</c> can resolve <c>--foobarbuz</c> uniquely but <c>--foobar</c> can resolve
    /// <c>--foobarbuz</c> or <c>--foobazbar</c>.</para>
    /// </summary>
    [Serializable]
    public class ArgumentParserAmbiguousOptionException : ArgumentParserException
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserAmbiguousOptionException"/> class.
        /// </summary>
        public ArgumentParserAmbiguousOptionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserAmbiguousOptionException"/> class with a long option name.
        /// </summary>
        /// <param name="longOptName">Long option name</param>
        public ArgumentParserAmbiguousOptionException(string longOptName)
            : base("Ambiguous long option", longOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserAmbiguousOptionException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("This ctor is only for .NET Framework", DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserAmbiguousOptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Static memthods
        /// <summary>
        /// Throws <see cref="ArgumentParserAmbiguousOptionException"/>.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <exception cref="ArgumentParserAmbiguousOptionException">Always thrown</exception>
        [DoesNotReturn]
        public static new void Throw(string longOptName)
        {
            throw new ArgumentParserAmbiguousOptionException(longOptName);
        }
        #endregion
    }
}
