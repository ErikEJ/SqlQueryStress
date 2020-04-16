#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: CommandLineParser.cs
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

namespace CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Provides methods to parse command line arguments.
    /// Default implementation for <see cref="CommandLine.ICommandLineParser"/>.
    /// </summary>
    public class CommandLineParser : ICommandLineParser
    {
        private object valueListLock = new object();

        /// <summary>
        /// Parses a <see cref="System.String"/> array of command line arguments,
        /// setting values read in <paramref name="options"/> parameter instance.
        /// </summary>
        /// <param name="args">A <see cref="System.String"/> array of command line arguments.</param>
        /// <param name="options">An instance to receive values.
        /// Parsing rules are defined using <see cref="CommandLine.BaseOptionAttribute"/> derived types.</param>
        /// <returns>True if parsing process succeed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="args"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="options"/> is null.</exception>
        public virtual bool ParseArguments(string[] args, object options)
        {
            Validator.CheckIsNull(args, "args");
            Validator.CheckIsNull(options, "options");

            return ParseArgumentList(args, options);
        }

        /// <summary>
        /// Parses a <see cref="System.String"/> array of command line arguments,
        /// setting values read in <paramref name="options"/> parameter instance.
        /// This overloads allows you to specify a <see cref="System.IO.TextWriter"/>
        /// derived instance for write text messages.         
        /// </summary>
        /// <param name="args">A <see cref="System.String"/> array of command line arguments.</param>
        /// <param name="options">An instance to receive values.
        /// Parsing rules are defined using <see cref="CommandLine.BaseOptionAttribute"/> derived types.</param>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// usually <see cref="System.Console.Out"/>.</param>
        /// <returns>True if parsing process succeed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="args"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="options"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="helpWriter"/> is null.</exception>
        public virtual bool ParseArguments(string[] args, object options, TextWriter helpWriter)
        {
            Validator.CheckIsNull(args, "args");
            Validator.CheckIsNull(options, "options");
            Validator.CheckIsNull(helpWriter, "helpWriter");

            Pair<MethodInfo, HelpOptionAttribute> pair =
                                ReflectionUtil.RetrieveMethod<HelpOptionAttribute>(options);
            if (pair == null)
            {
                throw new InvalidOperationException();
            }
            if (ParseHelp(args, pair.Right) || !ParseArgumentList(args, options))
            {
                string helpText;
                HelpOptionAttribute.InvokeMethod(options, pair, out helpText);
                helpWriter.Write(helpText);
                return false;
            }
            return true;
        }

        private bool ParseArgumentList(string[] args, object options)
        {
            bool hadError = false;
            IOptionMap optionMap = OptionInfo.CreateMap(options);
            IList<string> valueList = ValueListAttribute.GetReference(options);
            ValueListAttribute vlAttr = ValueListAttribute.GetAttribute(options);

            IStringEnumerator arguments = new StringEnumeratorEx(args);
            while (arguments.MoveNext())
            {
                string argument = arguments.Current;
                if (argument != null && argument.Length > 0)
                {
                    ArgumentParser parser = ArgumentParser.Create(argument);
                    if (parser != null)
                    {
                        ParserState result = parser.Parse(arguments, optionMap, options);
                        if ((result & ParserState.Failure) == ParserState.Failure)
                        {
                            hadError = true;
                            break;
                        }
                        if ((result & ParserState.MoveOnNextElement) == ParserState.MoveOnNextElement)
                        {
                            arguments.MoveNext();
                        }
                    }
                    else if (valueList != null)
                    {
                        if (vlAttr.MaximumElements < 0)
                        {
                            lock (valueListLock)
                            {
                                valueList.Add(argument);
                            }
                        }
                        else if (vlAttr.MaximumElements == 0)
                        {
                            hadError = true;
                            break;
                        }
                        else
                        {
                            if (vlAttr.MaximumElements > valueList.Count)
                            {
                                lock (valueListLock)
                                {
                                    valueList.Add(argument);
                                }
                            }
                            else
                            {
                                hadError = true;
                                break;
                            }
                        }
                    }
                }
            }

            hadError |= !optionMap.EnforceRules();
            return !hadError;
        }

        private static bool ParseHelp(string[] args, HelpOptionAttribute helpOption)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (!string.IsNullOrEmpty(helpOption.ShortName))
                {
                    if (ArgumentParser.CompareShort(args[i], helpOption.ShortName))
                    {
                        return true;
                    }
                }
                if (!string.IsNullOrEmpty(helpOption.LongName))
                {
                    if (ArgumentParser.CompareLong(args[i], helpOption.LongName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
