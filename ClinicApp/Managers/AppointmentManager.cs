using ClinicApp.Enums;
using ClinicApp.Events;
using ClinicApp.Models;
using System;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class AppointmentManager
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();
        private readonly PatientManager _patients;
        private readonly DoctorManager _doctors;
        public event EventHandler<AppointmentEventArgs>? AppointmentBooked;
        public event EventHandler<AppointmentEventArgs>? AppointmentCancelled;
        public event EventHandler<AppointmentEventArgs>? AppointmentCompleted;
        public event EventHandler<AppointmentEventArgs>? UrgentAppointmentBooked;
        public int Count => _appointments.Count;

        public AppointmentManager(PatientManager patients, DoctorManager doctors)
        {
            _patients = patients;
            _doctors = doctors;
        }
        public Appointment Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30, decimal discountMultiplier = 1.0m)
        {
            Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes, discountMultiplier);
            RegisterAppointment(appointment, "");
            return appointment;
        }

        public Appointment BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string symptoms, int durationMinutes = 30)
        {
            Appointment appointment = new UrgentAppointment(patientId, doctorId, scheduledAt, symptoms, durationMinutes);
            RegisterAppointment(appointment, symptoms);
            UrgentAppointmentBooked?.Invoke(this, new AppointmentEventArgs(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ScheduledAt,
                symptoms ?? ""
            ));
            return appointment;
        }

        public Appointment BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string referralSource, bool requiresFollowUp = false, int durationMinutes = 45)
        {
            Appointment appointment = new SpecialistAppointment(patientId, doctorId, scheduledAt, referralSource, durationMinutes, 1.0m);
            RegisterAppointment(appointment, referralSource);
            return appointment;
        }
        public bool Cancel(int appointmentId, string reason = "")
        {
            Appointment? appointment = FindById(appointmentId);
            if (appointment != null && appointment.Cancel(reason))
            {
                AppointmentCancelled?.Invoke(this, new AppointmentEventArgs(
                    appointment.Id,
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledAt,
                    appointment.Notes
                ));
                return true;
            }
            return false;
        }
        public bool Complete(int appointmentId)
        {
            Appointment? appointment = FindById(appointmentId);
            if (appointment != null && appointment.Complete())
            {
                AppointmentCompleted?.Invoke(this, new AppointmentEventArgs(
                    appointment.Id,
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledAt,
                    appointment.Notes
                ));
                return true;
            }
            return false;
        }

        private void RegisterAppointment(Appointment appointment, string notes)
        {
            _appointments.Add(appointment);
            AppointmentBooked?.Invoke(this, new AppointmentEventArgs(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ScheduledAt,
                notes
            ));
        }
        public Appointment? FindById(int id)
        {
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i].Id == id)
                {
                    return _appointments[i];
                }
            }
            return null;
        }
        public Appointment[] GetByPatient(int patientId)
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }
        public Appointment[] GetByDate(DateTime date)
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }

        public Appointment[] GetUpcoming()
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }
        public Appointment[] GetUrgent()
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i] is UrgentAppointment)
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }
        public Appointment[] GetSpecialist()
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (_appointments[i] is SpecialistAppointment)
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }
        public Appointment[] GetRegular()
        {
            List<Appointment> result = new List<Appointment>();
            for (int i = 0; i < _appointments.Count; i++)
            {
                if (!(_appointments[i] is UrgentAppointment) && !(_appointments[i] is SpecialistAppointment))
                {
                    result.Add(_appointments[i]);
                }
            }
            return result.ToArray();
        }
        public Appointment[] GetAll()
        {
            return _appointments.ToArray();
        }
        public static int CancelAll(Appointment[] appointments, string reason = "")
        {
            int count = 0;
            for (int i = 0; i < appointments.Length; i++)
            {
                if (appointments[i].Cancel(reason))
                {
                    count++;
                }
            }
            return count;
        }
        public int CancelAllForPatient(int patientId, string reason = "")
        {
            Appointment[] records = GetByPatient(patientId);
            int count = 0;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i].Cancel(reason))
                {
                    count++;
                    AppointmentCancelled?.Invoke(this, new AppointmentEventArgs(
                        records[i].Id,
                        records[i].PatientId,
                        records[i].DoctorId,
                        records[i].ScheduledAt,
                        records[i].Notes
                    ));
                }
            }
            return count;
        }
        public void DisplayAppointments(Appointment[] appointments)
        {
            if (appointments.Length == 0)
            {
                Console.WriteLine("Записів немає.");
                return;
            }
            for (int i = 0; i < appointments.Length; i++)
            {
                Console.WriteLine(appointments[i]);
            }
        }
    }
}