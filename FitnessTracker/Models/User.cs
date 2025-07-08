using System.ComponentModel.DataAnnotations;


namespace FitnessTracker.Models
{
    public class User
    {
    
        public int UserId { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }
        [Required]
        public String Role { get; set; }
        public Double Height { get; set; }
        public Double Weight { get; set; }
        public String ActivityLevel { get; set; }
        public DateTime MemberSince {  get; set; }

    
    }
}
