using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ArgumentParserSharp.Exceptions;


namespace ArgumentParserSharp
{
    /// <summary>
    /// Argument parser class.
    /// </summary>
    public class ArgumentParser
    {
        #region Static members
        /// <summary>
        /// A string which enable to convert to bool value, true.
        /// </summary>
        private static readonly string StringTrue;
        /// <summary>
        /// A string which enable to convert to bool value, false.
        /// </summary>
        private static readonly string StringFalse;
        /// <summary>
        /// String value converter dictionary.
        /// </summary>
        private static readonly Dictionary<Type, object> DefaultConverterDict;
        #endregion

        #region static ctor
        /// <summary>
        /// Initialize static members.
        /// </summary>
        static ArgumentParser()
        {
            StringTrue = true.ToString();
            StringFalse = false.ToString();
            DefaultConverterDict = new Dictionary<Type, object>()
            {
                {typeof(bool), (Func<string, bool>)bool.Parse},
                {typeof(sbyte), (Func<string, sbyte>)sbyte.Parse},
                {typeof(short), (Func<string, short>)short.Parse},
                {typeof(int), (Func<string, int>)int.Parse},
                {typeof(long), (Func<string, long>)long.Parse},
                {typeof(byte), (Func<string, byte>)byte.Parse},
                {typeof(ushort), (Func<string, ushort>)ushort.Parse},
                {typeof(uint), (Func<string, uint>)uint.Parse},
                {typeof(ulong), (Func<string, ulong>)ulong.Parse},
                {typeof(float), (Func<string, float>)float.Parse},
                {typeof(double), (Func<string, double>)double.Parse},
                {typeof(char), (Func<string, char>)char.Parse},
                {typeof(string), (Func<string, string>)(s => s)},
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name of this program.
        /// </summary>
        public string ProgName { get; set; }
        /// <summary>
        /// Rest of arguments.
        /// </summary>
        public List<string> Arguments { get; }
        /// <summary>
        /// Description for this program.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// String indent which used in <see cref="ShowUsage()"/>.
        /// </summary>
        public string IndentString { get; set; }
        #endregion

        #region Members
        /// <summary>
        /// Option items.
        /// </summary>
        private readonly List<OptionItem> _options;
        /// <summary>
        /// Dictionary for lookup option item with short name.
        /// </summary>
        private readonly Dictionary<char, OptionItem> _shortOptDict;
        /// <summary>
        /// Dictionary for lookup option item with long name.
        /// </summary>
        private readonly Dictionary<string, OptionItem> _longOptDict;
        #endregion

        #region Ctors
        /// <summary>
        /// Create argument parser with default program name.
        /// </summary>
        public ArgumentParser() :
            this(Environment.GetCommandLineArgs()[0])
        {
        }

        /// <summary>
        /// Create argument parser with specified program name.
        /// </summary>
        /// <param name="progName">Name of a program shown in help message.</param>
        public ArgumentParser(string progName)
        {
            ProgName = progName;
            IndentString = "  ";
            Arguments = new List<string>();
            _options = new List<OptionItem>();
            _shortOptDict = new Dictionary<char, OptionItem>();
            _longOptDict = new Dictionary<string, OptionItem>();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Add one option which has both short option and long option.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option.</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add(char shortOptName, string longOptName, OptionType optType, string? description = null, string? metavar = null, string? defaultValue = null)
        {
            var item = new OptionItem(shortOptName, longOptName, optType, description, metavar, defaultValue);
            _options.Add(item);
            _shortOptDict[shortOptName] = item;
            _longOptDict[longOptName] = item;
        }

        /// <summary>
        /// Add one option which has short option only.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option.</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add(char shortOptName, OptionType optType, string? description = null, string? metavar = null, string? defaultValue = null)
        {
            var item = new OptionItem(shortOptName, null, optType, description, metavar, defaultValue);
            _options.Add(item);
            _shortOptDict[shortOptName] = item;
        }

        /// <summary>
        /// Add one option which has long option only.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option.</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add(string longOptName, OptionType optType, string? description = null, string? metavar = null, string? defaultValue = null)
        {
            var item = new OptionItem('\0', longOptName, optType, description, metavar, defaultValue);
            _options.Add(item);
            _longOptDict[longOptName] = item;
        }

        /// <summary>
        /// <para>Add one option which has both short option and long option.</para>
        /// <para>It's possible to give default value in any type.</para>
        /// </summary>
        /// <typeparam name="T">Default value type (Assume this type parameter infered from <paramref name="defaultValue"/>).</typeparam>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option..</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add<T>(char shortOptName, string longOptName, OptionType optType, string description, string metavar, T defaultValue)
        {
            Add(shortOptName, longOptName, optType, description, metavar, defaultValue is null ? "" : defaultValue.ToString());
        }

        /// <summary>
        /// <para>Add one option which has short option only.</para>
        /// <para>It's possible to give default value in any type.</para>
        /// </summary>
        /// <typeparam name="T">Default value type. (Assume this type parameter infered from <paramref name="defaultValue"/>.)</typeparam>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option.</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add<T>(char shortOptName, OptionType optType, string description, string metavar, T defaultValue)
        {
            Add(shortOptName, optType, description, metavar, defaultValue is null ? "" : defaultValue.ToString());
        }

        /// <summary>
        /// <para>Add one option which has long option only.</para>
        /// <para>It's possible to give default value in any type.</para>
        /// </summary>
        /// <typeparam name="T">Default value type. (Assume this type parameter infered from <paramref name="defaultValue"/>)</typeparam>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="optType">Option type which indicates whether this option require an argument or not.</param>
        /// <param name="description">Description for this option.</param>
        /// <param name="metavar">Name of meta variable for this option. (This value is used in <see cref="ShowUsage()"/>)</param>
        /// <param name="defaultValue">Default value of this option.</param>
        public void Add<T>(string longOptName, OptionType optType, string description, string metavar, T defaultValue)
        {
            Add(longOptName, optType, description, metavar, defaultValue is null ? "" : defaultValue.ToString());
        }

        /// <summary>
        /// <para>Add one boolean option which has both short option and long option.</para>
        /// <para>This method is equivalent to <c>Add(char, string, OptionType.NoArgument, string)</c>.</para>
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="description">Description for this option.</param>
        public void Add(char shortOptName, string longOptName, string? description = null)
        {
            Add(shortOptName, longOptName, OptionType.NoArgument, description, null, StringFalse);
        }

        /// <summary>
        /// <para>Add one boolean option which has short option only.</para>
        /// <para>This method is equivalent to <c>Add(char, OptionType.NoArgument, string)</c>.</para>
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <param name="description">Description for this option.</param>
        public void Add(char shortOptName, string? description = null)
        {
            Add(shortOptName, OptionType.NoArgument, description, null, StringFalse);
        }

        /// <summary>
        /// <para>Add one boolean option which has long option only.</para>
        /// <para>This method is equivalent to <c>Add(string, OptionType.NoArgument, string)</c>.</para>
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="description">Description for this option.</param>
        public void Add(string longOptName, string? description = null)
        {
            Add(longOptName, OptionType.NoArgument, description, null, StringFalse);
        }

        /// <summary>
        /// Generate default help option.
        /// </summary>
        public void AddHelp()
        {
            var item = new OptionItem('h', "help", OptionType.NoArgument, "Show help and exit this program", "", StringFalse);
            _options.Add(item);
            _shortOptDict['h'] = item;
            _longOptDict["help"] = item;
        }

        /// <summary>
        /// Parse command-line arguments.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public void Parse(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    if (args[i].Length == 2)
                    {
                        Arguments.AddRange(args.Skip(i));
                        return;
                    }
                    i = ParseLongOption(args, i);
                }
                else if (args[i].StartsWith("-") && args[i].Length > 1)
                {
                    i = ParseShortOption(args, i);
                }
                else
                {
                    Arguments.Add(args[i]);
                }
            }
        }

