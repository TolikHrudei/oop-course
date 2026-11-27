using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class SpecialityReport
    {
        public Speciality Speciality { get; }
        public int DoctorCount { get; }
        public int AppointmentCount { get; }
        public decimal TotalRevenue { get; }
        public SpecialityReport(Speciality speciality, int doctorCount, int appointmentCount, decimal totalRevenue)
        {
            Speciality = speciality;
            DoctorCount = doctorCount;
            AppointmentCount = appointmentCount;
            TotalRevenue = totalRevenue;
        }
        public override string ToString()
        {
            string specialityName = ClinicFormatter.FormatSpeciality(Speciality);
            return $"{specialityName} | Лікарів: {DoctorCount} | Прийомів: {AppointmentCount} | Виручка: {TotalRevenue:F2} грн";
        }
    }
}
