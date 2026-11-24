using ClinicApp.Enums;
using ClinicApp.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class SessionManager
    {
        private readonly string _sessionPath;
        public SessionManager(string sessionPath = "session.dat")
        {
            _sessionPath = sessionPath;
        }
        public bool Exists()
        {
            return File.Exists(_sessionPath);
        }
        public void Save(Clinic clinic)
        {
            Patient[] patients = clinic.Patients.GetAll();
            using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
            writer.WriteLine("[PATIENTS]");

            for (int i = 0; i < patients.Length; i++)
            {
                Patient p = patients[i];
                writer.WriteLine($"{p.FirstName},{p.LastName},{p.BirthDate:dd.MM.yyyy},{p.BloodType},{p.Phone}");
            }

            writer.WriteLine("[END]");
        }
        public int Load(Clinic clinic)
        {
            if (!File.Exists(_sessionPath))
            {
                return 0;
            }
            string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
            string currentSection = "";
            int addedCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].Trim();
                if (trimmed.Length == 0)
                {
                    continue;
                }
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentSection = trimmed;
                    continue;
                }

                if (currentSection != "[PATIENTS]")
                {
                    continue;
                }
                try
                {
                    string[] parts = trimmed.Split(',');
                    if (parts.Length != 5)
                    {
                        continue;
                    }
                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    DateTime birthDate = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                    string phone = parts[4].Trim();

                    Patient candidate = new Patient(firstName, lastName, birthDate, bloodType, phone);

                    if (!IsDuplicate(clinic.Patients.GetAll(), candidate))
                    {
                        clinic.Patients.Add(candidate);
                        addedCount++;
                    }
                }
                catch
                {
                }
            }
            return addedCount;
        }
        private static bool IsDuplicate(Patient[] existing, Patient candidate)
        {
            for (int i = 0; i < existing.Length; i++)
            {
                if (existing[i].FirstName == candidate.FirstName &&
                    existing[i].LastName == candidate.LastName &&
                    existing[i].BirthDate.Date == candidate.BirthDate.Date)
                {
                    return true;
                }
            }
            return false;
        }
    }
}