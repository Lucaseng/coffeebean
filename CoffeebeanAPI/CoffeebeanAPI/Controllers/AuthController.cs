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
using System.Security.Claims;




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
        public async Task<IActionResult> Login([FromBody] LoginInput login)
        {
            User myUser = await _mongoUserService.GetByEmail(login.email);
            if (myUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(login.password, myUser.Password))
                {

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Email, login.email),
                        new Claim(ClaimTypes.Role, myUser.UserRole.ToString()) // Adding role claim
                    };

                    var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      claims,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    return Ok(token);
                }

            }

            return Unauthorized(new { fail = "Email or Password was incorrect." });

        }
    }
}
