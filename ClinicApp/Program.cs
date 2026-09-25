using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Clinic clinic = new Clinic("Медична Клініка");

        RunPatientMenu(clinic);
        RunDoctorMenu(clinic);
        RunAppointmentMenu(clinic);
        TestGrowablePatientManager();

        clinic.DisplaySchedule(new DateTime(2027, 5, 9));
        clinic.GenerateReport();
    }

    static void RunPatientMenu(Clinic clinic)
    {
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 1, 4), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));
    }

    static void RunDoctorMenu(Clinic clinic)
    {
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія");
        d3.LicenseNumber = "LIC-003";
        d3.Phone = "0443456789";

        clinic.Doctors.Add(d1);
        clinic.Doctors.Add(d2);
        clinic.Doctors.Add(d3);
    }
    static void TestGrowablePatientManager()
    {
        Console.WriteLine("=== Тест GrowablePatientManager ===");
        Console.WriteLine("Додаємо пацієнтів одного за одним...");

        GrowablePatientManager manager = new GrowablePatientManager();
        for (int i = 1; i <= 20; i++)
        {
            Patient p = new Patient("Тест", $"Пацієнт{i}");
            manager.Add(p);

            Console.WriteLine($"Додано [{i}]. Розмір: {manager.Count} / {manager.Capacity}");
        }

        Console.WriteLine("\nТест пошуку:");

        Patient? p10 = manager.FindById(10);
        Console.WriteLine($"FindById(10) -> {(p10 != null ? $"{p10.FirstName} {p10.LastName}" : "не знайдено")}");

        Patient? p99 = manager.FindById(99);
        Console.WriteLine($"FindById(99) -> {(p99 != null ? $"{p99.FirstName} {p99.LastName}" : "не знайдено")}");

        Console.WriteLine("\nПорівняння:");
        Console.WriteLine("PatientManager:         100 місць (фіксовано)");
        Console.WriteLine($"GrowablePatientManager: {manager.Capacity} місця (зросте при потребі)");
    }

    static void RunAppointmentMenu(Clinic clinic)
    {
        clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0));
        clinic.Appointments.Book(2, 2, new DateTime(2027, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2027, 5, 10, 9, 0, 0), 20);
    }
}