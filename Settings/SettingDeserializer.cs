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
                Queue<string> nodes = GetSettingNodes(key);

                WalkNodes(nodes, model, settings[key]);
            }

            return model;
        }

        private void WalkNodes(Queue<string> nodes, object parent, string leafValue)
        {
            if (nodes.Count == 0)
                return;

            string currentNode = nodes.Dequeue();
            Type parentType = parent.GetType();
            PropertyInfo property = parentType.GetProperty(currentNode);
            bool notLeaf = nodes.Count > 0;

            if (notLeaf && property != null)
            {
                object currentParent = parent;
                parent = Activator.CreateInstance(property.PropertyType);
                property.SetValue(currentParent, parent);

                WalkNodes(nodes, parent, leafValue);
            }
            else if (property != null)
            {
                object convertedSettingValue = ConvertSettingValue(property, leafValue);
                property.SetValue(parent, convertedSettingValue);
            }
        }

        /// <summary>
        /// Get all nodes in setting structure.
        /// </summary>
        /// <param name="structure">The structure of the setting.</param>
        /// <returns></returns>
        private Queue<string> GetSettingNodes(string structure)
        {
            string[] nodes = structure.Split(new string[] { "." }, StringSplitOptions.None);

            return new Queue<string>(nodes);            
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