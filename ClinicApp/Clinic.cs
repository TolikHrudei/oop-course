using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public class Clinic
    {
        public string Name { get; }
        public PatientManager Patients { get; }
        public DoctorManager Doctors { get; }
        public AppointmentManager Appointments { get; }

        public Clinic(string name)
        {
            Name = name;
            Patients = new PatientManager();
            Doctors = new DoctorManager();
            Appointments = new AppointmentManager(Patients, Doctors);
        }

        public void DisplaySchedule(DateTime date)
        {
            Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");
            Appointment[] dailyAppointments = Appointments.GetByDate(date);
            Appointments.DisplayList(dailyAppointments);
        }

        public void GenerateReport()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine($"║ Звіт — {Name,-37} ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"║ Пацієнтів:         {Patients.Count,-25} ║");
            Console.WriteLine($"║ Лікарів:           {Doctors.Count,-25} ║");

            Appointment[] upcoming = Appointments.GetUpcoming();
            Console.WriteLine($"║ Майбутніх записів: {upcoming.Length,-25} ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║ Навантаження лікарів (майбутні записи):      ║");

            Doctor[] allDoctors = Doctors.GetAll();
            for (int i = 0; i < allDoctors.Length; i++)
            {
                if (allDoctors[i] == null) continue;

                int docLoad = 0;
                for (int j = 0; j < upcoming.Length; j++)
                {
                    if (upcoming[j] != null && upcoming[j].DoctorId == allDoctors[i].Id)
                    {
                        docLoad++;
                    }
                }
                string loadString = $"  {allDoctors[i].FullName} ({allDoctors[i].Specialty}): {docLoad} записів";
                Console.WriteLine($"║ {loadString,-44} ║");
            }
            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }
    }
}
