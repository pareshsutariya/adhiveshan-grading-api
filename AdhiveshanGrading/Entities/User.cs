using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdhiveshanGrading.Entities;

public class User : MongoDBEntity
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }

    [BsonIgnoreIfNull]
    public List<string> Permissions { get; set; }
}