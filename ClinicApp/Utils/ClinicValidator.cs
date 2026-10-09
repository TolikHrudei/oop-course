using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils
{
    public static class ClinicValidator
    {
        private static readonly Regex PhoneRegex = new Regex(@"^(?:\+38)?[0-9]{10}\z");
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

        public static void ValidateName(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            {
                throw new ArgumentException("Некоректне значення: порожнє або перевищує 50 символів.", fieldName);
            }
        }
        public static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Телефон не може бути порожнім.", nameof(phone));
            }

            if (!PhoneRegex.IsMatch(phone))
            {
                throw new ArgumentException("Некоректний формат телефону. Очікується 10 цифр (або +38 та 10 цифр).", nameof(phone));
            }
        }
        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            if (!EmailRegex.IsMatch(email))
            {
                throw new ArgumentException("Некоректний формат email адреси.", nameof(email));
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