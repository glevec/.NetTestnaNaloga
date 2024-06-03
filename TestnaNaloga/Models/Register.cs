using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.Models
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
    }
}
