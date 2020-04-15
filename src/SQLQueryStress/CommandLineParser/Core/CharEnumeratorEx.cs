#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: CharEnumeratorEx.cs
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

    internal sealed class CharEnumeratorEx : IStringEnumerator
    {
        private string currentElement;
        private int index;
        private readonly string data;

        public CharEnumeratorEx(string value)
        {
            Validator.CheckIsNullOrEmpty(value, nameof(value));

            data = value;
            index = -1;
        }

        public string Current
        {
            get
            {
                if (index == -1)
                {
                    throw new InvalidOperationException();
                }
                if (index >= data.Length)
                {
                    throw new InvalidOperationException();
                }
                return currentElement;
            }
        }

        public string Next
        {
            get
            {
                if (index == -1)
                {
                    throw new InvalidOperationException();
                }
                if (index > data.Length)
                {
                    throw new InvalidOperationException();
                }
                if (IsLast)
                {
                    return null;
                }
                return data.Substring(index + 1, 1);
            }
        }

        public bool IsLast
        {
            get { return index == data.Length - 1; }
        }

        public void Reset()
        {
            index = -1;
        }

        public bool MoveNext()
        {
            if (index < (data.Length - 1))
            {
                index++;
                currentElement = data.Substring(index, 1);
                return true;
            }
            index = data.Length;
            return false;
        }

        public string GetRemainingFromNext()
        {
            if (index == -1)
            {
                throw new InvalidOperationException();
            }
            if (index > data.Length)
            {
                throw new InvalidOperationException();
            }
            return data.Substring(index + 1);
        }
    }
}
