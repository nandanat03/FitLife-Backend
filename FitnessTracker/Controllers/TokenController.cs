using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshRequestDto refreshRequest)
        {
            var result = _tokenService.RefreshAccessToken(refreshRequest.RefreshToken);
            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token." });

            return Ok(result);
        }
    }
}