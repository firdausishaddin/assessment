using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using assessment.Data;
using assessment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace assessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly TokenStoreService _tokenStore;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context,
            ILogger<AuthController> logger,
            TokenStoreService tokenStore,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _tokenStore = tokenStore;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpPost("login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == loginDto.Username || u.Username == loginDto.Username);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var hasher = new PasswordHasher<UserDto>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials");

            // Generate JWT
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username ?? user.Email)
                },
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                expiresIn = jwtSettings["ExpiresInMinutes"]
            });
        }
    }
}