        /// <summary>
        /// Check whether option has value or not.
        /// </summary>
        /// <param name="shortOptName">Short option name.</param>
        /// <returns>True if an option has value, otherwise false.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public bool HasValue(char shortOptName)
        {
            return !(GetValue(shortOptName) is null);
        }

        /// <summary>
        /// Check whether option has value or not.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <returns>True if an option has value, otherwise false.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public bool HasValue(string longOptName)
        {
            return !(GetValue(longOptName) is null);
        }

        /// <summary>
        /// Get option value with short name.
        /// </summary>
        /// <param name="shortOptName">Short name of option.</param>
        /// <returns>Option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public string? GetValue(char shortOptName)
        {
            return GetOptionItem(shortOptName).Value;
        }

        /// <summary>
        /// Get option value with short name and convert the value using default primitive type converter.
        /// </summary>
        /// <typeparam name="T">Converted type. (Assume primitive type and string only)</typeparam>
        /// <param name="shortOptName">Short option name.</param>
        /// <returns>Converted option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        /// <exception cref="ArgumentParserValueEmptyException">Throw if option value is empty.</exception>
        public T GetValue<T>(char shortOptName)
        {
            var value = GetValue(shortOptName);
            if (value is null)
            {
                ArgumentParserValueEmptyException.Throw(shortOptName);
            }
            return ((Func<string, T>)DefaultConverterDict[typeof(T)])(value);
        }

