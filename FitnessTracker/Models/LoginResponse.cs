namespace FitnessTracker.Models
{
    public sealed class LoginResponse
    {
        public int UserId {  get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public Double Weight { get; set; }
    }
}
