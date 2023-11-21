using EmsApi.Dtos;
using EmsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly EmsContext _context;
        private readonly IConfiguration config;

        public AuthController(EmsContext context,
            IConfiguration config)
        {
            _context = context;
            this.config = config;
        }

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'EmsContext.Users'  is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created("auth", user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(UserDto user)
        {
            //check if valid user
            var existingUser = _context.Users.FirstOrDefault(u =>
                u.Email == user.Email && u.Password == user.Password);
            if (existingUser == null)
            {
                return BadRequest(new
                {
                    Message = "Login failed"
                });
            }

            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            var subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            });

            var expires = DateTime.UtcNow.AddMinutes(10);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponse = new LoginResponseDto
            {
                Email = user.Email,
                Name = existingUser.UserName,
                Role = existingUser.Role,
                Token = tokenHandler.WriteToken(token)
            };

            return Ok(loginResponse);
        }
    }
}
