using Microsoft.AspNetCore.Mvc;
using System;
using CoffeebeanAPI.Models;
using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Services;
using MongoDB.Driver;

namespace CoffeebeanAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class CoffeeController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public CoffeeController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Coffee>> Get()
        {
            return await _mongoDBService.GetAsync();

        }


        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            Coffee myCoffee = await _mongoDBService.GetOneAsync(name);
            if (myCoffee == null)
            {
                return Ok();

            }
            return Ok(myCoffee); 


        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CoffeeInput coffee)
        {
            //Check if Coffee already exists
            Coffee myCoffee = await _mongoDBService.GetOneAsync(coffee.Name);
            if (myCoffee != null)
            {
                return BadRequest(new { fail = string.Format("{0} already exists in the database!", coffee.Name) });

            }
            myCoffee = new Coffee { Name = coffee.Name };
            await _mongoDBService.CreateAsync(myCoffee);
            return CreatedAtAction(nameof(Get), new {myCoffee.Id}, myCoffee);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoDBService.DeleteAsync(id);
            return NoContent();

        }



    }
}
