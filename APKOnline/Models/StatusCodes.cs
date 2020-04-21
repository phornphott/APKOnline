using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace APKOnline.Models
{
    public class Result
    {
        public int StatusCode { get; set; }
        public string Messages { get; set; }
        public int Records { get; set; }
        public object Results { get; set; }
    }

    public enum StatusCodes
    {
        [EnumValue("Connection Error.")]
        Connection = 99,
        [EnumValue("Success.")]
        Succuss = 1,
        [EnumValue("Error.")]
        Error = 2
    }

    public class EnumValue : System.Attribute
    {
        private string _value;
        public EnumValue(string value)
        {
            _value = value;
        }
        public string Value
        {
            get { return _value; }
        }
    }

    public static class EnumString
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            EnumValue[] attrs = fi.GetCustomAttributes(typeof(EnumValue), false) as EnumValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}