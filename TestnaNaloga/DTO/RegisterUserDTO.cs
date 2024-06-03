using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Role {  get; set; } 
    }
}