        /// <summary>
        /// Get option value with short name and convert the value using specified type converter.
        /// </summary>
        /// <typeparam name="T">Converted type. (This type parameter is infered from <paramref name="convert"/>)</typeparam>
        /// <param name="shortOptName">Short option name.</param>h
        /// <param name="convert">String value converter.</param>
        /// <returns>Converted option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public T GetValue<T>(char shortOptName, Func<string?, T> convert)
        {
            return convert(GetValue(shortOptName));
        }

        /// <summary>
        /// Get option value with long name.
        /// </summary>
        /// <param name="longOptName">Long option name.</param>
        /// <returns>Option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public string? GetValue(string longOptName)
        {
            return GetOptionItem(longOptName).Value;
        }

        /// <summary>
        /// Get option value with long name and convert the value using default primitive type converter.
        /// </summary>
        /// <typeparam name="T">Converted type. (Assume primitive type and string only)</typeparam>
        /// <param name="longOptName">Long option name.</param>
        /// <returns>Converted option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        /// <exception cref="ArgumentParserValueEmptyException">Throw if option value is empty.</exception>
        public T GetValue<T>(string longOptName)
        {
            var value = GetValue(longOptName);
            if (value is null)
            {
                ArgumentParserValueEmptyException.Throw(longOptName);
            }
            return ((Func<string, T>)DefaultConverterDict[typeof(T)])(value);
        }

        /// <summary>
        /// Get option value with long name and convert the value using specified type converter.
        /// </summary>
        /// <typeparam name="T">Converted type. (This type parameter is infered from <paramref name="convert"/>)</typeparam>
        /// <param name="longOptName">Long option name.</param>
        /// <param name="convert">String value converter.</param>
        /// <returns>Converted option value.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        public T GetValue<T>(string longOptName, Func<string?, T> convert)
        {
            return convert(GetValue(longOptName));
        }

        /// <summary>
        /// Show usage using <see cref="Console.Out"/>.
        /// </summary>
        public void ShowUsage()
        {
            ShowUsage(Console.Out);
        }

