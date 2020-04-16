#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: IncompatibleTypesException.cs
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
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the exception that is thrown when an attempt to assign incopatible types.
    /// This exception has only backward compatibility purpose, instead catch
    /// <see cref="CommandLine.ParserException"/>.
    /// The code will no more throw this type.
    /// </summary>
    [Obsolete]
    [Serializable]
    public sealed class IncompatibleTypesException : Exception, ISerializable
    {
        internal IncompatibleTypesException()
            : base()
        {
        }

        internal IncompatibleTypesException(string message)
            : base(message)
        {
        }

        internal IncompatibleTypesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal IncompatibleTypesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
