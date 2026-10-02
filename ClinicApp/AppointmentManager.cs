using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public class AppointmentManager
    {
        private const int MaxAppointments = 500;
        private Appointment[] _appointments = new Appointment[MaxAppointments];
        private int _count = 0;

        private PatientManager _patients;
        private DoctorManager _doctors;

        public int Count => _count;

        public AppointmentManager(PatientManager patients, DoctorManager doctors)
        {
            _patients = patients;
            _doctors = doctors;
        }
        public Appointment? this[int index]
        {
            get
            {
                if (index >= 0 && index < _count)
                {
                    return _appointments[index];
                }
                return null;
            }
        }
        private Appointment? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].Id == id)
                {
                    return _appointments[i];
                }
            }
            return null;
        }

        public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            if (_count >= MaxAppointments)
            {
                Console.WriteLine("Помилка: досягнуто ліміт записів.");
                return false;
            }

            Patient? patient = _patients.FindById(patientId);
            if (patient == null)
            {
                Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
                return false;
            }

            Doctor? doctor = _doctors.FindById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
                return false;
            }

            Appointment newApp = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
            _appointments[_count] = newApp;
            _count++;

            Console.WriteLine($"Запис [{newApp.Id}] створено: {patient.FullName} -> {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
            return true;
        }

        public bool Cancel(int id, string reason = "")
        {
            Appointment? app = FindById(id);
            if (app != null && app.Cancel(reason))
            {
                Console.WriteLine($"Запис [{id}] скасовано.");
                return true;
            }
            return false;
        }

        public bool Complete(int id)
        {
            Appointment? app = FindById(id);
            if (app != null && app.Complete())
            {
                return true;
            }
            return false;
        }

        public Appointment[] GetByPatient(int patientId)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].PatientId == patientId)
                    matchCount++;
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].PatientId == patientId)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }

        public Appointment[] GetByDoctor(int doctorId)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].DoctorId == doctorId)
                    matchCount++;
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].DoctorId == doctorId)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }

        public Appointment[] GetByDate(DateTime date)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].ScheduledAt.Date == date.Date)
                    matchCount++;
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].ScheduledAt.Date == date.Date)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }
        public Appointment[] GetByDate(int year, int month, int day)
        {
            DateTime targetDate = new DateTime(year, month, day);
            return GetByDate(targetDate);
        }

        public Appointment[] GetUpcoming()
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].IsUpcoming)
                    matchCount++;
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] != null && _appointments[i].IsUpcoming)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }

        public void DisplayAppointment(Appointment app)
        {
            Patient? patient = _patients.FindById(app.PatientId);
            string patientName = patient != null ? patient.FullName : $"Пацієнт #{app.PatientId}";

            Doctor? doctor = _doctors.FindById(app.DoctorId);
            string doctorName = doctor != null ? doctor.FullName : $"Лікар #{app.DoctorId}";

            string info = $"[{app.Id}] {patientName} -> {doctorName} | {app.ScheduledAt:dd.MM.yyyy HH:mm}-{app.EndsAt:HH:mm} | {app.Status}";
            if (app.Notes.Length > 0)
            {
                info += $" | {app.Notes}";
            }
            Console.WriteLine(info);
        }

        public void DisplayList(Appointment[] list)
        {
            if (list == null || list.Length == 0)
            {
                Console.WriteLine("Записів не знайдено.");
                return;
            }

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    DisplayAppointment(list[i]);
                }
            }
        }
    }
}
