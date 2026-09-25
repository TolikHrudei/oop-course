using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public class Appointment
    {
        private static int _nextId = 0;
        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes {  get; set; }
        public string Status { get; private set; }
        public string Notes { get; private set; }
        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";
        public Appointment(int patientId, int doctorId, DateTime scheduledAt,  int durationMinutes = 30)
        {
            Id = _nextId++;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Status = "Scheduled";
            Notes = "";
        }
        public bool Cancel(string reason = "")
        {
            if (Status == "Scheduled")
            {
                Status = "Canceled";
                if (!string.IsNullOrEmpty(reason))
                {
                    Notes = reason;
                }
                return true;
            }
            return false;
        }
        public bool Complete()
        {
            if(Status == "Scheduled")
            {
                Status = "Completed";
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            string info = $"[{Id}] Пацієнт #{PatientId} -> #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}-{EndsAt:HH:mm} | {Status}";
            if (Notes.Length > 0)
            {
                info += $" | {Notes}";
            }
            return info;
        }
    }
}
