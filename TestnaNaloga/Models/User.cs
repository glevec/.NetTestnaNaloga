using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TestnaNaloga.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
