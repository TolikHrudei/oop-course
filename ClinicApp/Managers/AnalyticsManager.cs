using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.Managers
{
    public class AnalyticsManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;
        public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
        }
        public IEnumerable<DoctorStats> ComputeDoctorStats()
        {
            Appointment[] appointments = _appointments.GetAll();
            Doctor[] doctors = _doctors.GetAll();

            return doctors.Select(d =>
            {
                var docApps = appointments.Where(a => a.DoctorId == d.Id);
                int count = docApps.Count();
                decimal revenue = docApps.Sum(a => a.GetCost());
                DateTime lastAppointment = docApps.Any() ? docApps.Max(a => a.ScheduledAt) : DateTime.MinValue;
                return new DoctorStats(d.Id, d.FullName, count, revenue, lastAppointment);
            });
        }
        public IEnumerable<PatientStats> ComputePatientStats()
        {
            Appointment[] appointments = _appointments.GetAll();
            Patient[] patients = _patients.GetAll();

            return patients.Select(p =>
            {
                var patApps = appointments.Where(a => a.PatientId == p.Id);
                int visits = patApps.Count();
                decimal totalSpent = patApps.Sum(a => a.GetCost());
                DateTime lastVisit = patApps.Any() ? patApps.Max(a => a.ScheduledAt) : DateTime.MinValue;
                return new PatientStats(p.Id, p.FullName, visits, totalSpent, lastVisit);
            });
        }
    }
}