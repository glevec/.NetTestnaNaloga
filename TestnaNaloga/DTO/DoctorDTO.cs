namespace TestnaNaloga.DTO
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public List<WorkingHoursDTO> WorkingHours { get; set; }
    }
}
