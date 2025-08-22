using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using WalletApp.CurrencyApi.Models;
using WalletApp.Domain.Entities;
using WalletApp.Infrastructure.Data;


namespace CleanProductApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NovibetDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        
        public AuthController(IConfiguration configuration, NovibetDbContext context)
        {
            _configuration = configuration;  //using configuration for jwt token 
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public class LoginRequest
        {
        public string? Username { get; set; }
        public string? Password { get; set; }


        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {   //checking if user already exists
                if (_context.Users.Any(u => u.Username == request.Username))
                {
                    return BadRequest("Username already exists");
                }

                var user = new User
                {
                    Username = request.Username
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password); //password hash

                _context.Users.Add(user); 
                _context.SaveChanges(); //add user info (password) in db

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                // status code for debugging
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {    
                //check if username or pass is null
                if (string.IsNullOrWhiteSpace(login?.Username) || string.IsNullOrWhiteSpace(login?.Password))
                {
                    return BadRequest("Username and password cannot be empty.");
                }
                //check if credentials is valid
                if (IsValidUserCredentials(login.Username, login.Password))
                {
                    var tokenString = GenerateJwtToken(login.Username);

                    return Ok(new { Token = tokenString });
                }
                //if credentials are wrong return unauthorized message
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }
            catch (Exception ex)
            {   //catch the error and print ex.message for debugging
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        private bool IsValidUserCredentials(string username, string password)
        {
            //take the user's info 
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                return false;
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
             
        }
        private string GenerateJwtToken(string username)
        {
            //create a symmetric security token from the JWT secret key defined in appsettings.json
            var secretKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");
            var issuertext = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
            var audiencetext = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWt:audience is missing");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            //Create signing credentials using the key and HMAC-SHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            // Create the JWT with issuer, audience, claims, expiration time, and signing credentials
            var token = new JwtSecurityToken(
                issuer: issuertext,
                audience: audiencetext,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}