using ClinicApp.Managers;
using System;

namespace ClinicApp
{
    public class Clinic
    {
        public string Name { get; }
        public PatientManager Patients { get; }
        public DoctorManager Doctors { get; }
        public AppointmentManager Appointments { get; }
        public MedicalRecordManager MedicalRecords { get; }

        public Clinic(string name)
        {
            Name = name;
            Patients = new PatientManager();
            Doctors = new DoctorManager();
            Appointments = new AppointmentManager(Patients, Doctors);
            MedicalRecords = new MedicalRecordManager();
        }
        public void DisplaySchedule(DateTime date)
        {
            Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");
            var apps = Appointments.GetByDate(date);
            if (apps.Length == 0)
            {
                Console.WriteLine("На цю дату записів немає.");
                return;
            }

            for (int i = 0; i < apps.Length; i++)
            {
                var app = apps[i];
                var patient = Patients.FindById(app.PatientId);
                var doctor = Doctors.FindById(app.DoctorId);

                string patientName = patient != null ? patient.FullName : $"Пацієнт #{app.PatientId}";
                string doctorName = doctor != null ? doctor.FullName : $"Лікар #{app.DoctorId}";

                Console.WriteLine($"[{app.Id}] {patientName} -> {doctorName} | {app.ScheduledAt:dd.MM.yyyy HH:mm}-{app.EndsAt:HH:mm} | {app.Status}");
            }
        }
        public void GenerateReport()
        {
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine($"│ Звіт — {Name,-31} │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Пацієнтів:          {Patients.Count,-18} │");
            Console.WriteLine($"│ Лікарів:            {Doctors.Count,-18} │");
            Console.WriteLine($"│ Майбутніх записів:  {Appointments.GetUpcoming().Length,-18} │");
            Console.WriteLine($"│ Медичних записів:   {MedicalRecords.Count,-18} │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }
}