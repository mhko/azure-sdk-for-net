// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Serialization
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Deserializes JSON objects and arrays to .NET types instead of JObject and JArray.
    /// </summary>
    internal class BaseFieldConverter : JsonConverter
    {
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
            return typeof(BaseField).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JObject propertyBag = serializer.Deserialize<JObject>(reader);
            string name = propertyBag["name"].Value<string>();

            var tokenReader = new JTokenReader(propertyBag["type"]);
            DataType type = serializer.Deserialize<DataType>(tokenReader);

            if (type == DataType.Collection(DataType.ComplexType) || type == DataType.ComplexType)
            {
                bool isCollection = type == DataType.Collection(DataType.ComplexType);
                return new ComplexField(name, isCollection: isCollection);
            }
            else
            {
                bool key = propertyBag["key"].Value<bool>();
                bool searchable = propertyBag["searchable"].Value<bool>();
                bool filterable = propertyBag["filterable"].Value<bool>();
                bool retrievable = propertyBag["retrievable"].Value<bool>();
                bool sortable = propertyBag["sortable"].Value<bool>();
                bool facetable = propertyBag["facetable"].Value<bool>();

                tokenReader = new JTokenReader(propertyBag["indexAnalyzer"]);
                AnalyzerName indexAnalyzer = serializer.Deserialize<AnalyzerName>(tokenReader);
                tokenReader = new JTokenReader(propertyBag["searchAnalyzer"]);
                AnalyzerName searchAnalyzer = serializer.Deserialize<AnalyzerName>(tokenReader);
                tokenReader = new JTokenReader(propertyBag["analyzer"]);
                AnalyzerName analyzer = serializer.Deserialize<AnalyzerName>(tokenReader);

                string[] synonymMaps = propertyBag["synonymMaps"].Values<string>().ToArray();

                return new Field(name, type)
                {
                    IsKey = key,
                    IsSearchable = searchable,
                    IsFilterable = filterable,
                    IsRetrievable = retrievable,
                    IsSortable = sortable,
                    IsFacetable = facetable,
                    IndexAnalyzer = indexAnalyzer,
                    SearchAnalyzer = searchAnalyzer,
                    Analyzer = analyzer,
                    SynonymMaps = synonymMaps
                };
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
