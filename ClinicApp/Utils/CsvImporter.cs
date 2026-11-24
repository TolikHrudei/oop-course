using ClinicApp.Enums;
using ClinicApp.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public static class CsvImporter
    {
        public static ImportResult ImportPatients(Clinic clinic, string filePath)
        {
            ImportResult result = new ImportResult();

            if (!File.Exists(filePath))
            {
                result.AddError(0, "Файл не знайдено.");
                return result;
            }
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                int lineNumber = i + 1;
                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5)
                    {
                        string noun = parts.Length == 1 ? "поле" : "полів";
                        throw new FormatException($"Рядок має {parts.Length} {noun} замість 5.");
                    }
                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    string birthDateStr = parts[2].Trim();
                    string bloodTypeStr = parts[3].Trim();
                    string phone = parts[4].Trim();

                    DateTime birthDate = DateTime.ParseExact(birthDateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodTypeStr);

                    Patient patient = new Patient(firstName, lastName, birthDate, bloodType, phone);
                    clinic.Patients.Add(patient);
                    clinic.Logger.LogInfo($"Імпортовано пацієнта #{patient.Id}: {patient.FullName}");
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError(lineNumber, ex.Message);
                    clinic.Logger.LogWarning($"Помилка імпорту (рядок {lineNumber}): {ex.Message}");
                }
            }
            return result;
        }
    }
}