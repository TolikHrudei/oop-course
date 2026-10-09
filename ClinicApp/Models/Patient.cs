using ClinicApp.Enums;
using ClinicApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Models
{
    public class Patient
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public BloodType BloodType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
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
            Id = _nextId;
            _nextId++;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            BloodType = bloodType;
            Phone = phone;
            Email = "";
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
