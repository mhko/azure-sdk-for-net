// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Serialization
{
    using System;
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Deserializes JSON objects and arrays to .NET types instead of JObject and JArray.
    /// </summary>
    internal class ComplexFieldConverter : JsonConverter
    {
        private static readonly string[] EmptyStringArray = new string[0];

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ComplexField) == objectType;
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value.GetType() == typeof(ComplexField))
            {
                var complexField = (ComplexField)value;

                string fieldType = "Edm.ComplexType"; //DataType.ComplexType.ToString();
                if (complexField.IsCollection)
                {
                    fieldType = "Collection(" + fieldType + ")";
                }

                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(complexField.Name);
                writer.WritePropertyName("type");
                writer.WriteValue(fieldType);
                writer.WriteEndObject();
            } else {
                serializer.Serialize(writer, value);
            }
        }
    }
}
