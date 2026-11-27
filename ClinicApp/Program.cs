using ClinicApp;
using ClinicApp.Comparators;
using ClinicApp.Enums;
using ClinicApp.Events;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;
using System;
using System.Collections.Generic;
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
        clinic.Appointments.AppointmentBooked += OnAppointmentBookedConsole;

        SeedInitialData(clinic);

        if (clinic.Session.Exists())
        {
            Console.Write("Знайдено збережену сесію. Завантажити? (y/n): ");
            string loadAnswer = (Console.ReadLine() ?? "").Trim().ToLower();
            if (loadAnswer == "y")
            {
                int loaded = clinic.Session.Load(clinic);
                Console.WriteLine($"Завантажено {loaded} пацієнтів.");
            }
        }

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║               МЕДИЧНА КЛІНІКА                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Пацієнти        – реєстрація, пошук      ║");
            Console.WriteLine("║  2. Лікарі          – персонал, розклад      ║");
            Console.WriteLine("║  3. Записи          – прийоми, скасування    ║");
            Console.WriteLine("║  4. Медична картка  – діагнози, рецепти      ║");
            Console.WriteLine("║  5. Рахунки         – оплата, борги          ║");
            Console.WriteLine("║  6. Черга           – очікування, прийом     ║");
            Console.WriteLine("║  7. Звіт            – загальна статистика    ║");
            Console.WriteLine("║  8. Аналітика       – лікарі та пацієнти     ║");
            Console.WriteLine("║  9. Плани лікування – рефлексія, атрибути   ║");
            Console.WriteLine("║ 10. Файли           – журнал подій, лог      ║");
            Console.WriteLine("║ 11. Звіти           – LINQ-аналітика        ║");
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
                    RunWaitingRoomMenu(clinic);
                    break;
                case "7":
                    clinic.GenerateReport();
                    break;
                case "8":
                    RunAnalyticsMenu(clinic);
                    break;
                case "9":
                    RunTreatmentPlansMenu(clinic);
                    break;
                case "10":
                    RunFilesMenu(clinic);
                    break;
                case "11":
                    ReportsMenu(clinic);
                    break;
                case "0":
                    clinic.Tracker.PrintSummary();
                    clinic.Tracker.SaveSummary();
                    Console.Write("Зберегти сесію? (y/n): ");
                    string saveAnswer = (Console.ReadLine() ?? "").Trim().ToLower();
                    if (saveAnswer == "y")
                    {
                        clinic.Session.Save(clinic);
                        Console.WriteLine("Сесію збережено.");
                    }
                    running = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void OnAppointmentBookedConsole(object? sender, AppointmentEventArgs e)
    {
        Console.WriteLine($"[EVENT] Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} -> лікар #{e.DoctorId}, {e.ScheduledAt:dd.MM.yyyy HH:mm}");
    }

    static void SeedInitialData(Clinic clinic)
    {
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1992, 8, 20), BloodType.ONegative, "0672345678"));
        clinic.Patients.Add(new Patient("Василь", "Сидоренко", new DateTime(1978, 12, 10), BloodType.BPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(1995, 2, 28), BloodType.APositive, "0631234567"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        d1.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.General, "LIC-002", "0442345678");
        d2.Schedule = new WorkSchedule(9, 17);
        clinic.Doctors.Add(d2);

        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(-2).AddHours(10), 30);
        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(-1).AddHours(10), 30);
        clinic.Appointments.BookUrgent(2, 2, DateTime.Today.AddDays(-1).AddHours(11), "біль у горлі", 30);

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 2, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт", false));
        clinic.MedicalRecords.Add(new LabResult(1, 2, DateTime.Today, "Гемоглобін", 145, "г/л", "120–160", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
        clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-15), "M54.5", "Біль у попереку", false));
        clinic.MedicalRecords.Add(new LabResult(3, 2, DateTime.Today, "Загальний аналіз крові", 5.0, "10^9/л", "4.0–9.0", true));
    }

    static void ReportsMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Звіти ────────────────────────────");
            Console.WriteLine("1. Спеціальності");
            Console.WriteLine("2. Найзавантаженіший лікар");
            Console.WriteLine("3. Пацієнти з кількома візитами");
            Console.WriteLine("4. Топ-3 лікарів за виручкою");
            Console.WriteLine("5. Чи є термінові записи");
            Console.WriteLine("6. Активні спеціальності");
            Console.WriteLine("7. Виручка по місяцях");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowSpecialityStatsReport(clinic);
                    break;
                case "2":
                    ShowBusiestDoctorReport(clinic);
                    break;
                case "3":
                    ShowPatientsWithMultipleVisitsReport(clinic);
                    break;
                case "4":
                    ShowTopEarnersReport(clinic);
                    break;
                case "5":
                    ShowHasUrgentAppointmentsReport(clinic);
                    break;
                case "6":
                    ShowActiveSpecialitiesReport(clinic);
                    break;
                case "7":
                    ShowMonthlyRevenueReport(clinic);
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

    static void ShowSpecialityStatsReport(Clinic clinic)
    {
        List<SpecialityReport> items = new List<SpecialityReport>();
        foreach (var report in clinic.Reports.GetSpecialityStats())
        {
            items.Add(report);
        }

        if (items.Count == 0)
        {
            Console.WriteLine("Даних по спеціальностях немає.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine(items[i]);
        }
    }

    static void ShowBusiestDoctorReport(Clinic clinic)
    {
        string? name = clinic.Reports.FindBusiestDoctorName();
        Console.WriteLine(name == null ? "немає лікарів" : $"Найзавантаженіший лікар: {name}");
    }

    static void ShowPatientsWithMultipleVisitsReport(Clinic clinic)
    {
        Console.Write("Мінімальна кількість візитів: ");
        if (!int.TryParse(Console.ReadLine(), out int minVisits))
        {
            Console.WriteLine("Некоректне число.");
            return;
        }

        List<string> names = new List<string>();
        foreach (var name in clinic.Reports.GetPatientsWithMultipleVisits(minVisits))
        {
            names.Add(name);
        }

        if (names.Count == 0)
        {
            Console.WriteLine($"Пацієнтів із щонайменше {minVisits} візитами немає.");
            return;
        }

        for (int i = 0; i < names.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {names[i]}");
        }
    }

    static void ShowTopEarnersReport(Clinic clinic)
    {
        List<DoctorStats> top = new List<DoctorStats>();
        foreach (var stat in clinic.Reports.GetTopEarners(3))
        {
            top.Add(stat);
        }

        if (top.Count == 0)
        {
            Console.WriteLine("Даних про виручку лікарів немає.");
            return;
        }

        for (int i = 0; i < top.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {top[i].FullName} — {top[i].TotalRevenue:F2} грн");
        }
    }

    static void ShowHasUrgentAppointmentsReport(Clinic clinic)
    {
        bool hasUrgent = clinic.Reports.HasAnyUrgentAppointments();
        Console.WriteLine(hasUrgent ? "Так" : "Ні");
    }

    static void ShowActiveSpecialitiesReport(Clinic clinic)
    {
        List<Speciality> specialities = new List<Speciality>();
        foreach (var spec in clinic.Reports.GetActiveSpecialities())
        {
            specialities.Add(spec);
        }

        if (specialities.Count == 0)
        {
            Console.WriteLine("Активних спеціальностей немає.");
            return;
        }

        for (int i = 0; i < specialities.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ClinicFormatter.FormatSpeciality(specialities[i])}");
        }
    }

    static void ShowMonthlyRevenueReport(Clinic clinic)
    {
        List<(int Year, int Month, decimal Total)> months = new List<(int Year, int Month, decimal Total)>();
        foreach (var item in clinic.Reports.GetMonthlyRevenue())
        {
            months.Add(item);
        }

        if (months.Count == 0)
        {
            Console.WriteLine("Даних про виручку по місяцях немає.");
            return;
        }

        for (int i = 0; i < months.Count; i++)
        {
            var m = months[i];
            Console.WriteLine($"{m.Year}/{m.Month:D2} – {m.Total:F2} грн");
        }
    }

    static void RunFilesMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Файли ────────────────────────────");
            Console.WriteLine("1. Останні рядки лога");
            Console.WriteLine("2. Очистити лог");
            Console.WriteLine("3. Експортувати всі звіти");
            Console.WriteLine("4. Імпорт пацієнтів з CSV");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowLastLogLinesMenu(clinic);
                    break;
                case "2":
                    clinic.Logger.Clear();
                    Console.WriteLine("Лог очищено.");
                    break;
                case "3":
                    ExportAllReportsMenu(clinic);
                    break;
                case "4":
                    ImportPatientsCsvMenu(clinic);
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

    static void ExportAllReportsMenu(Clinic clinic)
    {
        string[] paths = clinic.Exporter.ExportAll();
        Console.WriteLine("Експортовано файли:");
        for (int i = 0; i < paths.Length; i++)
        {
            Console.WriteLine($"  {paths[i]}");
        }
    }

    static void ImportPatientsCsvMenu(Clinic clinic)
    {
        Console.Write("Шлях до CSV-файлу: ");
        string filePath = Console.ReadLine() ?? "";
        ImportResult result = CsvImporter.ImportPatients(clinic, filePath);
        result.Print();
    }

    static void ShowLastLogLinesMenu(Clinic clinic)
    {
        Console.Write("Скільки останніх рядків вивести: ");
        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректна кількість рядків лога.");
            return;
        }

        string[] lines = clinic.Logger.GetLastLines(n);
        if (lines.Length == 0)
        {
            Console.WriteLine("Записів у лозі немає.");
            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            Console.WriteLine(lines[i]);
        }
    }

    static void RunTreatmentPlansMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Плани лікування ─────────────────");
            Console.WriteLine("1. Показати всі плани");
            Console.WriteLine("2. Додати план лікування");
            Console.WriteLine("3. Плани пацієнта");
            Console.WriteLine("4. Активувати план");
            Console.WriteLine("5. Завершити план");
            Console.WriteLine("6. Скасувати план");
            Console.WriteLine("7. Інформація про тип TreatmentPlan");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowAllTreatmentPlans(clinic);
                    break;
                case "2":
                    AddTreatmentPlanMenu(clinic);
                    break;
                case "3":
                    ShowPatientTreatmentPlans(clinic);
                    break;
                case "4":
                    ChangeTreatmentPlanStatus(clinic, "activate");
                    break;
                case "5":
                    CompleteTreatmentPlanMenu(clinic);
                    break;
                case "6":
                    ChangeTreatmentPlanStatus(clinic, "cancel");
                    break;
                case "7":
                    ModelValidator.PrintInfo(typeof(TreatmentPlan));
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

    static void ShowAllTreatmentPlans(Clinic clinic)
    {
        TreatmentPlan[] plans = clinic.TreatmentPlans.GetAll();
        if (plans.Length == 0)
        {
            Console.WriteLine("Планів лікування немає.");
            return;
        }

        for (int i = 0; i < plans.Length; i++)
        {
            PrintTreatmentPlan(plans[i]);
        }
    }

    static void AddTreatmentPlanMenu(Clinic clinic)
    {
        TreatmentPlan plan = FormBuilder.Build<TreatmentPlan>();
        if (clinic.TreatmentPlans.Add(plan))
        {
            Console.WriteLine($"План лікування [{plan.Id}] додано.");
        }
    }

    static void ShowPatientTreatmentPlans(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
            return;
        }

        TreatmentPlan[] plans = clinic.TreatmentPlans.GetByPatient(patientId);
        if (plans.Length == 0)
        {
            Console.WriteLine("Планів для цього пацієнта немає.");
            return;
        }

        for (int i = 0; i < plans.Length; i++)
        {
            PrintTreatmentPlan(plans[i]);
        }
    }

    static void CompleteTreatmentPlanMenu(Clinic clinic)
    {
        Console.Write("ID плану: ");
        if (!int.TryParse(Console.ReadLine(), out int planId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID плану.");
            return;
        }

        TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
        if (plan == null)
        {
            Console.WriteLine("План не знайдено");
            return;
        }

        if (clinic.TreatmentPlans.Complete(planId))
        {
            Console.WriteLine($"План [{plan.Id}] завершено.");
        }
        else
        {
            Console.WriteLine($"Неможливо завершити план зі статусом {plan.Status}.");
        }
    }

    static void ChangeTreatmentPlanStatus(Clinic clinic, string action)
    {
        Console.Write("ID плану: ");
        if (!int.TryParse(Console.ReadLine(), out int planId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID плану.");
            return;
        }

        TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
        if (plan == null)
        {
            Console.WriteLine("План не знайдено");
            return;
        }

        bool changed = false;
        string successMessage = "";
        string failMessage = "";

        if (action == "activate")
        {
            changed = plan.Activate();
            successMessage = $"План [{plan.Id}] активовано.";
            failMessage = $"Неможливо активувати план зі статусом {plan.Status}.";
        }
        else if (action == "cancel")
        {
            changed = plan.Cancel();
            successMessage = $"План [{plan.Id}] скасовано.";
            failMessage = $"Неможливо скасувати план зі статусом {plan.Status}.";
        }

        Console.WriteLine(changed ? successMessage : failMessage);
    }

    static void PrintTreatmentPlan(TreatmentPlan plan)
    {
        Console.WriteLine($"[{plan.Id}] Пацієнт #{plan.PatientId} | Діагноз: {plan.Diagnosis} | Тривалість: {plan.DurationDays} дн. | Статус: {plan.Status}");
    }

    static void RunAnalyticsMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Аналітика ─────────────────────────");
            Console.WriteLine("1. Лікарі за навантаженням");
            Console.WriteLine("2. Лікарі за виручкою");
            Console.WriteLine("3. Лікарі за іменем");
            Console.WriteLine("4. Пацієнти за кількістю візитів");
            Console.WriteLine("5. Пацієнти за витратами");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    var docs1 = CollectDoctorStats(clinic);
                    docs1.Sort();
                    DisplayList(docs1);
                    break;
                case "2":
                    var docs2 = CollectDoctorStats(clinic);
                    docs2.Sort(new DoctorStatsByRevenue());
                    DisplayList(docs2);
                    break;
                case "3":
                    var docs3 = CollectDoctorStats(clinic);
                    docs3.Sort(new DoctorStatsByName());
                    DisplayList(docs3);
                    break;
                case "4":
                    var pats1 = CollectPatientStats(clinic);
                    pats1.Sort();
                    DisplayList(pats1);
                    break;
                case "5":
                    var pats2 = CollectPatientStats(clinic);
                    pats2.Sort(new PatientStatsBySpent());
                    DisplayList(pats2);
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

    static List<DoctorStats> CollectDoctorStats(Clinic clinic)
    {
        List<DoctorStats> list = new List<DoctorStats>();
        foreach (var stat in clinic.Analytics.ComputeDoctorStats())
        {
            list.Add(stat);
        }
        return list;
    }

    static List<PatientStats> CollectPatientStats(Clinic clinic)
    {
        List<PatientStats> list = new List<PatientStats>();
        foreach (var stat in clinic.Analytics.ComputePatientStats())
        {
            list.Add(stat);
        }
        return list;
    }

    static void DisplayList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i]);
        }
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID лікаря.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний формат дати.");
            return;
        }

        Console.Write("Скільки слотів: ");
        if (!int.TryParse(Console.ReadLine(), out int slotCount))
        {
            clinic.Logger.LogWarning("Помилка введення: некоректна кількість слотів.");
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
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
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
            Console.WriteLine("9 — За типом прийому");
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
                case "9":
                    ShowAppointmentsByTypeMenu(clinic);
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

    static void ShowAppointmentsByTypeMenu(Clinic clinic)
    {
        Console.WriteLine("1. Термінові");
        Console.WriteLine("2. Консультації спеціаліста");
        Console.WriteLine("3. Звичайні");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine() ?? "";
        Appointment[] result = choice switch
        {
            "1" => clinic.Appointments.GetUrgent(),
            "2" => clinic.Appointments.GetSpecialist(),
            "3" => clinic.Appointments.GetRegular(),
            _ => new Appointment[0]
        };

        if (choice != "1" && choice != "2" && choice != "3")
        {
            Console.WriteLine("Невідома команда.");
            return;
        }

        clinic.Appointments.DisplayAppointments(result);
    }

    static void CancelAllPatientAppointmentsMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
            return;
        }

        Console.Write("Причина (Enter — без причини): ");
        string reason = Console.ReadLine() ?? "";

        int cancelled = clinic.Appointments.CancelAllForPatient(patientId, reason);
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID лікаря.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректне значення хронічності.");
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
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
    }

    static void AddLabResultMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID лікаря.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректне числове значення аналізу.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректне значення норми.");
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
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
    }

    static void AddPrescriptionMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID лікаря.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректна кількість днів.");
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
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
        }
    }

    static void ShowDoctorRecordsMenu(Clinic clinic)
    {
        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Некоректне число.");
            clinic.Logger.LogWarning("Помилка введення: некоректний ID лікаря.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
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
            clinic.Logger.LogWarning("Помилка введення: некоректний ID запису.");
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

    static void RunWaitingRoomMenu(Clinic clinic)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.WriteLine("\n— Черга ─────────────────────────");
            Console.WriteLine("1. Додати пацієнта до черги");
            Console.WriteLine("2. Прийняти першого");
            Console.WriteLine("3. Хто перший?");
            Console.WriteLine("4. Переглянути всю чергу");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    AddPatientToQueueMenu(clinic);
                    break;
                case "2":
                    AcceptFirstPatientMenu(clinic);
                    break;
                case "3":
                    ShowFirstPatientMenu(clinic);
                    break;
                case "4":
                    ShowFullQueueMenu(clinic);
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

    static void AddPatientToQueueMenu(Clinic clinic)
    {
        Console.Write("ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            clinic.Logger.LogWarning("Помилка введення: некоректний ID пацієнта.");
            return;
        }

        Patient? patient = clinic.Patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine("Пацієнта не знайдено");
            return;
        }

        clinic.WaitingRoom.Enqueue(patient);
        Console.WriteLine($"Пацієнта {patient.FullName} додано до черги.");
    }

    static void AcceptFirstPatientMenu(Clinic clinic)
    {
        try
        {
            Patient p = clinic.WaitingRoom.Dequeue();
            Console.WriteLine($"Прийнято: {p.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {ex.Message}");
        }
    }

    static void ShowFirstPatientMenu(Clinic clinic)
    {
        try
        {
            Patient p = clinic.WaitingRoom.Peek();
            Console.WriteLine($"Перший у черзі: {p.FullName}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            clinic.Logger.LogWarning($"Помилка введення: {ex.Message}");
        }
    }

    static void ShowFullQueueMenu(Clinic clinic)
    {
        Patient[] queue = clinic.WaitingRoom.ToArray();
        if (queue.Length == 0)
        {
            Console.WriteLine("Черга порожня.");
            return;
        }

        for (int i = 0; i < queue.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {queue[i].FullName}");
        }
    }
}