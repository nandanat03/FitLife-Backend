namespace FitnessTracker.DTOs
{
    public record LoginResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public double Weight { get; set; }
    }
}
