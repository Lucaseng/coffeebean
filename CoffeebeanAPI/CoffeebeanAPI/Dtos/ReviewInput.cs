using MongoDB.Bson;

namespace CoffeebeanAPI.Dtos
{
    public class ReviewInput
    {
        public string UserId { get; set; }

        public string CoffeeId { get; set; }

        public string CoffeeShopId { get; set; }

        public float Rating { get; set; }

        public string Description { get; set; }
    }
}
