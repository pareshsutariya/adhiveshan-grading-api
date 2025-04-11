using System;
using MongoDB.Bson;
using MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace AdhiveshanGrading.Entities;

public class Configuration : MongoDBEntity
{
    public int ConfigurationId { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    [BsonIgnoreIfNull]
    public string ValidValues { get; set; }

    public string Status { get; set; }
}