using ClinicApp.Interfaces;
using ClinicApp.Models;
using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicExporter
    {
        private readonly Clinic _clinic;
        private readonly string _baseDir;
        public ClinicExporter(Clinic clinic, string baseDir = "reports")
        {
            _clinic = clinic;
            _baseDir = baseDir;
        }
        public string ExportPatients()
        {
            string dir = EnsureDateDirectory();
            string path = Path.Combine(dir, "patients.txt");
            Patient[] items = _clinic.Patients.GetAll();

            using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            WriteHeader(writer, "Пацієнти — звіт");

            for (int i = 0; i < items.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {items[i]}");
            }

            WriteFooter(writer, items.Length);
            return path;
        }
        public string ExportAppointments()
        {
            string dir = EnsureDateDirectory();
            string path = Path.Combine(dir, "appointments.txt");
            Appointment[] items = _clinic.Appointments.GetAll();

            using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            WriteHeader(writer, "Записи — звіт");

            for (int i = 0; i < items.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {items[i]}");
            }
            WriteFooter(writer, items.Length);
            return path;
        }
        public string ExportBilling()
        {
            string dir = EnsureDateDirectory();
            string path = Path.Combine(dir, "billing.txt");
            IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
            decimal totalDebt = _clinic.Billing.GetTotalDebt();
            using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            WriteHeader(writer, "Рахунки — звіт");

            for (int i = 0; i < unpaid.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
            }

            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього неоплачених: {unpaid.Length}");
            writer.WriteLine($"Загальний борг: {totalDebt:F2} грн");
            return path;
        }
        public string ExportTreatmentPlans()
        {
            string dir = EnsureDateDirectory();
            string path = Path.Combine(dir, "treatment_plans.txt");
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

            using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            WriteHeader(writer, "Плани лікування — звіт");

            for (int i = 0; i < plans.Length; i++)
            {
                TreatmentPlan p = plans[i];
                writer.WriteLine($"{i + 1,3}. [{p.Id}] Пацієнт #{p.PatientId} | Діагноз: {p.Diagnosis} | Тривалість: {p.DurationDays} дн. | Статус: {p.Status}");
            }

            WriteFooter(writer, plans.Length);
            return path;
        }
        public string[] ExportAll()
        {
            string[] paths = new string[4];
            paths[0] = ExportPatients();
            paths[1] = ExportAppointments();
            paths[2] = ExportBilling();
            paths[3] = ExportTreatmentPlans();
            return paths;
        }
        private string EnsureDateDirectory()
        {
            string dateFolder = DateTime.Today.ToString("yyyy-MM-dd");
            string dir = Path.Combine(_baseDir, dateFolder);
            Directory.CreateDirectory(dir);
            return dir;
        }

        private void WriteHeader(StreamWriter writer, string title)
        {
            writer.WriteLine($"=== {title} ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");
        }

        private void WriteFooter(StreamWriter writer, int count)
        {
            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього: {count}");
        }
    }
}