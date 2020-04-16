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

namespace CommandLine
{
    using System;

    /// <summary>
    /// Models an option specification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,
            AllowMultiple=false,
            Inherited=true)]
    public class OptionAttribute : BaseOptionAttribute
    {
        private string uniqueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.OptionAttribute"/> class.
        /// </summary>
        /// <param name="shortName">The short name of the option or null if not used.</param>
        /// <param name="longName">The long name of the option or null if not used.</param>
        public OptionAttribute(string shortName, string longName)
        {
            if (!string.IsNullOrEmpty(shortName))
            {
                this.uniqueName = shortName;
            }
            else if (!string.IsNullOrEmpty(longName))
            {
                this.uniqueName = longName;
            }

            if (this.uniqueName == null)
            {
                throw new InvalidOperationException();
            }

            this.ShortName = shortName;
            this.LongName = longName;
        }

#if UNIT_TESTS
        internal OptionInfo CreateOptionInfo()
        {
            return new OptionInfo(this.ShortName, this.LongName);
        }
#endif

        internal string UniqueName
        {
            get { return this.uniqueName; }
        }
    }
}
