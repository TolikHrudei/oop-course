using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лікарі ===");
        Doctor[] doctors = new Doctor[4];
        int _count = 0;

        doctors[_count] = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        doctors[_count].WorkEndHour = 16;
        _count++;

        doctors[_count] = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        doctors[_count].WorkStartHour = 9;
        doctors[_count].WorkEndHour = 18;
        _count++;

        doctors[_count] = new Doctor("Андрій", "Власенко", "Педіатрія");
        doctors[_count].LicenseNumber = "LIC-003";
        doctors[_count].Phone = "0443456789";
        _count++;

        doctors[_count] = new Doctor();
        _count++;

        for (int i = 0; i < doctors.Length; i++)
        {
            Doctor? currentDoctor = doctors[i];
            if (currentDoctor != null)
            {
                Console.WriteLine(currentDoctor.ToString());
            }
        }

        Console.WriteLine();
        RunPatientMenu();
        RunDoctorMenu();
        RunAppointmentTest(); 
    }

    static void RunPatientMenu()
    {
        PatientManager manager = new PatientManager();

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

    static void RunDoctorMenu()
    {
        DoctorManager manager = new DoctorManager();
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
    static void RunAppointmentTest()
    {
        Console.WriteLine("\n=== Тест прийомів (Appointment) ==="); Appointment a1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0)); 
        Appointment a2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment a3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);
        Console.WriteLine(a1.ToString());
        Console.WriteLine(a2.ToString());
        Console.WriteLine(a3.ToString());

        Console.WriteLine("\n// Після Cancel та Complete:");
        a1.Cancel("Пацієнт не зміг прийти");
        a2.Complete();
        Console.WriteLine(a1.ToString());
        Console.WriteLine(a2.ToString());
    }
}