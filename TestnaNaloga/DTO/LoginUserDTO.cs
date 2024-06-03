using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.DTO
{
    public class LoginUserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
