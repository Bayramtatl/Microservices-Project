using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService) => _tokenService = tokenService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Şimdilik test amaçlı sabit kullanıcı
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _tokenService.CreateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Kullanıcı adı veya şifre hatalı!");
        }
    }
}
