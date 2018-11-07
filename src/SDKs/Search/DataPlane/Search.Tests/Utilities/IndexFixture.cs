// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using Newtonsoft.Json;

    public class IndexFixture : SearchServiceFixture
    {
        public string IndexName { get; private set; }

        public override void Initialize(MockContext context)
        {
            base.Initialize(context);

            SearchServiceClient searchClient = this.GetSearchServiceClient();

            IndexName = SearchTestUtilities.GenerateName();

            // This is intentionally a different index definition than the one returned by IndexTests.CreateTestIndex().
            // That index is meant to exercise serialization of the index definition itself, while this one is tuned
            // more for exercising document serialization, indexing, and querying operations.
            var index =
                new Index()
                {
                    Name = IndexName,
                    Fields = new List<BaseField>()
                    {
                        new Field("hotelId", DataType.String) { IsKey = true, IsFilterable = true, IsSortable = true, IsFacetable = true, IsRetrievable = true},
                        new Field("baseRate", DataType.Double) { IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("description", DataType.String) { IsSearchable = true },
                        new Field("descriptionFr", DataType.String) { IsSearchable = true, Analyzer = AnalyzerName.FrLucene },
                        new Field("hotelName", DataType.String) { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("category", DataType.String) { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("tags", DataType.Collection(DataType.String)) { IsSearchable = true, IsFilterable = true, IsFacetable = true },
                        new Field("parkingIncluded", DataType.Boolean) { IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("smokingAllowed", DataType.Boolean) { IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("lastRenovationDate", DataType.DateTimeOffset) { IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("rating", DataType.Int32) { IsFilterable = true, IsSortable = true, IsFacetable = true },
                        new Field("pastRatings", DataType.Collection(DataType.Double)) { IsKey = false, IsSearchable = false, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true  },
                        new Field("pastAwards", DataType.Collection(DataType.Int32)) { IsKey = false, IsSearchable = false, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true  },
                        new Field("location", DataType.GeographyPoint) { IsFilterable = true, IsSortable = true, IsRetrievable = true },
                        new ComplexField("address") {},
                        new Field("address/street", DataType.String) { IsKey = false, IsSearchable = true, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true },
                        new Field("address/city", DataType.String) { IsKey = false, IsSearchable = true, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true },
                        new ComplexField("rooms", isCollection: true),
                        new Field("rooms/roomId", DataType.String) { IsKey = false, IsSearchable = true, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true  },
                        new Field("rooms/type", DataType.String) { IsKey = false, IsSearchable = true, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true  },
                        new Field("rooms/baseRate", DataType.Double) { IsFilterable = true, IsFacetable = true },
                        new Field("rooms/sleepCount", DataType.Int32) { IsFilterable = true, IsFacetable = true }
                    },
                    Suggesters = new[]
                    {
                        new Suggester(
                            name: "sg",
                            sourceFields: new[] { "description", "hotelName" })
                    },
                    ScoringProfiles = new[]
                    {
                        new ScoringProfile("nearest")
                        {
                            FunctionAggregation = ScoringFunctionAggregation.Sum,
                            Functions = new[]
                            {
                                new DistanceScoringFunction("location", 2, new DistanceScoringParameters("myloc", 100))
                            }
                        }
                    }
                };

            searchClient.Indexes.Create(index);

            // Give the index time to stabilize before running tests.
            // TODO: Remove this workaround once the retry hang bug is fixed.
            TestUtilities.Wait(TimeSpan.FromSeconds(20));
        }

        public SearchIndexClient GetSearchIndexClient(string indexName = null, params DelegatingHandler[] handlers)
        {
            indexName = indexName ?? IndexName;
            return GetSearchIndexClientForKey(indexName, PrimaryApiKey);
        }

        public SearchIndexClient GetSearchIndexClientForQuery(
            string indexName = null,
            params DelegatingHandler[] handlers)
        {
            indexName = indexName ?? IndexName;
            return GetSearchIndexClientForKey(indexName, QueryApiKey);
        }

        private SearchIndexClient GetSearchIndexClientForKey(
            string indexName,
            string apiKey,
            params DelegatingHandler[] handlers)
        {
            TestEnvironment currentEnvironment = TestEnvironmentFactory.GetTestEnvironment();

            SearchIndexClient client =
                MockContext.GetServiceClientWithCredentials<SearchIndexClient>(
                    currentEnvironment,
                    new SearchCredentials(apiKey),
                    internalBaseUri: true,
                    handlers: handlers);

            client.SearchServiceName = SearchServiceName;
            client.SearchDnsSuffix = currentEnvironment.GetSearchDnsSuffix(SearchServiceName);
            client.IndexName = indexName;
            return client;
        }
    }
}
