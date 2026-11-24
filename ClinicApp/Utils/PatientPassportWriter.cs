using ClinicApp.Events;
using ClinicApp.Models;
using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class PatientPassportWriter
    {
        private readonly Clinic _clinic;
        private readonly string _passportDir;
        public PatientPassportWriter(Clinic clinic, string passportDir = "patients")
        {
            _clinic = clinic;
            _passportDir = passportDir;
            Directory.CreateDirectory(_passportDir);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            WritePassport(e.PatientId);
        }
        public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        private void WritePassport(int patientId)
        {
            Patient? patient = _clinic.Patients.FindById(patientId);
            if (patient == null)
            {
                return;
            }

            Directory.CreateDirectory(_passportDir);
            string filePath = Path.Combine(_passportDir, $"passport_{patient.Id}.txt");

            MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patient.Id);
            Appointment[] appointments = _clinic.Appointments.GetByPatient(patient.Id);
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patient.Id);
            decimal debt = _clinic.Billing.GetPatientDebt(patient.Id);

            using StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine();

            writer.WriteLine($"Ім'я: {patient.FullName}");
            writer.WriteLine($"Дата народження: {patient.BirthDate:dd.MM.yyyy} ({patient.Age})");
            writer.WriteLine($"Група крові: {patient.BloodType}   Телефон: {patient.Phone}");
            writer.WriteLine();

            writer.WriteLine("-- Діагнози --");
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Diagnosis d)
                {
                    string chronicLabel = d.IsChronic ? " [хронічне]" : "";
                    writer.WriteLine($"{d.Description}{chronicLabel}");
                }
            }
            writer.WriteLine();
            writer.WriteLine("-- Аналізи --");
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is LabResult lr)
                {
                    string statusLabel = lr.IsNormal ? "в нормі" : "поза нормою";
                    writer.WriteLine($"{lr.TestName}: {lr.Value} {lr.Unit} (норма: {lr.ReferenceRange}) — {statusLabel}");
                }
            }
            writer.WriteLine();

            writer.WriteLine("-- Рецепти --");
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Prescription rx)
                {
                    string instrLabel = string.IsNullOrWhiteSpace(rx.Instructions) ? "" : $" ({rx.Instructions})";
                    writer.WriteLine($"{rx.MedicationName} {rx.Dosage} x {rx.DurationDays} днів{instrLabel}");
                }
            }
            writer.WriteLine();
            writer.WriteLine("-- Записи --");
            for (int i = 0; i < appointments.Length; i++)
            {
                Appointment app = appointments[i];
                writer.WriteLine($"[{app.Id}] {app.ScheduledAt:dd.MM.yyyy HH:mm} | {app.Status} | {app.GetCost():F2} грн");
            }
            writer.WriteLine();
            writer.WriteLine("-- Плани лікування --");
            for (int i = 0; i < plans.Length; i++)
            {
                TreatmentPlan plan = plans[i];
                writer.WriteLine($"[{plan.Id}] {plan.Diagnosis} | {plan.DurationDays} днів | {plan.Status}");
            }
            writer.WriteLine();

            writer.WriteLine($"Заборгованість: {debt:F2} грн");
        }
    }
}