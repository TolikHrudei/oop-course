using ClinicApp;
using System;

class Program
{
    static void Main()
    {
        Patient[] patients = new Patient[5];
        int _count = 0;
        patients[_count] = new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "А+", "0501234567");
        _count++;
        patients[_count] = new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678");
        _count++;
        patients[_count] = new Patient("Максим", "Бойко", new DateTime(1993, 8, 20), "B-", "0672345678");
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
    }
}