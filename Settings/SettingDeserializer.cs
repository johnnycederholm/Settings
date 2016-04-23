using System;
using System.Collections.Generic;
using System.Reflection;

namespace Settings
{
    /// <summary>
    /// Deserialize a dictionary of key/value pairs into a matching model definition.
    /// </summary>
    public class SettingDeserializer
    {
        public T Deserialize<T>(Dictionary<string, string> settings) where T : class, new()
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            T model = new T();
            Type modelTypeInformation = model.GetType();

            foreach (string key in settings.Keys)
            {
                PropertyInfo property = modelTypeInformation.GetProperty(key);
                object convertedSettingValue = ConvertSettingValue(property, settings[key]);

                property.SetValue(model, convertedSettingValue);
            }

            return model;
        }

        /// <summary>
        /// Try convert the setting value into a type matching the propertys type.
        /// </summary>
        /// <param name="property">Information about property.</param>
        /// <param name="settingValue">Value to convert.</param>
        private object ConvertSettingValue(PropertyInfo property, string settingValue)
        {
            TypeCode typeCode = Type.GetTypeCode(property.PropertyType);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return Convert.ToBoolean(settingValue);
                case TypeCode.Char:
                    return Convert.ToChar(settingValue);
                case TypeCode.DateTime:
                    return Convert.ToDateTime(settingValue);
                case TypeCode.Decimal:
                    return Convert.ToDecimal(settingValue);
                case TypeCode.Double:
                    return Convert.ToDouble(settingValue);
                case TypeCode.Int16:
                    return Convert.ToInt16(settingValue);
                case TypeCode.Int32:
                    return Convert.ToInt32(settingValue);
                case TypeCode.Int64:
                    return Convert.ToInt64(settingValue);
                case TypeCode.Single:
                    return Convert.ToSingle(settingValue);
                case TypeCode.String:
                    return settingValue;
                default:
                    throw new ArgumentException($"{settingValue} can't be converted to known type.");
            }
        }
    }
}