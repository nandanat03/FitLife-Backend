using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetToday()
        {
            var result = await _progressService.GetDailySummaryAsync();
            return Ok(result);
        }

        [HttpGet("monthly/{year}")]
        public async Task<IActionResult> GetMonthly(int year)
        {
            var result = await _progressService.GetMonthlySummaryAsync(year);
            return Ok(result);
        }

        [HttpGet("custom")]
        public async Task<IActionResult> GetCustom([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _progressService.GetCustomSummaryAsync(start, end);
            return Ok(result);
        }
    }
}
