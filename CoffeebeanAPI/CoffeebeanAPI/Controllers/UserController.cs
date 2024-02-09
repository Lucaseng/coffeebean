using CoffeebeanAPI.Models;
using CoffeebeanAPI.Services;
using CoffeebeanAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;


namespace CoffeebeanAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly MongoUserService _mongoUserService;

        public UserController(MongoUserService mongoUserService)
        {
            _mongoUserService = mongoUserService;
        }



        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _mongoUserService.GetAsync();

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            User myUser = await _mongoUserService.GetByIdAsync(id);
            return Ok(myUser);

        }


        [HttpPost]
        public async Task<IActionResult> Post(UserInput user)
        {
            User myUser = new User { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Password = BCrypt.Net.BCrypt.HashPassword(user.Password)};
            await _mongoUserService.PostAsync(myUser);
            return CreatedAtAction(nameof(Get), new { myUser.Id }, myUser);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //Check if user exists
            User myUser = await _mongoUserService.GetByIdAsync(id);
            if (myUser == null) {
                return BadRequest(new {fail = "User does not exist!" });
            }
            await _mongoUserService.DeleteAsync(id);
            return Ok(new { success = string.Format("User with id {0} was removed!", id) });
        }

    }
}
