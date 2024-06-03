using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestnaNaloga.Models;

namespace TestnaNaloga.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<WorkingHours> WorkingHours { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Defined relationships
            builder.Entity<Doctor>()
                .HasMany(d => d.WorkingHours)
                .WithOne(wh => wh.Doctor)
                .HasForeignKey(wh => wh.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, Name = "Tim Rus", Specialty = "Cardiology" },
                new Doctor { Id = 2, Name = "Sandi Pangerc", Specialty = "Dermatology" },
                new Doctor { Id = 3, Name = "Jernej Leskovsek", Specialty = "General" }
            );

            builder.Entity<WorkingHours>().HasData(
                new WorkingHours { Id = 1, DoctorId = 1, Date = new DateTime(2024, 6, 1), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
                new WorkingHours { Id = 2, DoctorId = 1, Date = new DateTime(2024, 6, 3), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
                new WorkingHours { Id = 3, DoctorId = 2, Date = new DateTime(2024, 6, 2), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
                new WorkingHours { Id = 4, DoctorId = 3, Date = new DateTime(2024, 6, 5), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) }
            );

            builder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "Gasper Levec " },
                new Patient { Id = 2, Name = "Jan Capuder" },
                new Patient { Id = 3, Name = "Mitja Siska" }
            );

            builder.Entity<Appointment>().HasData(
                new Appointment { Id = 1, Date = new DateTime(2024, 6, 1), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), DoctorId = 1, PatientId = 1, IsReserved = true },
                new Appointment { Id = 2, Date = new DateTime(2024, 6, 2), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0), DoctorId = 2, IsReserved = false },
                new Appointment { Id = 3, Date = new DateTime(2024, 6, 4), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), DoctorId = 1, PatientId = 1, IsReserved = true }
            );
        }
    }
}
