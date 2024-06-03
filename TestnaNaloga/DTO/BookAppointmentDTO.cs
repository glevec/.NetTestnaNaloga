using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.DTO
{
    public class BookAppointmentDTO
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}
