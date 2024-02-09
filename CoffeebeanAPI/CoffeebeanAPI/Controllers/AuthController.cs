using Microsoft.AspNetCore.Mvc;
using System;
using CoffeebeanAPI.Models;
using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;




namespace CoffeebeanAPI.Controllers
{

    [Controller]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly MongoUserService _mongoUserService;

        private IConfiguration _config;
        public AuthController(IConfiguration config, MongoUserService mongoUserService)
        {
            _config = config;
            _mongoUserService = mongoUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]string email, string password)
        {
            User myUser = await _mongoUserService.GetByEmail(email, password);
            if (BCrypt.Net.BCrypt.Verify(password, myUser.Password))
            {
                return Ok(myUser);
            }
            else
            {
                return Unauthorized();
            }
        }

        /*[HttpPost]
        public IActionResult Post(string email, string password);
        {
            //LoginRequest loginRequest
            //your logic for login process
            //If login usrename and password are correct then proceed to generate token
            User myUser = await _mongoUserService.Login(email, password);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }*/
    }
}
