﻿using System;
using System.Runtime.Serialization;


namespace ArgumentParserSharp.Exceptions
{
    /// <summary>
    /// An exception caused in <see cref="ArgumentParser"/>.
    /// </summary>
    [Serializable]
    public class ArgumentParserException : Exception
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class.
        /// </summary>
        public ArgumentParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ArgumentParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class with
        /// a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public ArgumentParserException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class with a short option name.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="shortOptName">Short option name.</param>
        public ArgumentParserException(string message, char shortOptName)
            : base(message + ": -" + shortOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class with a long option name.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="longOptName">Long option name.</param>
        public ArgumentParserException(string message, string longOptName)
            : base(message + ": --" + longOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected ArgumentParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
