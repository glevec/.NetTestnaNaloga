using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestnaNaloga.Data;
using TestnaNaloga.DTO;

namespace TestnaNaloga.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/doctor/getAppointments/{doctorId}
        [Authorize(Roles = "Doctor")]
        [HttpGet("getAppointments/{doctorId}")]
        public async Task<IActionResult> GetAppointments(int doctorId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();

            return Ok(appointments);
        }


        // PUT: api/doctor/changeAppointmentDate/{appointmentId}
        [Authorize(Roles = "Doctor")]
        [HttpPut("changeAppointmentDate/{appointmentId}")]
        public async Task<IActionResult> ChangeAppointmentDate(int appointmentId, [FromBody] ChangeAppointmentTimeDTO request)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.DoctorId != request.DoctorId)
            {
                return Unauthorized(new { Message = "You can only change your appointments" });
            }

            // check if another appointment already exists during the requested time slot
            bool appointmentExists = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == appointment.DoctorId &&
                a.Id != appointmentId &&
                a.Date.Date == request.NewDate.Date &&
                ((a.StartTime < request.NewEndTime && a.EndTime > request.NewStartTime) || (request.NewStartTime < a.EndTime && request.NewEndTime > a.StartTime))
            );

            if (appointmentExists)
            {
                return BadRequest("Another appointment exists alredy in the requested time slot");
            }

            appointment.Date = request.NewDate.Date;
            appointment.StartTime = request.NewStartTime;
            appointment.EndTime = request.NewEndTime;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Appointment time changed successfully" });
        }


        // DELETE: api/doctor/removeAppointment/{appointmentId}/{doctorId}
        [Authorize(Roles = "Doctor")]
        [HttpDelete("removeAppointment/{appointmentId}/{doctorId}")]
        public async Task<IActionResult> RemoveAppointment(int appointmentId, int doctorId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.DoctorId != doctorId)
            {
                return Unauthorized(new { Message = "You can only remove your appointments" });
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Patient appointment removed", AppointmentId = appointmentId });
        }
    }
}
