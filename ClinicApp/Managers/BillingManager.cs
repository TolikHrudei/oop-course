using ClinicApp.Events;
using ClinicApp.Interfaces;
using ClinicApp.Models;
using System;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointments;
        public event EventHandler<PaymentEventArgs>? PaymentReceived;
        public BillingManager(AppointmentManager appointments)
        {
            _appointments = appointments;
        }
        public IPayable[] GetUnpaidByPatient(int patientId)
        {
            Appointment[] patientApps = _appointments.GetByPatient(patientId);
            List<IPayable> unpaid = new List<IPayable>();

            for (int i = 0; i < patientApps.Length; i++)
            {
                if (!patientApps[i].IsCancelled && !patientApps[i].IsPaid)
                {
                    unpaid.Add(patientApps[i]);
                }
            }

            return unpaid.ToArray();
        }
        public IPayable[] GetAllUnpaid()
        {
            Appointment[] allApps = _appointments.GetAll();
            List<IPayable> unpaid = new List<IPayable>();

            for (int i = 0; i < allApps.Length; i++)
            {
                if (!allApps[i].IsCancelled && !allApps[i].IsPaid)
                {
                    unpaid.Add(allApps[i]);
                }
            }

            return unpaid.ToArray();
        }
        public decimal GetPatientDebt(int patientId)
        {
            IPayable[] unpaid = GetUnpaidByPatient(patientId);
            decimal total = 0m;
            for (int i = 0; i < unpaid.Length; i++)
            {
                total += unpaid[i].GetCost();
            }
            return total;
        }
        public decimal GetTotalDebt()
        {
            IPayable[] unpaid = GetAllUnpaid();
            decimal total = 0m;
            for (int i = 0; i < unpaid.Length; i++)
            {
                total += unpaid[i].GetCost();
            }
            return total;
        }
        public bool PayAppointment(int appointmentId)
        {
            Appointment? app = _appointments.FindById(appointmentId);
            if (app == null || app.IsCancelled || app.IsPaid)
            {
                return false;
            }

            decimal amount = app.GetCost();
            app.MarkPaid();
            PaymentReceived?.Invoke(this, new PaymentEventArgs(app.Id, amount));
            return true;
        }
        public void DisplayUnpaid(IPayable[] unpaid)
        {
            if (unpaid.Length == 0)
            {
                Console.WriteLine("Неоплачених записів немає.");
                return;
            }
            for (int i = 0; i < unpaid.Length; i++)
            {
                Console.WriteLine(unpaid[i]);
            }
        }
    }
}