using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Utils
{
    public static class ClinicValidator
    {
        public static void ValidateName(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            {
                throw new ArgumentException("Некоректне значення: порожнє або перевищує 50 символів.", fieldName);
            }
        }
        public static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10)
            {
                throw new ArgumentException("Телефон має містити рівно 10 символів.", nameof(phone));
            }

            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    throw new ArgumentException("Телефон має містити лише цифри.", nameof(phone));
                }
            }
        }

        public static void ValidateDate(DateTime value, string fieldName)
        {
            if (value > DateTime.Today || value.Year < 1900)
            {
                throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому або раніше 1900 року.");
            }
        }
        public static void ValidatePositive(int value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(fieldName, "Значення має бути більшим за нуль.");
            }
        }
    }
}
