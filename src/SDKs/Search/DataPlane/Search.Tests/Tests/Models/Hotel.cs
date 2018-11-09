// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Tests
{
    using System;
    using System.Linq;
    using Common;
    using Models;
    using Spatial;
    
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }

        public Document AsDocument() =>
            new Document()
            {
                ["street"] = Street,
                ["city"] = City
            };

        public override bool Equals(object obj)
        {
            Address other = obj as Address;

            if (other == null)
            {
                return false;
            }

            return
                Street == other.Street && City == other.City;
        }

        public override int GetHashCode() => 0;

        public override string ToString() => $"Street: {Street}; City: {City};";
    }

    [SerializePropertyNamesAsCamelCase]
    public class Room
    {
        public string RoomId { get; set; }

        public string Type { get; set; }

        public double? BaseRate { get; set; }

        public int SleepCount { get; set; }

        public override bool Equals(object obj)
        {
            Room other = obj as Room;

            if (other == null)
            {
                return false;
            }

            return
                RoomId == other.RoomId &&
                Hotel.DoublesEqual(BaseRate, other.BaseRate) &&
                Type == other.Type &&
                SleepCount == other.SleepCount;
        }

        public override int GetHashCode() => RoomId?.GetHashCode() ?? 0;

        public Document AsDocument() =>
            new Document()
            {
                ["baseRate"] = BaseRate,
                ["type"] = Type,
                ["roomId"] = RoomId,
                ["sleepCount"] = SleepCount
            };
    }

    [SerializePropertyNamesAsCamelCase]
    public class Hotel
    {
        public string HotelId { get; set; }

        public double? BaseRate { get; set; }

        public string Description { get; set; }

        public string DescriptionFr { get; set; }

        public string HotelName { get; set; }

        public string Category { get; set; }

        public string[] Tags { get; set; }

        public bool? ParkingIncluded { get; set; }

        public bool? SmokingAllowed { get; set; }

        public DateTimeOffset? LastRenovationDate { get; set; }

        public int? Rating { get; set; }

        public double[] PastRatings { get; set; }

        public int[] PastAwards { get; set; }

        public GeographyPoint Location { get; set; }

        public Address Address { get; set; }

        public Room[] Rooms { get; set; }

        public override bool Equals(object obj)
        {
            Hotel other = obj as Hotel;

            if (other == null)
            {
                return false;
            }

            return
                HotelId == other.HotelId &&
                DoublesEqual(BaseRate, other.BaseRate) &&
                Description == other.Description &&
                DescriptionFr == other.DescriptionFr &&
                HotelName == other.HotelName &&
                Category == other.Category &&
                ((Tags == null) ? (other.Tags == null || other.Tags.Length == 0) : Tags.SequenceEqual(other.Tags ?? new string[0])) &&
                ParkingIncluded == other.ParkingIncluded &&
                SmokingAllowed == other.SmokingAllowed &&
                DateTimeOffsetsEqual(LastRenovationDate, other.LastRenovationDate) &&
                Rating == other.Rating &&
                ((PastRatings == null) ? (other.PastRatings == null || other.PastRatings.Length == 0) : PastRatings.SequenceEqual(other.PastRatings ?? new double[0])) &&
                ((PastAwards == null) ? (other.PastAwards == null || other.PastAwards.Length == 0) : PastAwards.SequenceEqual(other.PastAwards ?? new int[0])) &&
                ((Rooms == null) ? (other.Rooms == null || other.Rooms.Length == 0) : Rooms.SequenceEqual(other.Rooms ?? new Room[0])) &&
                ((Address == null) ? other.Address == null : Address.Equals(other.Address)) &&
                ((Location == null) ? other.Location == null : Location.Equals(other.Location));
        }

        public override int GetHashCode() => HotelId?.GetHashCode() ?? 0;

        public override string ToString() =>
            $"ID: {HotelId}; BaseRate: {BaseRate}; Description: {Description}; " +
            $"Description (French): {DescriptionFr}; Name: {HotelName}; Category: {Category}; " +
            $"Tags: {Tags?.ToCommaSeparatedString() ?? "null"}; Parking: {ParkingIncluded}; " +
            $"Smoking: {SmokingAllowed}; LastRenovationDate: {LastRenovationDate}; Rating: {Rating}; " +
            $"PastRatings: {PastRatings?.ToCommaSeparatedString() ?? "null"}; " +
            $"PastAwards: {PastAwards?.ToCommaSeparatedString() ?? "null"}; " +
            $"LastRenovationDate: {LastRenovationDate}; Rating: {Rating}; " +
            $"Location: [{Location?.Longitude ?? 0}, {Location?.Latitude ?? 0}]" +
            $"Address: {Address.ToString()};" +
            $"Rooms: {Rooms?.ToCommaSeparatedString() ?? "null"};";

        public Document AsDocument() =>
            new Document()
            {
                ["address"] = Address?.AsDocument(),
                ["baseRate"] = BaseRate,
                ["category"] = Category,
                ["description"] = Description,
                ["descriptionFr"] = DescriptionFr,
                ["hotelId"] = HotelId,
                ["hotelName"] = HotelName,
                ["lastRenovationDate"] = LastRenovationDate,
                ["location"] = Location,
                ["parkingIncluded"] = ParkingIncluded,
                ["pastAwards"] = PastAwards != null ? PastAwards : new int[0],
                ["pastRatings"] = PastRatings != null ? PastRatings : new double[0],
                ["rating"] = Rating.HasValue ? (long?) Rating.Value : null, // JSON.NET always deserializes to int64
                ["rooms"] = Rooms != null ? Rooms.Select(x => x.AsDocument()).ToArray() : new Document[0],
                ["smokingAllowed"] = SmokingAllowed,
                ["tags"] = Tags ?? new string[0]   // OData always gives [] instead of null for collections.
            };

        public static bool DoublesEqual(double? x, double? y)
        {
            if (x == null)
            {
                return y == null;
            }

            if (Double.IsNaN(x.Value))
            {
                return y != null && Double.IsNaN(y.Value);
            }

            return x == y;
        }

        private static bool DateTimeOffsetsEqual(DateTimeOffset? a, DateTimeOffset? b)
        {
            if (a == null)
            {
                return b == null;
            }

            if (b == null)
            {
                return false;
            }

            if (a.Value.EqualsExact(b.Value))
            {
                return true;
            }

            // Allow for some loss of precision in the tick count.
            long aTicks = a.Value.UtcTicks;
            long bTicks = b.Value.UtcTicks;

            return (aTicks / 10000) == (bTicks / 10000);
        }
    }
}
