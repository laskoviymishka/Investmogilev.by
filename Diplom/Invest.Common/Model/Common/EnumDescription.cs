using System;
using System.Reflection;

namespace Invest.Common.Model.Common
{
    public class EnumDescription : Attribute
    {
        private string _value;

        public EnumDescription(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        public static string GetEnumDescription(Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            EnumDescription[] attrs = fi.GetCustomAttributes(typeof(EnumDescription), false) as EnumDescription[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}