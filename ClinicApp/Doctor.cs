using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public class Doctor
    {
        private static int _nextId = 1;
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public int WorkStartHour  { get; set; }
        public int WorkEndHour {  get; set; }
        public string FullName => FirstName + " " + LastName;
        public int WorkHoursPerDay => WorkEndHour - WorkStartHour;
        public string WorkSchedule => $"{WorkStartHour:D2}:00-{WorkEndHour:D2}:00";
        public bool IsAvailableNow => CanAcceptAt(DateTime.Now.Hour);

        public Doctor(string firstName, string lastName, string specialty, string licenseNumber, string phone)
        {
            Id = _nextId;
            _nextId++;
            FirstName = firstName;
            LastName = lastName;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            Phone = phone;
            WorkStartHour = 8;
            WorkEndHour = 17;
        }
        public Doctor(string firstName, string lastName, string specialty)
            : this(firstName, lastName, specialty, "Невідомо", "0000000000")
        {

        }
         public Doctor()
            : this("Невідомий", "Лікар", "Загальна практика")
        {

        }
        public bool CanAcceptAt(int hour)
        {
            return hour >= WorkStartHour && hour < WorkEndHour;
        }

        public override string ToString()
        {
            string status = IsAvailableNow ? "доступний" : "не в робочий час";
            return $"[{Id}] {FullName} | {Specialty} | {LicenseNumber} | Тел. {Phone}, {WorkSchedule}, ({WorkHoursPerDay} год) | {status}";
        }
    }
}
