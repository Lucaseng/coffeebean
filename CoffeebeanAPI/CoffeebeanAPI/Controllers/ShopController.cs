using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Models;
using CoffeebeanAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CoffeebeanAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ShopController : Controller
    {
        private readonly MongoShopService _mongoDBService;

        public ShopController(MongoShopService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<CoffeeShopInput> myShops = await _mongoDBService.GetShopsAsync();
            return Ok(myShops);
        }

        [HttpPost]

        public async Task<IActionResult> AddShop(CoffeeShopInput myShop)
        {
            //CONVERT STRINGS TO OBJECT IDs
            List<ObjectId> myCoffeeShops = new List<ObjectId>();
            for (int i = 0; i < myShop.CoffeeTypes.Count; i++)
            {
                myCoffeeShops.Add(ObjectId.Parse(myShop.CoffeeTypes[i]));
            }

            //CONVERT FROM DTO TO SHOP
            CoffeeShop shop = new CoffeeShop { Name = myShop.Name, Coords = myShop.Coords, CoffeeTypes = myCoffeeShops };

            //ADD TO DATABASE
            await _mongoDBService.PostShop(shop);
            return CreatedAtAction(nameof(Get), new { shop.Id }, shop);
        }


    }
}
