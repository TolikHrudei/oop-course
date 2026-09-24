using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Patient[] patients = new Patient[5];
        int _count = 0;
        patients[_count] = new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "А+", "0501234567");
        _count++;
        patients[_count] = new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678");
        _count++;
        patients[_count] = new Patient("Максим", "Бойко", new DateTime(2010, 1, 4), "B-", "0672345678");
        _count++;
        patients[_count] = new Patient();
        _count++;
        patients[_count] = new Patient("Марія", "Ткач");
        _count++;

        for (int i = 0; i < patients.Length; i++) 
        {
            Patient? currentPatient = patients[i];
            if(currentPatient != null)
            {
                Console.WriteLine(currentPatient.ToString());
            }
        }
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
    }
}