using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBDemo.ConsoleApp.Models
{
    public class User
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("created_at")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
        [BsonElement("is_active")]
        public bool IsActive { get; set; }
        [BsonElement("age")]
        public int Age { get; set; }
        [BsonElement("order")]
        public long Order { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }
}

