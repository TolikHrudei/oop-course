using System;

namespace ClinicApp.Models
{
    public sealed class SpecialistAppointment : Appointment
    {
        public string ConsultationTopic { get; }

        public SpecialistAppointment(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45, decimal discountMultiplier = 1.0m)
            : base(patientId, doctorId, scheduledAt, durationMinutes, discountMultiplier)
        {
            ConsultationTopic = topic ?? "";
        }

        public override decimal GetCost()
        {
            return base.GetCost() * 1.3m;
        }
        public override string GetDescription()
        {
            if (string.IsNullOrWhiteSpace(ConsultationTopic))
            {
                return "Консультація спеціаліста";
            }
            return $"Консультація спеціаліста: {ConsultationTopic}";
        }
    }
}