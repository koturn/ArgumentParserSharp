using System;
using System.Runtime.Serialization;


namespace ArgumentParserSharp.Exceptions
{
    /// <summary>
    /// <para>An exception caused in <see cref="ArgumentParser"/>.</para>
    /// <para>This exception is thrown when get value from an argument-required option and the value is empty.</para>
    /// </summary>
    [Serializable]
    public class ArgumentParserValueEmptyException : ArgumentParserException
    {
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserValueEmptyException"/> class.
        /// </summary>
        public ArgumentParserValueEmptyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserValueEmptyException"/> class with a short option name.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        public ArgumentParserValueEmptyException(char shortOptName)
            : base("Short option value is empty", shortOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserValueEmptyException"/> class with a long option name.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        public ArgumentParserValueEmptyException(string longOptName)
            : base("Long option value is empty", longOptName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserValueEmptyException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ArgumentParserValueEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
