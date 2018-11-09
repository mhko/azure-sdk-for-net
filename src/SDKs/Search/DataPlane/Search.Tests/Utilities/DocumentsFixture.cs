﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Tests.Utilities
{
    using System;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using Microsoft.Spatial;

    public class DocumentsFixture : IndexFixture
    {
        public readonly Hotel[] TestDocuments =
            new[]
            {
                new Hotel()
                {
                    HotelId = "1",
                    BaseRate = 199.0,
                    Description =
                        "Best hotel in town if you like luxury hotels. They have an amazing infinity pool, a spa, " +
                        "and a really helpful concierge. The location is perfect -- right downtown, close to all " +
                        "the tourist attractions. We highly recommend this hotel.",
                    DescriptionFr = 
                        "Meilleur hôtel en ville si vous aimez les hôtels de luxe. Ils ont une magnifique piscine " +
                        "à débordement, un spa et un concierge très utile. L'emplacement est parfait – en plein " +
                        "centre, à proximité de toutes les attractions touristiques. Nous recommandons fortement " +
                        "cet hôtel.",
                    HotelName = "Fancy Stay",
                    Category = "Luxury",
                    Tags = new[] { "pool", "view", "wifi", "concierge" },
                    ParkingIncluded = false,
                    SmokingAllowed = false,
                    LastRenovationDate = new DateTimeOffset(2010, 6, 27, 0, 0, 0, TimeSpan.Zero),
                    Rating = 5,
                    Location = GeographyPoint.Create(47.678581, -122.131577),
                    Address = new Address()
                    {
                        City = "Bellevue",
                        Street = "11025 NE 8th St"
                    },
                    PastAwards = new[] { 1, 2, 3 },
                    PastRatings = new[] { 1.1, 2.2, 3.3 },
                    Rooms = new []
                    {
                        new Room()
                        {
                            RoomId = "101",
                            SleepCount = 4,
                            BaseRate = 99.00,
                            Type = "Two Double Beds"
                        },
                        new Room()
                        {
                            RoomId = "102",
                            SleepCount = 2,
                            BaseRate = 49.00,
                            Type = "One Queen Bed"
                        }
                    }
                },
                new Hotel()
                {
                    HotelId = "2",
                    BaseRate = 79.99,
                    Description = "Cheapest hotel in town. Infact, a motel.",
                    DescriptionFr = "Hôtel le moins cher en ville. Infact, un motel.",
                    HotelName = "Roach Motel",
                    Category = "Budget",
                    Tags = new[] { "motel", "budget" },
                    ParkingIncluded = true,
                    SmokingAllowed = true,
                    LastRenovationDate = new DateTimeOffset(1982, 4, 28, 0, 0, 0, TimeSpan.Zero),   //aka.ms/sre-codescan/disable
                    Rating = 1,
                    Location = GeographyPoint.Create(49.678581, -122.131577),
                    Address = new Address()
                    {
                        City = "Redmond",
                        Street = "One Microsoft Way"
                    },
                    PastAwards = new[] { 1, 2, 3 },
                    PastRatings = new[] { 1.1, 2.2, 3.3 },
                    Rooms = new []
                    {
                        new Room()
                        {
                            RoomId = "A",
                            SleepCount = 4,
                            BaseRate = 2.00,
                            Type = "Two Double Beds"
                        },
                        new Room()
                        {
                            RoomId = "B",
                            SleepCount = 2,
                            BaseRate = 3.50,
                            Type = "One Queen Bed"
                        }
                    }
                },
                new Hotel()
                {
                    HotelId = "3",
                    BaseRate = 129.99,
                    Description = "Very popular hotel in town",
                    DescriptionFr = "Hôtel le plus populaire en ville",
                    HotelName = "EconoStay",
                    Category = "Budget",
                    Tags = new[] { "wifi", "budget" },
                    ParkingIncluded = true,
                    SmokingAllowed = false,
                    LastRenovationDate = new DateTimeOffset(1995, 7, 1, 0, 0, 0, TimeSpan.Zero),
                    Rating = 4,
                    Location = GeographyPoint.Create(46.678581, -122.131577)
                },
                new Hotel()
                {
                    HotelId = "4",
                    BaseRate = 129.99,
                    Description = "Pretty good hotel",
                    DescriptionFr = "Assez bon hôtel",
                    HotelName = "Express Rooms",
                    Category = "Budget",
                    Tags = new[] { "wifi", "budget" },
                    ParkingIncluded = true,
                    SmokingAllowed = false,
                    LastRenovationDate = new DateTimeOffset(1995, 7, 1, 0, 0, 0, TimeSpan.Zero),
                    Rating = 4,
                    Location = GeographyPoint.Create(48.678581, -122.131577)
                },
                new Hotel()
                {
                    HotelId = "5",
                    BaseRate = 129.99,
                    Description = "Another good hotel",
                    DescriptionFr = "Un autre bon hôtel",
                    HotelName = "Comfy Place",
                    Category = "Budget",
                    Tags = new[] { "wifi", "budget" },
                    ParkingIncluded = true,
                    SmokingAllowed = false,
                    LastRenovationDate = new DateTimeOffset(2012, 8, 12, 0, 0, 0, TimeSpan.Zero),
                    Rating = 4,
                    Location = GeographyPoint.Create(48.678581, -122.131577)
                },
                new Hotel()
                {
                    HotelId = "6",
                    BaseRate = 279.99,
                    Description = "Surprisingly expensive. Model suites have an ocean-view.",
                    LastRenovationDate = null
                },
                new Hotel()
                {
                    HotelId = "7",
                    BaseRate = 279.99,
                    Description = "Modern architecture, very polite staff and very clean. Also very affordable.",
                    DescriptionFr = "Architecture moderne, personnel poli et très propre. Aussi très abordable.",
                    HotelName = "Modern Stay"
                },
                new Hotel()
                {
                    HotelId = "8",
                    BaseRate = 79.99,
                    Description = "Has some road noise and is next to the very police station. Bathrooms had morel coverings.",
                    DescriptionFr = "Il y a du bruit de la route et se trouve à côté de la station de police. Les salles de bain avaient des revêtements de morilles."
                }
            };

        public override void Initialize(MockContext context)
        {
            base.Initialize(context);

            SearchIndexClient indexClient = this.GetSearchIndexClient();

            var batch = IndexBatch.Upload(TestDocuments);
            indexClient.Documents.Index(batch);

            SearchTestUtilities.WaitForIndexing();
        }
    }
}
