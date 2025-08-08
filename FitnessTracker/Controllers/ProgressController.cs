using Asp.Versioning;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                return null;

            return userId;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetToday()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User ID not found");

            var result = await _progressService.GetDailySummaryAsync(userId.Value);
            return Ok(result);
        }

        [HttpGet("monthly/{year}")]
        public async Task<IActionResult> GetMonthly(int year)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User ID not found");

            var result = await _progressService.GetMonthlySummaryAsync(year, userId.Value);
            return Ok(result);
        }

        [HttpGet("custom")]
        public async Task<IActionResult> GetCustom([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User ID not found");

            var result = await _progressService.GetCustomSummaryAsync(start, end, userId.Value);
            return Ok(result);
        }
    }
}
