using ClinicApp.Events;
using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;
        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }
        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e) =>
            LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

        public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) =>
            LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} -> лікар #{e.DoctorId}, {e.ScheduledAt:dd.MM.yyyy HH:mm}");

        public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e) =>
            LogInfo($"Запис #{e.AppointmentId} скасовано (пацієнт #{e.PatientId}, лікар #{e.DoctorId})");

        public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
            LogInfo($"Запис #{e.AppointmentId} завершено (пацієнт #{e.PatientId}, лікар #{e.DoctorId})");
        public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
        {
            LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");
            WriteUrgentAlert(e);
        }
        public void OnPaymentReceived(object? sender, PaymentEventArgs e) =>
            LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");
        public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
            LogInfo($"План лікування #{e.PlanId} завершено (пацієнт #{e.PatientId}, діагноз: {e.Diagnosis})");
        public string[] GetLastLines(int n)
        {
            if (n <= 0 || !File.Exists(_logPath))
            {
                return new string[0];
            }
            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
            int skip = lines.Length - n;
            if (skip < 0)
            {
                skip = 0;
            }
            int resultCount = lines.Length - skip;
            string[] result = new string[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                result[i] = lines[skip + i];
            }

            return result;
        }
        public void Clear()
        {
            File.WriteAllText(_logPath, string.Empty, Encoding.UTF8);
        }
        public bool Exists()
        {
            return File.Exists(_logPath);
        }
        private void WriteUrgentAlert(AppointmentEventArgs e)
        {
            string dir = Path.Combine("alerts");
            Directory.CreateDirectory(dir);
            string fileName = $"urgent_{DateTime.Today:yyyy-MM-dd}.txt";
            string path = Path.Combine(dir, fileName);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] Запис #{e.AppointmentId} | Пацієнт #{e.PatientId} -> Лікар #{e.DoctorId} | {e.Notes}{Environment.NewLine}";
            File.AppendAllText(path, line, Encoding.UTF8);
        }
        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] [{level,-5}] {message}{Environment.NewLine}";
            File.AppendAllText(_logPath, line, Encoding.UTF8);
        }
    }
}