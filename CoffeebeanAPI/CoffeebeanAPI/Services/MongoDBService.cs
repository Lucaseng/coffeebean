using CoffeebeanAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;


namespace CoffeebeanAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Coffee> _coffeeCollection;


        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _coffeeCollection = database.GetCollection<Coffee>(mongoDBSettings.Value.CollectionName);

        }


        public async Task CreateAsync(Coffee coffee)
        {
            await _coffeeCollection.InsertOneAsync(coffee);
            return;
        }


        public async Task<List<Coffee>> GetAsync()
        {
            return await _coffeeCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Coffee> GetOneAsync(string name)
        {
            return await _coffeeCollection.Find(x => x.Name == name).FirstOrDefaultAsync();
        }


        public async Task DeleteAsync(string id)
        {
            FilterDefinition<Coffee> coffee = Builders<Coffee>.Filter.Eq("Id", id);
            await _coffeeCollection.DeleteOneAsync(coffee);
        }

    }
}