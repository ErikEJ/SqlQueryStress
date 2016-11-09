#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: ArgumentParser.cs
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
    internal abstract class ArgumentParser
    {
        public abstract ParserState Parse(IStringEnumerator argumentEnumerator, IOptionMap map, object options);

        public static ArgumentParser Create(string argument)
        {
            if (argument[0] == '-' && argument[1] == '-')
            {
                return new LongOptionParser();
            }
            if (argument[0] == '-')
            {
                return new OptionGroupParser();
            }
            return null;
        }

        public static bool IsInputValue(string argument)
        {
            return argument[0] != '-';
        }

        public static bool CompareShort(string argument, string option)
        {
            return string.CompareOrdinal(argument, "-" + option) == 0;
        }

        public static bool CompareLong(string argument, string option)
        {
            return string.CompareOrdinal(argument, "--" + option) == 0;
        }
    }
}
