using ClinicApp.Models;
using System;
using System.Collections.Generic;

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
            var doctors = _doctors.GetAll();
            var appointments = _appointments.GetAll();

            foreach (var doctor in doctors)
            {
                int count = 0;
                decimal revenue = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (var app in appointments)
                {
                    if (app.DoctorId == doctor.Id)
                    {
                        count++;
                        revenue += app.GetCost();
                        if (app.ScheduledAt > lastDate)
                        {
                            lastDate = app.ScheduledAt;
                        }
                    }
                }
                yield return new DoctorStats(doctor.Id, doctor.FullName, count, revenue, lastDate);
            }
        }

        public IEnumerable<PatientStats> ComputePatientStats()
        {
            var patients = _patients.GetAll();
            var appointments = _appointments.GetAll();

            foreach (var patient in patients)
            {
                int count = 0;
                decimal spent = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (var app in appointments)
                {
                    if (app.PatientId == patient.Id)
                    {
                        count++;
                        spent += app.GetCost();
                        if (app.ScheduledAt > lastDate)
                        {
                            lastDate = app.ScheduledAt;
                        }
                    }
                }
                yield return new PatientStats(patient.Id, patient.FullName, count, spent, lastDate);
            }
        }
    }
}