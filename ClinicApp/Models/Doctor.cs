using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Doctor : ISchedulable
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
            set
            {
                ClinicValidator.ValidateName(value, nameof(FirstName));
                _firstName = value;
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                ClinicValidator.ValidateName(value, nameof(LastName));
                _lastName = value;
            }
        }
        public Speciality Specialty { get; set; }
        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ліцензія не може бути порожньою", nameof(LicenseNumber));
                }
                _licenseNumber = value;
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                ClinicValidator.ValidatePhone(value);

                if (value.StartsWith("+38"))
                {
                    _phone = value.Substring(3);
                }
                else
                {
                    _phone = value;
                }
            }
        }

        public WorkSchedule Schedule { get; set; }
        public string FullName => FirstName + " " + LastName;
        public bool IsAvailableNow => Schedule.IsNow;
        public bool CanAcceptAt(int hour)
        {
            return Schedule.Contains(hour);
        }

        public bool CanSchedule(DateTime at)
        {
            return CanAcceptAt(at.Hour);
        }

        public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
        {
            ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

            int count = Math.Min(slotCount, Schedule.HoursPerDay);
            DateTime[] slots = new DateTime[count];
            DateTime startDate = date.Date;

            for (int i = 0; i < count; i++)
            {
                slots[i] = startDate.AddHours(Schedule.Start + i);
            }

            return slots;
        }
        public Doctor(string firstName, string lastName, Speciality specialty, string licenseNumber, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            Phone = phone;
            Schedule = new WorkSchedule(8, 17);

            Id = _nextId;
            _nextId++;
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