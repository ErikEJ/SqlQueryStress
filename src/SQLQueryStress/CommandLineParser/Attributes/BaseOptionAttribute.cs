#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: BaseOptionAttribute.cs
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

    /// <summary>
    /// Provides base properties for creating an attribute, used to define rules for command line parsing.
    /// </summary>
    public abstract class BaseOptionAttribute : Attribute
    {
        private string shortName;
        private string longName;
        private bool required;
        private string helpText;

        /// <summary>
        /// Short name of this command line option. This name is usually a single character.
        /// </summary>
        public string ShortName
        {
            get { return shortName; }
            internal set { shortName = value; }
        }

        /// <summary>
        /// Long name of this command line option. This name is usually a single english word.
        /// </summary>
        public string LongName
        {
            get { return longName; }
            internal set { longName = value; }
        }

        /// <summary>
        /// True if this command line option is required.
        /// </summary>
        public virtual bool Required
        {
            get { return required; }
            set { required = value; }
        }

        internal bool HasShortName
        {
            get { return !string.IsNullOrEmpty(shortName); }
        }

        internal bool HasLongName
        {
            get { return !string.IsNullOrEmpty(longName); }
        }

        /// <summary>
        /// A short description of this command line option. Usually a sentence summary. 
        /// </summary>
        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; }
        }
    }
}