        /// <summary>
        /// Show usage using specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">TextWriter to output message.</param>
        public void ShowUsage(TextWriter writer)
        {
            if (!(Description is null))
            {
                writer.WriteLine(Description + Environment.NewLine);
            }
            writer.WriteLine(
                "[Usage]" + Environment.NewLine
                + ProgName + " [Options ...] [Arguments ...]" + Environment.NewLine + Environment.NewLine
                + "[Options]");
            var indentString = IndentString ?? "";
            foreach (var item in _options)
            {
                writer.Write(indentString);
                if (item.LongOptName is null)
                {
                    ShowShortOptionDescription(writer, item);
                }
                else if (item.ShortOptName == '\0')
                {
                    ShowLongOptionDescription(writer, item);
                }
                else
                {
                    ShowShortOptionDescription(writer, item);
                    writer.Write(", ");
                    ShowLongOptionDescription(writer, item);
                }
                writer.WriteLine(Environment.NewLine + indentString + indentString + item.Description);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Parse one short option.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <param name="idx">Current index of parsing.</param>
        /// <returns>Parse finished index. (<c>idx</c> or <c>idx + 1</c>)</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        /// <exception cref="ArgumentParserMissingArgumentException">Throw if argument-required option value is not found.</exception>
        private int ParseShortOption(string[] args, int idx)
        {
            var arg = args[idx];
            for (int i = 1; i < arg.Length; i++)
            {
                var shortOptName = arg[i];
                var item = GetOptionItem(shortOptName);
                if (item.OptType == OptionType.NoArgument)
                {
                    item.Value = StringTrue;
                }
                else if (i == arg.Length - 1)
                {
                    if (idx + 1 >= args.Length)
                    {
                        ArgumentParserMissingArgumentException.Throw(shortOptName);
                    }
                    item.Value = args[idx + 1];
                    return idx + 1;
                }
                else
                {
                    item.Value = arg.Substring(i + 1);
                    return idx;
                }
            }
            return idx;
        }

        /// <summary>
        /// Parse one long option.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <param name="idx">Current index of parsing.</param>
        /// <returns>Parse finished index. (<c>idx</c> or <c>idx + 1</c>)</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        /// <exception cref="ArgumentParserMissingArgumentException">Throw if argument-required option value is not found.</exception>
        /// <exception cref="ArgumentParserAmbiguousOptionException">Throw if unknown a command-line is now resolve to one long option uniquely.</exception>
        /// <exception cref="ArgumentParserDoesNotTakeArgumentException">Throw if non argument-required option is given an argument.</exception>
        private int ParseLongOption(string[] args, int idx)
        {
            SplitFirstPos(args[idx].Substring(2), '=', out var longOptName, out var value);
            var items = GetOptionItems(longOptName);
            if (items.Length > 1)
            {
                ArgumentParserAmbiguousOptionException.Throw(longOptName);
            }
            var item = items[0];
            switch (item.OptType)
            {
                case OptionType.NoArgument:
                    if (!(value is null))
                    {
                        ArgumentParserDoesNotTakeArgumentException.Throw(longOptName, value);
                    }
                    item.Value = StringTrue;
                    return idx;
                case OptionType.OptionalArgument:
                    item.Value = value ?? StringTrue;
                    return idx;
                case OptionType.RequiredArgument:
                    if (value is null)
                    {
                        if (idx + 1 >= args.Length)
                        {
                            ArgumentParserMissingArgumentException.Throw(longOptName);
                        }
                        item.Value = args[idx + 1];
                        return idx + 1;
                    }
                    else
                    {
                        item.Value = value;
                        return idx;
                    }
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Get <see cref="OptionItem"/> with short name.
        /// </summary>
        /// <param name="shortOptName">Short name of option.</param>
        /// <returns><see cref="OptionItem"/>.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        private OptionItem GetOptionItem(char shortOptName)
        {
            if (!_shortOptDict.TryGetValue(shortOptName, out var optItem))
            {
                ArgumentParserUnknownOptionException.Throw(shortOptName);
            }
            return optItem;
        }

        /// <summary>
        /// Get <see cref="OptionItem"/> with long name.
        /// </summary>
        /// <param name="longOptName">Long name of option.</param>
        /// <returns><see cref="OptionItem"/>.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        private OptionItem GetOptionItem(string longOptName)
        {
            if (!_longOptDict.TryGetValue(longOptName, out var optItem))
            {
                ArgumentParserUnknownOptionException.Throw(longOptName);
            }
            return optItem;
        }

        /// <summary>
        /// Get <see cref="OptionItem"/> with forward matched long name.
        /// </summary>
        /// <param name="longOptName">Long name of option.</param>
        /// <returns>Array of matched <see cref="OptionItem"/>s.</returns>
        /// <exception cref="ArgumentParserUnknownOptionException">Throw if unknown option is specified.</exception>
        private OptionItem[] GetOptionItems(string longOptName)
        {
            var optItems = _longOptDict.Where(pair => pair.Key.StartsWith(longOptName))
                .Select(pair => pair.Value)
                .ToArray();
            if (optItems.Length == 0)
            {
                ArgumentParserUnknownOptionException.Throw(longOptName);
            }
            return optItems;
        }
        #endregion

        #region Private static methods
        /// <summary>
        /// Split string at the first position of <paramref name="ch"/>.
        /// </summary>
        /// <param name="str">Target string.</param>
        /// <param name="ch">Separator character.</param>
        /// <param name="first">First part of separated string. If target string doesn't have a character <paramref name="ch"/>, store <paramref name="str"/> to this variable</param>
        /// <param name="second">Second part of separated string. If target string doesn't have a character <paramref name="ch"/>, store <c>null</c> to this variable</param>
        private static void SplitFirstPos(string str, char ch, out string first, out string? second)
        {
            var pos = str.IndexOf(ch);
            if (pos == -1)
            {
                first = str;
                second = null;
            }
            else
            {
                first = str.Substring(0, pos);
                second = str.Substring(pos + 1);
            }
        }

        /// <summary>
        /// Show description of a short option.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> instance to output.</param>
        /// <param name="item">Option item of short option.</param>
        private static void ShowShortOptionDescription(TextWriter writer, OptionItem item)
        {
            writer.Write("-" + item.ShortOptName);
            if (item.OptType != OptionType.NoArgument)
            {
                writer.Write(" " + item.Metavar);
            }
        }

        /// <summary>
        /// Show description of a long option.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> instance to output.</param>
        /// <param name="item">Option item of long option.</param>
        private static void ShowLongOptionDescription(TextWriter writer, OptionItem item)
        {
            writer.Write("--" + item.LongOptName);
            switch (item.OptType)
            {
                case OptionType.OptionalArgument:
                    writer.Write("[=" + item.Metavar + "]");
                    break;
                case OptionType.RequiredArgument:
                    writer.Write("=" + item.Metavar);
                    break;
            }
        }
        #endregion
    }
}
