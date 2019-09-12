using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Health.Checks.In.ASP.NET.Core.Model
{
    public class Vehicle
    {
        [BsonId]
        public ObjectId ID { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public long ModelYear { get; set; }

        public long YearOfManufacture { get; set; }

        [BsonDateTimeOptions]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
