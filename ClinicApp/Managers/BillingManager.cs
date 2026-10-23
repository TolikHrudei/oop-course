using ClinicApp.Interfaces;
using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointmentManager;
        public BillingManager(AppointmentManager appointmentManager)
        {
            _appointmentManager = appointmentManager;
        }
        private IPayable[] FilterUnpaid(Appointment[] source)
        {
            if (source == null)
            {
                return new IPayable[0];
            }

            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != null && !source[i].IsPaid && !source[i].IsCancelled)
                {
                    count++;
                }
            }

            IPayable[] result = new IPayable[count];
            int index = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != null && !source[i].IsPaid && !source[i].IsCancelled)
                {
                    result[index] = source[i];
                    index++;
                }
            }
            return result;
        }
        public IPayable[] GetAllUnpaid()
        {
            Appointment[] all = _appointmentManager.GetAll();
            return FilterUnpaid(all);
        }
        public IPayable[] GetUnpaidByPatient(int patientId)
        {
            Appointment[] patientApps = _appointmentManager.GetByPatient(patientId);
            return FilterUnpaid(patientApps);
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
        public bool PayAppointment(int appointmentId)
        {
            Appointment? app = _appointmentManager.FindById(appointmentId);
            if (app == null || app.IsPaid || app.IsCancelled)
            {
                return false;
            }
            app.MarkPaid();
            return true;
        }
        public void DisplayUnpaid(IPayable[] items)
        {
            if (items == null || items.Length == 0)
            {
                Console.WriteLine("Немає неоплачених записів.");
                return;
            }
            for (int i = 0; i < items.Length; i++)
            {
                IPayable item = items[i];
                if (item is Appointment app)
                {
                    Console.WriteLine($"{app} | Сума: {item.GetCost():F2} грн");
                }
                else
                {
                    Console.WriteLine($"Запис | Сума: {item.GetCost():F2} грн");
                }
            }
        }
    }
}
