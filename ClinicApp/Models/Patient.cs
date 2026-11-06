using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Patient : IIdentifiable
    {
        private static int _nextId = 1;

        private string _firstName = "";
        private string _lastName = "";
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

        public DateTime BirthDate { get; set; }
        public BloodType BloodType { get; set; }

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

        public string FullName => FirstName + " " + LastName;

        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - BirthDate.Year;
                if (BirthDate.Date > DateTime.Today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        public Patient(string firstName, string lastName, DateTime birthDate, BloodType bloodType, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            BloodType = bloodType;
            Phone = phone;

            Id = _nextId;
            _nextId++;
        }

        public Patient(string firstName, string lastName)
            : this(firstName, lastName, DateTime.Today, BloodType.Unknown, "0000000000")
        {
        }

        public Patient()
            : this("Невідомий", "Пацієнт")
        {
        }

        public override string ToString()
        {
            string bloodStr = ClinicFormatter.FormatBloodType(BloodType);
            string phoneStr = ClinicFormatter.FormatPhone(Phone);
            return $"[{Id}] {FullName}, {Age} р. | {bloodStr} | Тел. {phoneStr}";
        }
    }
}