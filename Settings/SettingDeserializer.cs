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
                string[] nodes = GetSettingNodes(key);

                WalkNodes(settings, modelTypeInformation, key, nodes, model);
            }

            return model;
        }

        private void WalkNodes(Dictionary<string, string> settings, Type modelTypeInformation, string key, string[] nodes, object parent)
        {
            for (int index = 0; index < nodes.Length; index++)
            {
                PropertyInfo property = null;

                if (index == 0)
                {
                    property = modelTypeInformation.GetProperty(nodes[index]);
                }
                else if (parent != null)
                {
                    Type parentType = parent.GetType();
                    property = parentType.GetProperty(nodes[index]);
                }

                if (property != null && (index + 1) == nodes.Length)
                {
                    object convertedSettingValue = ConvertSettingValue(property, settings[key]);
                    property.SetValue(parent, convertedSettingValue);
                }
                else
                {
                    object currentParent = parent;
                    parent = Activator.CreateInstance(property.PropertyType);

                    if (index == 0)
                        property.SetValue(currentParent, parent);
                }
            }
        }

        /// <summary>
        /// Get all nodes in setting structure.
        /// </summary>
        /// <param name="structure">The structure of the setting.</param>
        /// <returns></returns>
        private string[] GetSettingNodes(string structure)
        {
            return structure.Split(new string[] { "." }, StringSplitOptions.None);
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