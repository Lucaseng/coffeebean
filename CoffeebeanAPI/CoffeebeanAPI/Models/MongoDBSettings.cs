namespace CoffeebeanAPI.Models
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string ShopCollectionName { get; set; } = null!;

        public string ReviewCollectionName { get; set; } = null!;

    }

}
