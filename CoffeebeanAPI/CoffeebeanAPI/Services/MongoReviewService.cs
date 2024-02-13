using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CoffeebeanAPI.Services
{
    public class MongoReviewService
    {
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly MongoShopService _coffeeShopService;
        private readonly MongoUserService _userService;
        private readonly MongoDBService _coffeeService;
        public MongoReviewService(IOptions<MongoDBSettings> mongoDBSettings) {

            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _reviewCollection = database.GetCollection<Review>(mongoDBSettings.Value.ReviewCollectionName);
            _coffeeShopService = new MongoShopService(mongoDBSettings);
            _userService = new MongoUserService(mongoDBSettings);
            _coffeeService = new MongoDBService(mongoDBSettings);
        }


        public async Task<List<Review>> GetReviewsAsync()
        {
            return await _reviewCollection.Find(new BsonDocument()).ToListAsync();
        }

       public async Task<Review> PostReviewAsync(ReviewInput myReview)
        {
            CoffeeShop myShop = await _coffeeShopService.GetShopById(myReview.CoffeeShopId);
            User myUser = await _userService.GetByIdAsync(myReview.UserId);
            Coffee myCoffee = await _coffeeService.GetCoffeeById(myReview.CoffeeId);

            if (myCoffee == null || myShop == null || myUser == null) {
                return null;
            }

            Review newReview = new Review { User = ObjectId.Parse(myUser.Id), Shop = ObjectId.Parse(myShop.Id), Coffee = ObjectId.Parse(myCoffee.Id), Rating = myReview.Rating, Description = myReview.Description};
            
            //POST THE REVIEW
            await _reviewCollection.InsertOneAsync(newReview);
            return newReview;

        }


    }
}
