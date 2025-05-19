namespace AdhiveshanGrading.Entities;

[BsonIgnoreExtraElements(Inherited = true)]
public class MongoDBEntity
{
    [BsonId(Order = 0)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
