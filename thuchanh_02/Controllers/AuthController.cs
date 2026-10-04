using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtSupabaseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using thuchanh_02.Models;

namespace thuchanh_02.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(PostgresContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1. API Đăng nhập & Sinh JWT (POST / hoặc POST /auth/login)
        [HttpPost("/")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.UserName) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập userName và password!" });
            }

            // Truy vấn kiểm tra thông tin tài khoản từ Supabase Postgres
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác!" });
            }

            // Sinh mã JWT
            var token = GenerateJwtToken(user);

            // Cập nhật mã Token vừa tạo vào cơ sở dữ liệu
            user.Token = token;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token = token
            });
        }

        // 2. API Xác thực Token (GET /auth)
        [HttpGet("/auth")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                message = "Token hợp lệ!",
                userData = new { userId, userName }
            });
        }

        // 3. API in dòng "Hello World" ở bài 1 (Bảo vệ bởi Middleware Authorize)
        [HttpGet("/hello")]
        [Authorize]
        public IActionResult HelloWorld()
        {
            return Ok("Hello World");
        }

        // Hàm riêng hỗ trợ sinh mã JWT
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}