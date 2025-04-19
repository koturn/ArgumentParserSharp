namespace Koturn.CommandLine
{
    /// <summary>
    /// One option item.
    /// </summary>
    /// <remarks>
    /// Primary ctor: Create one option item.
    /// </remarks>
    /// <param name="shortOptName">Short option name.</param>
    /// <param name="longOptName">Long option name.</param>
    /// <param name="optType">Option type.</param>
    /// <param name="description">Description for this option.</param>
    /// <param name="metavar">Name of meta variable for option parameter.</param>
    /// <param name="defaultValue">Default value of this option.</param>
    public class OptionItem(char shortOptName, string? longOptName, OptionType optType, string? description = null, string? metavar = null, string? defaultValue = null)
    {
        #region Properties
        /// <summary>
        /// Short option name.
        /// </summary>
        public char ShortOptName { get; } = shortOptName;
        /// <summary>
        /// Long option name.
        /// </summary>
        public string? LongOptName { get; } = longOptName;
        /// <summary>
        /// Description for this option.
        /// </summary>
        public string? Description { get; } = description;
        /// <summary>
        /// Name of meta variable for option parameter.
        /// </summary>
        public string? Metavar { get; } = metavar;
        /// <summary>
        /// Option type.
        /// </summary>
        public OptionType OptType { get; } = optType;
        /// <summary>
        /// Value of this option.
        /// </summary>
        public string? Value { get; set; } = defaultValue;
        #endregion
    }


    /// <summary>
    /// This enumeration indicates whether an option requires an argument or not.
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// Mean that the option doesn't require an argument.
        /// </summary>
        NoArgument,
        /// <summary>
        /// Mean that the option requires an argument.
        /// </summary>
        RequiredArgument,
        /// <summary>
        /// <para>Mean that the option may or may not requires an argument.</para>
        /// <para>In short option, this constant is equivalent to <see cref="RequiredArgument"/>.</para>
        /// <para>But in long option, you don't have to give argument the option.</para>
        /// <para><c>--option</c>, <c>--option=arg</c></para>
        /// </summary>
        OptionalArgument
    }
}
