using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Utils;
using ClinicApp;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Clinic clinic = new Clinic("Медична Клініка");

        RunPatientMenu(clinic);
        RunDoctorMenu(clinic);
        RunAppointmentMenu(clinic);

        clinic.DisplaySchedule(new DateTime(2027, 5, 9));
        clinic.GenerateReport();
    }
    static void RunPatientMenu(Clinic clinic)
    {
        try
        {
            clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), BloodType.APositive, "0501234567"));

            Patient invalidPatient = new Patient("", "Коваль", new DateTime(2050, 8, 20), BloodType.BNegative, "0672345678");
            clinic.Patients.Add(invalidPatient);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка вводу дати або діапазону (Пацієнт): {e.Message} [{e.GetType().Name}]");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка вводу тексту або формату (Пацієнт): {e.Message} [{e.GetType().Name}]");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Неочікувана помилка: {e.Message}");
        }
    }

    static void RunDoctorMenu(Clinic clinic)
    {
        try
        {
            Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
            d1.Schedule = new WorkSchedule(8, 16);
            clinic.Doctors.Add(d1);

            Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
            d2.Schedule = new WorkSchedule(20, 6);
            clinic.Doctors.Add(d2);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка вводу розкладу (Лікар): {e.Message} [{e.GetType().Name}]");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка вводу даних лікаря: {e.Message} [{e.GetType().Name}]");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Неочікувана помилка: {e.Message}");
        }
    }
    static void RunAppointmentMenu(Clinic clinic)
    {
        try
        {
            clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0), 45);

            clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 10, 9, 0, 0), -20);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка параметрів запису: {e.Message} [{e.GetType().Name}]");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка запису: {e.Message} [{e.GetType().Name}]");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Неочікувана помилка: {e.Message}");
        }
    }
}