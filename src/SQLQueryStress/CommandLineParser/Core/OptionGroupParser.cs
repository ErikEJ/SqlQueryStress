#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: OptionGroupParser.cs
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
    sealed class OptionGroupParser : ArgumentParser
    {
        public sealed override ParserState Parse(IStringEnumerator argumentEnumerator, IOptionMap map, object options)
        {
            IStringEnumerator group = new CharEnumeratorEx(argumentEnumerator.Current.Substring(1));
            while (group.MoveNext())
            {
                OptionInfo option = map[group.Current];
                if (option == null)
                {
                    return ParserState.Failure;
                }

                option.IsDefined = true;

                if (!option.IsBoolean)
                {
                    if (argumentEnumerator.IsLast && group.IsLast)
                    {
                        return ParserState.Failure;
                    }

                    if (!group.IsLast)
                    {
                        if (option.SetValue(group.GetRemainingFromNext(), options))
                        {
                            return ParserState.Success;
                        }
                        else
                        {
                            return ParserState.Failure;
                        }
                    }

                    if (!argumentEnumerator.IsLast && !ArgumentParser.IsInputValue(argumentEnumerator.Next))
                    {
                        return ParserState.Failure;
                    }
                    else
                    {
                        if (option.SetValue(argumentEnumerator.Next, options))
                        {
                            return ParserState.Success | ParserState.MoveOnNextElement;
                        }
                        else
                        {
                            return ParserState.Failure;
                        }
                    }
                }
                else
                {
                    if (!group.IsLast && map[group.Next] == null)
                    {
                        return ParserState.Failure;
                    }
                    if (!option.SetValue(true, options))
                    {
                        return ParserState.Failure;
                    }
                }
            }
            return ParserState.Success;
        }
    }
}
