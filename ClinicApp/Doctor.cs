using System;

namespace ClinicApp
{
    public class Doctor
    {
        private static int _nextId = 1;
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Speciality Specialty { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public struct WorkSchedule
        {
            public int Start { get; set; }
            public int End { get; set; }

            public int HoursPerDay => End - Start;
            public string Display => $"{Start:D2}:00-{End:D2}:00";
            public bool IsNow()
            {
                int currentHour = DateTime.Now.Hour;
                return currentHour >= Start && currentHour < End;
            }
        }
        public WorkSchedule Schedule { get; set; }

        public string FullName => FirstName + " " + LastName;
        public bool IsAvailableNow => Schedule.IsNow();

        public Doctor(string firstName, string lastName, Speciality specialty, string licenseNumber, string phone)
        {
            Id = _nextId;
            _nextId++;
            FirstName = firstName;
            LastName = lastName;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            Phone = phone;

            Schedule = new WorkSchedule { Start = 8, End = 17 };
        }

        public Doctor(string firstName, string lastName, Speciality specialty)
            : this(firstName, lastName, specialty, "Невідомо", "0000000000")
        {
        }

        public Doctor()
           : this("Невідомий", "Лікар", Speciality.General) 
        {
        }

        public bool CanAcceptAt(int hour)
        {
            return hour >= Schedule.Start && hour < Schedule.End;
        }

        public override string ToString()
        {
            string status = IsAvailableNow ? "доступний" : "не в робочий час";
            return $"[{Id}] {FullName} | {Specialty} | {LicenseNumber} | Тел. {Phone}, {Schedule.Display}, ({Schedule.HoursPerDay} год) | {status}";
        }
    }
}