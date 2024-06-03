using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
