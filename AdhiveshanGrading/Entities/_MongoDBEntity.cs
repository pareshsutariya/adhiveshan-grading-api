using System;
using MongoDB.Bson;
using MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace AdhiveshanGrading.Entities;

[BsonIgnoreExtraElements(Inherited = true)]
public class MongoDBEntity
{
    [BsonId(Order = 0)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
