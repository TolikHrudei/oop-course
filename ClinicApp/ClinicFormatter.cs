using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public static class ClinicFormatter
    {
        public static string FormatBloodType(BloodType bt)
        {
            return bt switch
            {
                BloodType.Unknown => "Невідомо",
                BloodType.APositive => "A+",
                BloodType.ANegative => "A-",
                BloodType.BPositive => "B+",
                BloodType.BNegative => "B-",
                BloodType.ABPositive => "AB+",
                BloodType.ABNegative => "AB-",
                BloodType.OPositive => "O+",
                BloodType.ONegative => "O-",
                _ => "Невідомо" 
            };
        }

        public static string FormatSpeciality(Speciality s)
        {
            return s switch
            {
                Speciality.General => "Загальна практика",
                Speciality.Cardiology => "Кардіологія",
                Speciality.Neurology => "Неврологія",
                Speciality.Pediatrics => "Педіатрія",
                _ => s.ToString()
            };
        }
        public static string FormatAge(int age)
        {
            int remainder100 = age % 100;
            if (remainder100 >= 11 && remainder100 <= 19)
            {
                return $"{age} років";
            }

            int remainder10 = age % 10;
            if (remainder10 == 1) return $"{age} рік";
            if (remainder10 >= 2 && remainder10 <= 4) return $"{age} роки";
            return $"{age} років";
        }
        public static string FormatPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length != 10)
                return phone;
            foreach (char c in phone)
            {
                if (!char.IsDigit(c)) return phone;
            }
            return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
        }
    }
}
