#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: StringEnumeratorEx.cs
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

    sealed class StringEnumeratorEx : IStringEnumerator
    {
        private string[] data;
        private int index;
        private int endIndex;

        public StringEnumeratorEx(string[] value)
        {
            Validator.CheckIsNull(value, "value");

            this.data = value;
            this.index = -1;
            this.endIndex = value.Length;
        }

        public string Current
        {
            get
            {
                if (this.index == -1)
                {
                    throw new InvalidOperationException();
                }
                if (this.index >= this.endIndex)
                {
                    throw new InvalidOperationException();
                }
                return this.data[this.index];
            }
        }

        public string Next
        {
            get
            {
                if (this.index == -1)
                {
                    throw new InvalidOperationException();
                }
                if (this.index > this.endIndex)
                {
                    throw new InvalidOperationException();
                }
                if (this.IsLast)
                {
                    return null;
                }
                return this.data[this.index + 1];
            }
        }

        public bool IsLast
        {
            get { return this.index == this.endIndex - 1; }
        }

        public void Reset()
        {
            this.index = -1;
        }

        public bool MoveNext()
        {
            if (this.index < this.endIndex)
            {
                this.index++;
                return this.index < this.endIndex;
            }
            return false;
        }

        public string GetRemainingFromNext()
        {
            throw new NotImplementedException();
        }
    }
}
