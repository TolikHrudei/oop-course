using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();
        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);
        RunPatientMenu(patientManager);
        RunDoctorMenu(doctorManager);
        RunAppointmentMenu(appointmentManager);
    }

    static void RunPatientMenu(PatientManager manager)
    {
        Patient p1 = new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567");
        Patient p2 = new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678");
        Patient p3 = new Patient("Максим", "Бойко", new DateTime(2010, 1, 4), "B-", "0672345678");
        Patient p4 = new Patient("Марія", "Ткач");

        manager.Add(p1);
        manager.Add(p2);
        manager.Add(p3);
        manager.Add(p4);
        manager.DisplayAll();
        manager.DisplayStats();
    }

    static void RunDoctorMenu(DoctorManager manager) { 
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія");
        d3.LicenseNumber = "LIC-003";
        d3.Phone = "0443456789";

        manager.Add(d1);
        manager.Add(d2);
        manager.Add(d3);
        manager.DisplayAll();
        manager.DisplayStats();
        Console.WriteLine("\n--- Перевірка доступності ---");
        Console.Write("Введіть годину (0-23) для перевірки: ");
        string input = Console.ReadLine()!;

        if (int.TryParse(input, out int hour))
        {
            Console.WriteLine($"Ви ввели годину: {hour}.");
        }
        else
        {
            Console.WriteLine("Помилка: введено некоректне число!");
        }
    }

    static void RunAppointmentMenu(AppointmentManager manager)
    {
        Console.WriteLine("\n=== Тестування записів (AppointmentManager) ===");
        manager.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        manager.Book(99, 1, new DateTime(2026, 10, 9, 10, 0, 0));
        manager.Book(2, 2, new DateTime(2026, 9, 9, 11, 0, 0), 45);
        manager.Book(3, 3, new DateTime(2026, 12, 10, 9, 0, 0), 20);

        Console.WriteLine("\nМайбутні записи:");
        manager.DisplayList(manager.GetUpcoming());

        Console.WriteLine();
        manager.Cancel(1, "Пацієнт не зміг прийти"); 

        Console.WriteLine("\nЗаписи пацієнта #2:");
        manager.DisplayList(manager.GetByPatient(2));
    }
}