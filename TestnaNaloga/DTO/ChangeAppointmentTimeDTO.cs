using System.ComponentModel.DataAnnotations;

namespace TestnaNaloga.DTO
{
    public class ChangeAppointmentTimeDTO
    {
        [Required]
        public DateTime NewDate { get; set; }

        [Required]
        public TimeSpan NewStartTime { get; set; }

        [Required]
        public TimeSpan NewEndTime { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
