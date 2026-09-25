using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лікарі ===");
        Doctor[] doctors = new Doctor[4];
        int count_ = 0;

        doctors[count_] = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        doctors[count_].WorkEndHour = 16;
        count_++;

        doctors[count_] = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        doctors[count_].WorkStartHour = 9;
        doctors[count_].WorkEndHour = 18;
        count_++;

        doctors[count_] = new Doctor("Андрій", "Власенко", "Педіатрія");
        doctors[count_].LicenseNumber = "LIC-003";
        doctors[count_].Phone = "0443456789";
        count_++;

        doctors[count_] = new Doctor();
        count_++;

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
            Console.WriteLine($"Ви ввели годину: {hour}. (Тут можна викликати CanAcceptAt)");
        }
        else
        {
            Console.WriteLine("Помилка: введено некоректне число!");
        }
    }
}