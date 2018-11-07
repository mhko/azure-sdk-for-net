// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Serialization
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Models;
    using Spatial;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Deserializes JSON objects and arrays to .NET types instead of JObject and JArray.
    /// </summary>
    internal class DocumentConverter : JsonConverter
    {
        private static readonly string[] EmptyStringArray = new string[0];

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Document).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var result = new Document();
            JObject bag = serializer.Deserialize<JObject>(reader);

            foreach (JProperty field in bag.Properties())
            {
                if (field.Name.StartsWith("@search", StringComparison.Ordinal))
                {
                    continue;
                }

                object value;
                if (field.Value == null)
                {
                    value = null;
                }
                else if (field.Value is JArray)
                {
                    JArray array = (JArray)field.Value;

                    if (array.Count == 0)
                    {
                        // Treat as string arrays, for backward compatibility.
                        value = EmptyStringArray;
                    }
                    else
                    {
                        value = ConvertArray(array, serializer);
                    }
                }
                else if (field.Value is JObject)
                {
                    // TODO, nateko, revisit this try-parse chaining
                    JObject jobject = field.Value as JObject;
                    try
                    {
                        var tokenReader = new JTokenReader(jobject as JObject);
                        value = serializer.Deserialize<GeographyPoint>(tokenReader);
                    }
                    catch (JsonSerializationException)
                    {
                        var tokenReader = new JTokenReader(jobject as JObject);
                        value = serializer.Deserialize<Document>(tokenReader);
                    }
                }
                else
                {
                    value = field.Value.ToObject(typeof(object), serializer);
                }

                result[field.Name] = value;
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static object ConvertArray(JArray array, JsonSerializer serializer)
        {
            if (array.All(t => t.Type == JTokenType.String || t.Type == JTokenType.Null))
            {
                return array.Select(t => t.Value<string>()).ToArray();
            }
            else if (array.All(t => t.Type == JTokenType.Boolean))
            {
                return array.Select(t => t.Value<bool>()).ToArray();
            }
            else if (array.All(t => t.Type == JTokenType.Integer))
            {
                return array.Select(t => t.Value<int>()).ToArray();
            }
            else if (array.All(t => t.Type == JTokenType.Float))
            {
                return array.Select(t => t.Value<double>()).ToArray();
            }
            else if (array.All(t => t.Type == JTokenType.Object))
            {
                // TODO, nateko, revisit this try-parse chaining 
                try
                {
                    var list = new List<GeographyPoint>();
                    foreach (var obj in array)
                    {
                        var tokenReader = new JTokenReader(obj as JObject);
                        list.Add(serializer.Deserialize<GeographyPoint>(tokenReader));
                    }
                    return list.ToArray();
                }
                catch (JsonSerializationException)
                {
                    var list = new List<Document>();
                    foreach (var obj in array)
                    {
                        var tokenReader = new JTokenReader(obj as JObject);
                        list.Add(serializer.Deserialize<Document>(tokenReader));
                    }
                    return list.ToArray();
                }
            }
            else
            {
                return array.Select(t => t.ToObject(typeof(object), serializer)).ToArray();
            }
        }
    }
}
