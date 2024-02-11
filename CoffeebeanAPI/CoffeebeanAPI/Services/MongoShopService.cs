using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CoffeebeanAPI.Services
{
    public class MongoShopService
    {
        private readonly IMongoCollection<CoffeeShop> _shopCollection;

        public MongoShopService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _shopCollection = database.GetCollection<CoffeeShop>(mongoDBSettings.Value.ShopCollectionName);

        }


        public async Task<List<CoffeeShopInput>> GetShopsAsync()
        {
            List<CoffeeShop> myShops = await _shopCollection.Find(new BsonDocument()).ToListAsync();

            List<CoffeeShopInput> outputShops = new List<CoffeeShopInput> { };
            for (int i = 0; i < myShops.Count; i++)
            {
                CoffeeShopInput convertedShop = new CoffeeShopInput { Name = myShops[i].Name, CoffeeTypes = ConvertObjectIdsToString(myShops[i].CoffeeTypes), Coords = myShops[i].Coords };
                outputShops.Add(convertedShop);
            }
            return outputShops;

        }

        public List<string> ConvertObjectIdsToString(List<ObjectId> ids)
        {
            List<string> outputIds = new List<string> { };
            for (int i = 0; i < ids.Count; i++)
            {
                outputIds.Add(ids[i].ToString());
            }
            return outputIds;
        }

        public async Task PostShop(CoffeeShop shop)
        {
            await _shopCollection.InsertOneAsync(shop);
            return;
        }


    }
}
