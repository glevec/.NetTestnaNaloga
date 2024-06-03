using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestnaNaloga.Data;
using TestnaNaloga.DTO;
using TestnaNaloga.Models;

namespace TestnaNaloga.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patient/getDoctorWorkingHours
        [Authorize(Roles = "Patient")]
        [HttpGet("getDoctorWorkingHours")]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.WorkingHours)
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours.Select(wh => new WorkingHoursDTO
                    {
                        Date = wh.Date,
                        StartTime = wh.StartTime,
                        EndTime = wh.EndTime,
                    }).ToList(),
                }).ToListAsync();

            return Ok(doctors);
        }

        // GET: api/patient/{doctorId}/getDoctorBookedAppointments
        [Authorize(Roles = "Patient")]
        [HttpGet("{doctorId}/getDoctorBookedAppointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetBookedAppointments(int doctorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date");
            }

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date >= startDate && a.Date <= endDate && a.IsReserved == true)
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsReserved = a.IsReserved

                }).ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound("The selected doctor does not have any booked appointments in the selected time period");
            }

            return Ok(appointments);
        }

        // POST: api/patient/bookAppointment
        [Authorize(Roles = "Patient")]
        [HttpPost("bookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDTO request)
        {
            // Check doctor's working hours
            var workingHour = await _context.WorkingHours
            .FirstOrDefaultAsync(wh => wh.DoctorId == request.DoctorId &&
                                       wh.Date.Date == request.Date.Date &&
                                       wh.StartTime <= request.StartTime &&
                                       wh.EndTime >= request.EndTime);

            if (workingHour == null)
            {
                return BadRequest("Doctor does not work during the requested time");
            }

            // Check if the doctor has any bookings during that time
            var isBooked = await _context.Appointments
            .AnyAsync(a => a.DoctorId == request.DoctorId &&
                           a.Date.Date == request.Date.Date &&
                           a.StartTime < request.EndTime &&
                           a.EndTime > request.StartTime);


            if (isBooked)
            {
                return BadRequest("Doctor already has an appointment during the requested time");
            }

            var appointment = new Appointment
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                Date = request.Date.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsReserved = true
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok("Appointment booked");
        }

        // DELETE: api/patient/removeAppointment/{id}
        [Authorize(Roles = "Patient")]
        [HttpDelete("removeAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Appointment successfully deleted.", AppointmentId = id });
        }
    }
}
