#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: OptionAttribute.cs
//
// Author:
//   Giacomo Stelluti Scala (gsscoder@ymail.com)
//
// Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

namespace CommandLine.Text
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Models an help text and collects related informations.
    /// You can assign it in place of a <see cref="System.String"/> instance, this is why
    /// this type lacks a method to add lines after the options usage informations;
    /// simple use a <see cref="System.Text.StringBuilder"/> or similar solutions.
    /// </summary>
    public class HelpText
    {
        #region Private Members
        private const int builderCapacity = 128;
        private readonly string heading;
        private string copyright;
        private readonly StringBuilder preOptionsHelp;
        private StringBuilder optionsHelp;
        private const string defaultRequiredWord = "Required.";
        #endregion

        private HelpText()
        {
            preOptionsHelp = new StringBuilder(builderCapacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.Text.HelpText"/> class
        /// specifying heading informations.
        /// </summary>
        /// <param name="heading">A string with heading information or
        /// an instance of <see cref="CommandLine.Text.HeadingInfo"/>.</param>
        /// <exception cref="System.ArgumentException">Thrown when parameter <paramref name="heading"/> is null or empty string.</exception>
        public HelpText(string heading)
            : this()
        {
            Validator.CheckIsNullOrEmpty(heading, nameof(heading));

            this.heading = heading;
        }

        /// <summary>
        /// Sets the copyright information string.
        /// You can directly assign a <see cref="CommandLine.Text.CopyrightInfo"/> instance.
        /// </summary>
        public void SetCopyright(string value)
        {
            Validator.CheckIsNullOrEmpty(value, nameof(value));
            copyright = value;
        }

        /// <summary>
        /// Adds a text line after copyright and before options usage informations.
        /// </summary>
        /// <param name="value">A <see cref="System.String"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when parameter <paramref name="value"/> is null or empty string.</exception>
        public void AddPreOptionsLine(string value)
        {
            AddLine(preOptionsHelp, value);
        }

        /// <summary>
        /// Adds a text block with options usage informations.
        /// </summary>
        /// <param name="options">The instance that collected command line arguments parsed with <see cref="CommandLine.Parser"/> class.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when parameter <paramref name="options"/> is null.</exception>
        public void AddOptions(object options)
        {
            AddOptions(options, defaultRequiredWord);
        }

        /// <summary>
        /// Adds a text block with options usage informations.
        /// </summary>
        /// <param name="options">The instance that collected command line arguments parsed with the <see cref="CommandLine.Parser"/> class.</param>
        /// <param name="requiredWord">The word to use when the option is required.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when parameter <paramref name="options"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when parameter <paramref name="requiredWord"/> is null or empty string.</exception>
        public void AddOptions(object options, string requiredWord)
        {
            Validator.CheckIsNull(options, nameof(options));
            Validator.CheckIsNullOrEmpty(requiredWord, nameof(requiredWord));

            IList<BaseOptionAttribute> optionList =
                                ReflectionUtil.RetrieveFieldAttributeList<BaseOptionAttribute>(options);

            HelpOptionAttribute optionHelp =
                                ReflectionUtil.RetrieveMethodAttributeOnly<HelpOptionAttribute>(options);
            if (optionHelp != null)
            {
                optionList.Add(optionHelp);
            }

            if (optionList.Count == 0)
            {
                return;
            }

            int maxLength = GetMaxLength(optionList);
            optionsHelp = new StringBuilder(builderCapacity);

            foreach (BaseOptionAttribute option in optionList)
            {
                optionsHelp.Append("  ");
                StringBuilder optionName = new StringBuilder(maxLength);
                if (option.HasShortName)
                {
                    optionName.Append(option.ShortName);
                    if (option.HasLongName)
                    {
                        optionName.Append(", ");
                    }
                }
                if (option.HasLongName)
                {
                    optionName.Append(option.LongName);
                }
                if (optionName.Length < maxLength)
                {
                    optionsHelp.Append(optionName.ToString().PadRight(maxLength));
                }
                else
                {
                    optionsHelp.Append(optionName.ToString());
                }
                optionsHelp.Append("\t");
                if (option.Required)
                {
                    optionsHelp.Append(requiredWord);
                    optionsHelp.Append(' ');
                }
                optionsHelp.Append(option.HelpText);
                optionsHelp.Append(Environment.NewLine);
            }
        }

        /// <summary>
        /// Returns the help informations as a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The <see cref="System.String"/> that contains the help informations.</returns>
        public override string ToString()
        {
            const int extraLength = 10;
            StringBuilder builder = new StringBuilder(heading.Length +
                                GetLength(copyright) + GetLength(preOptionsHelp) +
                                GetLength(optionsHelp) + extraLength);

            builder.Append(heading);
            if (!string.IsNullOrEmpty(copyright))
            {
                builder.Append(Environment.NewLine);
                builder.Append(copyright);
            }
            if (preOptionsHelp.Length > 0)
            {
                builder.Append(Environment.NewLine);
                builder.Append(preOptionsHelp.ToString());
            }
            if (optionsHelp != null && optionsHelp.Length > 0)
            {
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
                builder.Append(optionsHelp.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts the help informations to a <see cref="System.String"/>.
        /// </summary>
        /// <param name="info">This <see cref="CommandLine.Text.HelpText"/> instance.</param>
        /// <returns>The <see cref="System.String"/> that contains the help informations.</returns>
        public static implicit operator string(HelpText info)
        {
            return info.ToString();
        }

        private static void AddLine(StringBuilder builder, string value)
        {
            //Validator.CheckIsNullOrEmpty(value, "value");
            Validator.CheckIsNull(value, nameof(value));

            if (builder.Length > 0)
            {
                builder.Append(Environment.NewLine);
            }
            builder.Append(value);
        }

        private static int GetLength(string value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return value.Length;
            }
        }

        private static int GetLength(StringBuilder value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return value.Length;
            }
        }

        private static int GetMaxLength(IList<BaseOptionAttribute> optionList)
        {
            int length = 0;
            foreach (BaseOptionAttribute option in optionList)
            {
                int optionLenght = 0;
                bool hasShort = option.HasShortName;
                bool hasLong = option.HasLongName;
                if (hasShort)
                {
                    optionLenght += option.ShortName.Length;
                }
                if (hasLong)
                {
                    optionLenght += option.LongName.Length;
                }
                if (hasShort && hasLong)
                {
                    optionLenght += 2; // ", "
                }
                length = Math.Max(length, optionLenght);
            }
            return length;
        }
    }
}
