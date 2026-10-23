using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using System;
using System.Globalization;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Clinic clinic = new Clinic("Медична Клініка");

        SeedInitialData(clinic);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║               МЕДИЧНА КЛІНІКА                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Пацієнти       – реєстрація, пошук       ║");
            Console.WriteLine("║  2. Лікарі         – персонал, розклад       ║");
            Console.WriteLine("║  3. Записи         – прийоми, скасування     ║");
            Console.WriteLine("║  4. Медична картка – діагнози, рецепти       ║");
            Console.WriteLine("║  5. Рахунки        – оплата, борги           ║");
            Console.WriteLine("║  6. Звіт           – загальна статистика     ║");
            Console.WriteLine("║  0. Вийти                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("Оберіть розділ: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    RunPatientsMenu(clinic);
                    break;
                case "2":
                    RunDoctorsMenu(clinic);
                    break;
                case "3":
                    RunAppointmentsMenu(clinic);
                    break;
                case "4":
                    RunMedicalRecordsMenu(clinic);
                    break;
                case "5":
                    RunBillingMenu(clinic);
                    break;
                case "6":
                    clinic.GenerateReport();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void SeedInitialData(Clinic clinic)
    {
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1992, 8, 20), BloodType.ONegative, "0672345678"));
        clinic.Patients.Add(new Patient("Василь", "Сидоренко", new DateTime(1978, 12, 10), BloodType.BPositive, "0933456789"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        d1.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Марія", "Іванова", Speciality.General, "LIC-002", "0442345678");
        d2.Schedule = new WorkSchedule(9, 17);
        clinic.Doctors.Add(d2);

        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(1).AddHours(10), 30);
        clinic.Appointments.Book(1, 2, DateTime.Today.AddDays(2).AddHours(11), 45);
        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(3).AddHours(14), 20);

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 2, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт", false));
        clinic.MedicalRecords.Add(new LabResult(1, 2, DateTime.Today, "Гемоглобін", 145, "г/л", "120–160", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));

        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
        clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-15), "M54.5", "Біль у попереку", false));

        clinic.MedicalRecords.Add(new LabResult(3, 2, DateTime.Today, "Загальний аналіз крові", 5.0, "10^9/л", "4.0–9.0", true));
    }

    static void RunPatientsMenu(Clinic clinic)
    {
        Console.WriteLine("\n--- Пацієнти ---");
        clinic.Patients.DisplayAll();
    }

    static void RunDoctorsMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n--- Лікарі ---");
            Console.WriteLine("1 — Список лікарів");
            Console.WriteLine("5 — Вільні години лікаря");
            Console.WriteLine("0 — Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    clinic.Doctors.DisplayAll();
                    break;
                case "5":
                    ShowDoctorAvailableSlotsMenu(clinic);
                    break;
                case "0":
                    inMenu = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void ShowDoctorAvailableSlotsMenu(Clinic clinic)
    {
        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            return;
        }

        Doctor? doctor = clinic.Doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine("Лікаря не знайдено");
            return;
        }

        ISchedulable schedulable = doctor;

        Console.Write("Дата (dd.MM.yyyy): ");
        string dateStr = Console.ReadLine() ?? "";
        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Некоректний формат дати.");
            return;
        }

        Console.Write("Скільки слотів: ");
        if (!int.TryParse(Console.ReadLine(), out int slotCount))
        {
            return;
        }

        try
        {
            DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
            for (int i = 0; i < slots.Length; i++)
            {
                Console.WriteLine(slots[i].ToString("HH:mm"));
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void RunAppointmentsMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n--- Записи ---");
            Console.WriteLine("1 — Розклад на сьогодні");
            Console.WriteLine("8 — Скасувати всі записи пацієнта");
            Console.WriteLine("0 — Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    clinic.DisplaySchedule(DateTime.Today);
                    break;
                case "8":
                    CancelAllPatientAppointmentsMenu(clinic);
                    break;
                case "0":
                    inMenu = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void CancelAllPatientAppointmentsMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            return;
        }

        Console.Write("Причина (Enter — без причини): ");
        string reason = Console.ReadLine() ?? "";

        Appointment[] records = clinic.Appointments.GetByPatient(patientId);
        int cancelled = AppointmentManager.CancelAll(records, reason);

        Console.WriteLine($"Скасовано записів: {cancelled}");
    }

    static void RunMedicalRecordsMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n--- Медична картка ---");
            Console.WriteLine("1 — Картка пацієнта");
            Console.WriteLine("2 — Усі записи пацієнта");
            Console.WriteLine("3 — Додати діагноз");
            Console.WriteLine("4 — Додати аналіз");
            Console.WriteLine("5 — Додати рецепт");
            Console.WriteLine("6 — Записи лікаря");
            Console.WriteLine("0 — Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowPatientSummaryMenu(clinic);
                    break;
                case "2":
                    ShowAllPatientRecordsMenu(clinic);
                    break;
                case "3":
                    AddDiagnosisMenu(clinic);
                    break;
                case "4":
                    AddLabResultMenu(clinic);
                    break;
                case "5":
                    AddPrescriptionMenu(clinic);
                    break;
                case "6":
                    ShowDoctorRecordsMenu(clinic);
                    break;
                case "0":
                    inMenu = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void ShowPatientSummaryMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Пацієнта з таким ID не знайдено.");
            return;
        }

        clinic.MedicalRecords.DisplayPatientSummary(patientId);
    }

    static void ShowAllPatientRecordsMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Пацієнта з таким ID не знайдено.");
            return;
        }

        var records = clinic.MedicalRecords.GetByPatient(patientId);
        clinic.MedicalRecords.DisplayList(records);
    }

    static void AddDiagnosisMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Пацієнта з таким ID не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Doctors.FindById(doctorId) == null)
        {
            Console.WriteLine("Лікаря з таким ID не знайдено.");
            return;
        }

        Console.Write("Код діагнозу (напр. J06.9): ");
        string code = Console.ReadLine() ?? "";

        Console.Write("Опис: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Хронічне? (1=так, 0=ні): ");
        if (!int.TryParse(Console.ReadLine(), out int chronicVal))
        {
            Console.WriteLine("Некоректне значення.");
            return;
        }

        bool isChronic = (chronicVal == 1);

        try
        {
            Diagnosis d = new Diagnosis(patientId, doctorId, DateTime.Today, code, description, isChronic);
            clinic.MedicalRecords.Add(d);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void AddLabResultMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Пацієнта з таким ID не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Doctors.FindById(doctorId) == null)
        {
            Console.WriteLine("Лікаря з таким ID не знайдено.");
            return;
        }

        Console.Write("Назва аналізу: ");
        string testName = Console.ReadLine() ?? "";

        Console.Write("Значення (число): ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        Console.Write("Одиниці виміру: ");
        string unit = Console.ReadLine() ?? "";

        Console.Write("Норма (напр. 4.0–9.0): ");
        string referenceRange = Console.ReadLine() ?? "";

        Console.Write("В нормі? (1=так, 0=ні): ");
        if (!int.TryParse(Console.ReadLine(), out int normVal))
        {
            Console.WriteLine("Некоректне значення.");
            return;
        }

        bool isNormal = (normVal == 1);

        try
        {
            LabResult lr = new LabResult(patientId, doctorId, DateTime.Today, testName, val, unit, referenceRange, isNormal);
            clinic.MedicalRecords.Add(lr);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void AddPrescriptionMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Пацієнта з таким ID не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Doctors.FindById(doctorId) == null)
        {
            Console.WriteLine("Лікаря з таким ID не знайдено.");
            return;
        }

        Console.Write("Препарат: ");
        string medicationName = Console.ReadLine() ?? "";

        Console.Write("Дозування (10 мг): ");
        string dosage = Console.ReadLine() ?? "";

        Console.Write("Кількість днів (ціле): ");
        if (!int.TryParse(Console.ReadLine(), out int durationDays))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        Console.Write("Інструкція (Enter — пропустити): ");
        string instructions = Console.ReadLine() ?? "";

        try
        {
            Prescription rx = new Prescription(patientId, doctorId, DateTime.Today, medicationName, dosage, durationDays, instructions);
            clinic.MedicalRecords.Add(rx);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void ShowDoctorRecordsMenu(Clinic clinic)
    {
        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        if (clinic.Doctors.FindById(doctorId) == null)
        {
            Console.WriteLine("Лікаря з таким ID не знайдено.");
            return;
        }

        var records = clinic.MedicalRecords.GetByDoctor(doctorId);
        clinic.MedicalRecords.DisplayList(records);
    }

    static void RunBillingMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Рахунки ─────────────────────────");
            Console.WriteLine("1. Борги пацієнта");
            Console.WriteLine("2. Всі неоплачені записи");
            Console.WriteLine("3. Оплатити запис");
            Console.WriteLine("4. Загальний борг клініки");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowPatientDebtMenu(clinic);
                    break;
                case "2":
                    ShowAllUnpaidMenu(clinic);
                    break;
                case "3":
                    PayAppointmentMenu(clinic);
                    break;
                case "4":
                    ShowTotalDebtMenu(clinic);
                    break;
                case "0":
                    inMenu = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void ShowPatientDebtMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            return;
        }

        IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
        decimal debt = clinic.Billing.GetPatientDebt(patientId);

        if (unpaid.Length > 0)
        {
            Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
        }

        clinic.Billing.DisplayUnpaid(unpaid);
        Console.WriteLine($"Борг: {debt:F2} грн");
    }

    static void ShowAllUnpaidMenu(Clinic clinic)
    {
        IPayable[] unpaid = clinic.Billing.GetAllUnpaid();
        clinic.Billing.DisplayUnpaid(unpaid);
    }

    static void PayAppointmentMenu(Clinic clinic)
    {
        Console.Write("ID запису для оплати: ");
        if (!int.TryParse(Console.ReadLine(), out int appointmentId))
        {
            return;
        }

        bool success = clinic.Billing.PayAppointment(appointmentId);
        if (success)
        {
            Console.WriteLine($"Запис [{appointmentId}] оплачено.");
        }
        else
        {
            Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
        }
    }

    static void ShowTotalDebtMenu(Clinic clinic)
    {
        decimal totalDebt = clinic.Billing.GetTotalDebt();
        Console.WriteLine($"Загальний борг по клініці: {totalDebt:F2} грн");
    }
}