using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.Models
{
    public class WorkingHours
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public Doctor Doctor { get; set; }
    }
}
