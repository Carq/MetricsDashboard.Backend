﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MetricsDashboard.Core.Entities
{
    public abstract class MetricBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("metricName")]
        public string MetricName { get; set; }

        [BsonElement("metricKind")]
        public string MetricKind { get; set; }

        [BsonElement("addedOn")]
        public DateTimeOffset AddedOn { get; set; }

        public abstract object MetricValue { get; }
    }
}