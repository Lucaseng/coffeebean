using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoffeebeanAPI.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public float Rating { get; set; }

        public ObjectId User { get; set; }

        public ObjectId Coffee {  get; set; }

        public ObjectId Shop { get; set; }

        public string Description { get; set; }
    }
}
