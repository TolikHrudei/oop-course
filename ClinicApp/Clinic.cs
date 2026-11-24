using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;
using System;

namespace ClinicApp
{
    public class Clinic
    {
        public string Name { get; }
        public PatientManager Patients { get; }
        public DoctorManager Doctors { get; }
        public AppointmentManager Appointments { get; }
        public MedicalRecordManager MedicalRecords { get; }
        public BillingManager Billing { get; }
        public WaitingQueue<Patient> WaitingRoom { get; }
        public AnalyticsManager Analytics { get; }
        public TreatmentPlanManager TreatmentPlans { get; }
        public ClinicLogger Logger { get; }
        public ClinicExporter Exporter { get; }
        public SessionManager Session { get; }
        public PatientPassportWriter Passport { get; }
        public Clinic(string name)
        {
            Name = name;
            Patients = new PatientManager();
            Doctors = new DoctorManager();
            Appointments = new AppointmentManager(Patients, Doctors);
            MedicalRecords = new MedicalRecordManager();
            Billing = new BillingManager(Appointments);
            WaitingRoom = new WaitingQueue<Patient>();
            Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
            TreatmentPlans = new TreatmentPlanManager();
            Logger = new ClinicLogger();
            Exporter = new ClinicExporter(this);
            Session = new SessionManager();
            Passport = new PatientPassportWriter(this);

            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            Patients.PatientAdded += Logger.OnPatientAdded;
            Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
            Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
            Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
            Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
            Billing.PaymentReceived += Logger.OnPaymentReceived;
            TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;
            Patients.PatientAdded += Passport.OnPatientAdded;
            Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
            TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;
        }
        public void DisplaySchedule(DateTime date)
        {
            Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");
            var apps = Appointments.GetByDate(date);
            if (apps.Length == 0)
            {
                Console.WriteLine("На цю дату записів немає.");
                return;
            }
            Appointments.DisplayAppointments(apps);
        }
        public void GenerateReport()
        {
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine($"│ Звіт — {Name,-31} │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Пацієнтів:          {Patients.Count,-18} │");
            Console.WriteLine($"│ Лікарів:            {Doctors.Count,-18} │");
            Console.WriteLine($"│ Майбутніх записів:  {Appointments.GetUpcoming().Length,-18} │");
            Console.WriteLine($"│ Медичних записів:   {MedicalRecords.Count,-18} │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }
}