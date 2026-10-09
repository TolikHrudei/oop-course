using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
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
        public WorkSchedule Schedule { get; set; }
        public string FullName => FirstName + " " + LastName;
        public bool IsAvailableNow => Schedule.IsNow;
        public bool CanAcceptAt(int hour)
        {
            return Schedule.Contains(hour);
        }

        public Doctor(string firstName, string lastName, Speciality specialty, string licenseNumber, string phone)
        {
            Id = _nextId;
            _nextId++;
            FirstName = firstName;
            LastName = lastName;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            Phone = phone;

            Schedule = new WorkSchedule (8, 17);
        }

        public Doctor(string firstName, string lastName, Speciality specialty)
            : this(firstName, lastName, specialty, "Невідомо", "0000000000")
        {
        }

        public Doctor()
           : this("Невідомий", "Лікар", Speciality.General) 
        {
        }

        public override string ToString()
        {
            string status = IsAvailableNow ? "доступний" : "не в робочий час";
            string specFormatted = ClinicFormatter.FormatSpeciality(Specialty);
            string phoneFormatted = ClinicFormatter.FormatPhone(Phone);

            return $"[{Id}] {FullName} | {specFormatted} | {LicenseNumber} | Тел. {phoneFormatted}, {Schedule} | {status}";
        }
    }
}