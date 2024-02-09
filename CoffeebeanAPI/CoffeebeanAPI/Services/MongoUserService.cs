using CoffeebeanAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using CoffeebeanAPI.Dtos;


namespace CoffeebeanAPI.Services
{
    public class MongoUserService
    {
        private readonly IMongoCollection<User> _userCollection;


        public MongoUserService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollectionName);

        }


        public async Task<List<User>> GetAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            //return await _userCollection.Find(new BsonDocument()).ToListAsync();

            return await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task PostAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            return;

        }


        public async Task DeleteAsync(string id)
        {
            await _userCollection.FindOneAndDeleteAsync(x => x.Id == id);
            return;
        }


    }
}