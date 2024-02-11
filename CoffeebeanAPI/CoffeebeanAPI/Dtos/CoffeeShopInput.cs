using MongoDB.Bson.Serialization.Attributes;

namespace CoffeebeanAPI.Dtos
{
    public class CoffeeShopInput
    {
        public string Name { get; set; }
        public int[]? Coords { get; set; } = new int[2];
        public List<string>? CoffeeTypes { get; set; } = new List<string>();
    }
}
