using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Patient
    {
        private static int _nextId = 1;

        private string _firstName = "";
        private string _lastName = "";
        private DateTime _dateOfBirth;
        private string _phone = "";

        public int Id { get; }
        public BloodType BloodType { get; set; }
        public string Email { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                {
                    throw new ArgumentException("Некоректне ім'я", nameof(FirstName));
                }
                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                {
                    throw new ArgumentException("Некоректне прізвище", nameof(LastName));
                }
                _lastName = value;
            }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (value > DateTime.Today || value.Year < 1900)
                {
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Некоректна дата народження");
                }
                _dateOfBirth = value;
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 10)
                {
                    throw new ArgumentException("Некоректна довжина телефону", nameof(Phone));
                }

                foreach (char c in value)
                {
                    if (!char.IsDigit(c))
                    {
                        throw new ArgumentException("Телефон має містити лише цифри", nameof(Phone));
                    }
                }

                _phone = value;
            }
        }

        public string FullName => FirstName + " " + LastName;

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        public bool IsAdult => Age >= 18;

        public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            BloodType = bloodType;
            Phone = phone;
            Email = "";

            Id = _nextId;
            _nextId++;
        }

        public Patient(string firstName, string lastName)
            : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000")
        {
        }

        public Patient()
            : this("Невідомий", "Пацієнт")
        {
        }

        public override string ToString()
        {
            string bloodFormatted = ClinicFormatter.FormatBloodType(BloodType);
            string ageFormatted = ClinicFormatter.FormatAge(Age);
            string phoneFormatted = ClinicFormatter.FormatPhone(Phone);

            return $"[{Id}] {FullName}, {ageFormatted}, Кров: {bloodFormatted}, Тел. {phoneFormatted}";
        }

        public string GetAgeCategory()
        {
            if (Age < 18)
            {
                return "дитина";
            }
            else if (Age >= 60)
            {
                return "літній";
            }
            else
            {
                return "дорослий";
            }
        }
    }
}