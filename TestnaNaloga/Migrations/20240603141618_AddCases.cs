using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestnaNaloga.Migrations
{
    /// <inheritdoc />
    public partial class AddCases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "EndTime", "IsReserved", "PatientId", "StartTime" },
                values: new object[] { new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), false, null, new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "DoctorId", "EndTime", "IsReserved", "PatientId", "StartTime" },
                values: new object[] { new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new TimeSpan(0, 10, 0, 0, 0), true, 1, new TimeSpan(0, 9, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "EndTime", "IsReserved", "PatientId", "StartTime" },
                values: new object[] { new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), true, 3, new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "DoctorId", "EndTime", "IsReserved", "PatientId", "StartTime" },
                values: new object[] { new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new TimeSpan(0, 11, 0, 0, 0), false, null, new TimeSpan(0, 10, 0, 0, 0) });
        }
    }
}
