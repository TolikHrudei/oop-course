using ClinicApp.Interfaces;
using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class AppointmentManager
    {
        private const int MaxAppointments = 1000;
        private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
        private int _count = 0;
        private readonly PatientManager _patients;
        private readonly DoctorManager _doctors;
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
                if (index < 0 || index >= _count)
                {
                    return null;
                }
                return _appointments[index];
            }
        }
        public static int CancelAll(ICancellable[] items, string reason = "")
        {
            if (items == null)
            {
                return 0;
            }

            int cancelledCount = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].Cancel(reason))
                {
                    cancelledCount++;
                }
            }
            return cancelledCount;
        }

        public Appointment? Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            if (_count >= MaxAppointments)
            {
                Console.WriteLine("Неможливо створити запис: ліміт досягнуто.");
                return null;
            }

            var patient = _patients.FindById(patientId);
            if (patient == null)
            {
                Console.WriteLine($"Пацієнта з ID #{patientId} не знайдено.");
                return null;
            }

            var doctor = _doctors.FindById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine($"Лікаря з ID #{doctorId} не знайдено.");
                return null;
            }

            Appointment app = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
            _appointments[_count] = app;
            _count++;
            Console.WriteLine($"Запис [{app.Id}] створено: {patient.FullName} -> {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
            return app;
        }

        public Appointment? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].Id == id)
                {
                    return _appointments[i];
                }
            }
            return null;
        }

        public Appointment[] GetAll()
        {
            Appointment[] result = new Appointment[_count];
            for (int i = 0; i < _count; i++)
            {
                result[i] = _appointments[i];
            }
            return result;
        }

        public Appointment[] GetByPatient(int patientId)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
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
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }

        public Appointment[] GetUpcoming()
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    result[index] = _appointments[i];
                    index++;
                }
            }
            return result;
        }

        public void DisplayAll()
        {
            if (_count == 0)
            {
                Console.WriteLine("Список записів на прийом порожній.");
                return;
            }
            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine(_appointments[i]);
            }
        }
    }
}