using ClinicApp.Enums;
using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.Managers
{
    public class ReportManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;
        private readonly AnalyticsManager _analytics;

        public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
            _analytics = new AnalyticsManager(appointments, doctors, patients);
        }

        public IEnumerable<SpecialityReport> GetSpecialityStats()
        {
            Appointment[] appointments = _appointments.GetAll();
            Doctor[] doctors = _doctors.GetAll();

            return doctors
                .GroupBy(d => d.Specialty)
                .Select(g =>
                {
                    int[] doctorIds = g.Select(d => d.Id).ToArray();
                    var groupApps = appointments.Where(a => doctorIds.Contains(a.DoctorId));
                    return new SpecialityReport(
                        g.Key,
                        g.Count(),
                        groupApps.Count(),
                        groupApps.Sum(a => a.GetCost())
                    );
                })
                .OrderByDescending(r => r.TotalRevenue);
        }

        public string? FindBusiestDoctorName()
        {
            return _analytics.ComputeDoctorStats()
                .OrderByDescending(s => s.AppointmentCount)
                .Select(s => (string?)s.FullName)
                .FirstOrDefault();
        }

        public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
        {
            Appointment[] appointments = _appointments.GetAll();
            Patient[] patients = _patients.GetAll();

            return appointments
                .GroupBy(a => a.PatientId)
                .Where(g => g.Count() >= minVisits)
                .Join(
                    patients,
                    g => g.Key,
                    p => p.Id,
                    (g, p) => p.FullName
                );
        }

        public IEnumerable<DoctorStats> GetTopEarners(int n)
        {
            return _analytics.ComputeDoctorStats()
                .OrderByDescending(s => s.TotalRevenue)
                .Take(n);
        }

        public bool HasAnyUrgentAppointments()
        {
            return _appointments.GetAll().Any(a => a is UrgentAppointment);
        }

        public IEnumerable<Speciality> GetActiveSpecialities()
        {
            return _doctors.GetAll()
                .Select(d => d.Specialty)
                .Distinct()
                .OrderBy(s => s);
        }

        public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
        {
            return _appointments.GetAll()
                .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => (g.Key.Year, g.Key.Month, g.Sum(a => a.GetCost())));
        }
    }
}