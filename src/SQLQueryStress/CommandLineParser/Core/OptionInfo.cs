#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: OptionInfo.cs
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
    using System.Globalization;
    using System.Reflection;

    sealed class OptionInfo
    {
        private readonly OptionAttribute _attribute;
        private readonly FieldInfo _field;
        private readonly object setValueLock = new object();

        public OptionInfo(OptionAttribute attribute, FieldInfo field)
        {
            Required = attribute.Required;
            HelpText = attribute.HelpText;
            ShortName = attribute.ShortName;
            LongName = attribute.LongName;
            _field = field;
            _attribute = attribute;
        }

#if UNIT_TESTS
        internal OptionInfo(string shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }
#endif
        public static IOptionMap CreateMap(object target)
        {
            IList<Pair<FieldInfo, OptionAttribute>> list = ReflectionUtil.RetrieveFieldList<OptionAttribute>(target);
            IOptionMap map = new OptionMap(list.Count);
            foreach (Pair<FieldInfo, OptionAttribute> pair in list)
            {
                map[pair.Right.UniqueName] = new OptionInfo(pair.Right, pair.Left);
            }
            return map;
        }

        public bool SetValue(string value, object options)
        {
            if (_attribute is OptionListAttribute)
            {
                return SetValueList(value, options);
            }
            else
            {
                return SetValueScalar(value, options);
            }
        }

        public bool SetValueScalar(string value, object options)
        {
            try
            {
                if (_field.FieldType.IsEnum)
                {
                    lock (setValueLock)
                    {
                        _field.SetValue(options, Enum.Parse(_field.FieldType, value, true));
                    }
                }
                else
                {
                    lock (setValueLock)
                    {
                        _field.SetValue(options, Convert.ChangeType(value, _field.FieldType, CultureInfo.InvariantCulture));
                    }
                }
            }
            catch (InvalidCastException) // Convert.ChangeType
            {
                return false;
            }
            catch (FormatException) // Convert.ChangeType
            {
                return false;
            }
            catch (ArgumentException) // Enum.Parse
            {
                return false;
            }
            return true;
        }

        public bool SetValue(bool value, object options)
        {
            lock (setValueLock)
            {
                _field.SetValue(options, value);
                return true;
            }
        }

        public bool SetValueList(string value, object options)
        {
            lock (setValueLock)
            {
                _field.SetValue(options, new List<string>());
                IList<string> fieldRef = (IList<string>)_field.GetValue(options);
                string[] values = value.Split(((OptionListAttribute)_attribute).Separator);
                for (int i = 0; i < values.Length; i++)
                {
                    fieldRef.Add(values[i]);
                }
                return true;
            }
        }

        public string ShortName { get; }

        public string LongName { get; }

        public bool Required { get; }

        public string HelpText { get; }

        public bool IsBoolean
        {
            get { return _field.FieldType == typeof(bool); }
        }

        public bool IsDefined { get; set; }

        public bool HasBothNames
        {
            get { return ShortName != null && LongName != null; }
        }
    }
}
