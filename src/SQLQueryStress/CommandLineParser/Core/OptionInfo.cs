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
        private readonly OptionAttribute attribute;
        private readonly FieldInfo field;
        private bool required;
        private string helpText;
        private bool isDefined;
        private string shortName;
        private string longName;
        
        private object setValueLock = new object();

        public OptionInfo(OptionAttribute attribute, FieldInfo field)
        {
            this.required = attribute.Required;
            this.helpText = attribute.HelpText;
            this.shortName = attribute.ShortName;
            this.longName = attribute.LongName;
            this.field = field;
            this.attribute = attribute;
        }

#if UNIT_TESTS
        internal OptionInfo(string shortName, string longName)
        {
            this.shortName = shortName;
            this.longName = longName;
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
            if (attribute is OptionListAttribute)
            {
                return this.SetValueList(value, options);
            }
            else
            {
                return this.SetValueScalar(value, options);
            }
        }

        public bool SetValueScalar(string value, object options)
        {
            try
            {
                if (this.field.FieldType.IsEnum)
                {
                    lock (this.setValueLock)
                    {
                        this.field.SetValue(options, Enum.Parse(this.field.FieldType, value, true));
                    }
                }
                else
                {
                    lock (this.setValueLock)
                    {
                        this.field.SetValue(options, Convert.ChangeType(value, this.field.FieldType, CultureInfo.InvariantCulture));
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
            lock (this.setValueLock)
            {
                this.field.SetValue(options, value);
                return true;
            }
        }

        public bool SetValueList(string value, object options)
        {
            lock (this.setValueLock)
            {
                field.SetValue(options, new List<string>());
                IList<string> fieldRef = (IList<string>)field.GetValue(options);
                string[] values = value.Split(((OptionListAttribute)this.attribute).Separator);
                for (int i = 0; i < values.Length; i++)
                {
                    fieldRef.Add(values[i]);
                }
                return true;
            }
        }

        public string ShortName
        {
            get { return this.shortName; }
        }

        public string LongName
        {
            get { return this.longName; }
        }

        public bool Required
        {
            get { return this.required; }
        }

        public string HelpText
        {
            get { return this.helpText; }
        }

        public bool IsBoolean
        {
            get { return this.field.FieldType == typeof(bool); }
        }

        public bool IsDefined
        {
            get { return this.isDefined; }
            set { this.isDefined = value; }
        }

        public bool HasBothNames
        {
            get { return (this.shortName != null && this.longName != null); }
        }
    }
}
