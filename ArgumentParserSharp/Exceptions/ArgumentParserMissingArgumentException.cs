﻿using System;
using System.Runtime.Serialization;


namespace ArgumentParserSharp.Exceptions
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
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserMissingArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
