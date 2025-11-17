using AuthLibrary.Contexts;
using AuthLibrary.Models;
using AuthLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseLibrary _context;
        private readonly string _issuer = "CinemaAPI";          // издатель
        private readonly string _audience = "CinemaClient";  // потребители
        private readonly string _secretKey = "12345678123456781234567812345678";


        public AuthController(DatabaseLibrary context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest request)
        {
            if (await _context.CinemaUsers.AnyAsync(u => u.Login == request.Login))
                return BadRequest("Пользователь с таким логином уже существует");

            var passwordHash = HashPassword(request.Password);

            var user = new CinemaUser
            {
                Login = request.Login,
                PasswordHash = passwordHash,
                RoleId = 3
            };


            _context.CinemaUsers.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>>Login(LoginRequest request)
        {
            var user = await _context.CinemaUsers
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == request.Login);

            if (user == null || user.PasswordHash != HashPassword(request.Password))
                return Unauthorized("Неверный логин или пароль");

            var token = GenereteJwtToken(user);

        }

        private string GenereteJwtToken(CinemaUser user)
        {
            var claims = new Claim[]
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Login),
                new (ClaimTypes.Role, user.Role.RoleName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // GET api/<AuthCinemaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthCinemaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
