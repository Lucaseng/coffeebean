using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;

namespace CoffeebeanAPI.Models
{
    public class CoffeeShop
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public int[] Coords { get; set; }
        public List<ObjectId>? CoffeeTypes { get; set; } = new List<ObjectId>();
    }
}
