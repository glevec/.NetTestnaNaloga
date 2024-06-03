using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        public ICollection<WorkingHours> WorkingHours { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
