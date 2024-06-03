using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public int? PatientId { get; set; }

        public Patient Patient { get; set; }

        [Required]
        public bool IsReserved { get; set; }
    }
}
