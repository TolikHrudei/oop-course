using System;

namespace ClinicApp.Models
{
    public class RegularAppointment : Appointment
    {
        public RegularAppointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30, decimal discountMultiplier = 1.0m)
            : base(patientId, doctorId, scheduledAt, durationMinutes, discountMultiplier)
        {
        }
        public override string GetDescription()
        {
            return "Звичайний прийом";
        }
    }
}