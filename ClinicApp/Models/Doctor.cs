using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Doctor
    {
        private static int _nextId = 1;

        private string _firstName = "";
        private string _lastName = "";
        private string _licenseNumber = "";
        private string _phone = "";

        public int Id { get; }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public Speciality Specialty { get; set; }

        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set { _licenseNumber = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

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

            Schedule = new WorkSchedule(8, 17);
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