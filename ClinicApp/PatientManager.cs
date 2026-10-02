using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ClinicApp
{
    public class PatientManager
    {
        private const int MaxPatients = 100;
        private Patient[] _patients = new Patient[MaxPatients];
        private int _count = 0;
        public int Count => _count;
        public Patient? this[int index]
        {
            get
            {
                if(index >= 0 && index < _count)
                {
                    return _patients[index];
                }
                return null;
            }
        }
        public void Add(Patient patient)
        {
            if (_count >= MaxPatients)
            {
                Console.WriteLine("Перевищено ліміт пацієнтів");
                return;
            }
            _patients[_count] = patient;
            _count++;
            Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
        }
        public Patient? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null && _patients[i].Id == id)
                {
                    return _patients[i];
                }
            }
            return null;
        }
        public Patient[] FindByName(string name)
        {
            string search = name.ToLower();
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null)
                {
                    string fName = _patients[i].FirstName.ToLower();
                    string lName = _patients[i].LastName.ToLower();
                    if (fName.Contains(search) || lName.Contains(search))
                    {
                        matchCount++;
                    }
                }

            }
            Patient[] result = new Patient[matchCount];
            int resultIndex = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null)
                {
                    string fName = _patients[i].FirstName.ToLower();
                    string lName = _patients[i].LastName.ToLower();
                    if (fName.Contains(search) || lName.Contains(search))
                    {
                        result[resultIndex] = _patients[i];
                        resultIndex++;
                    }
                }
            }
            return result;

        }
        public bool Remove(int id)
        {
            int foundIndex = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null && _patients[i].Id == id)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex == -1)
            {
                return false;
            }
            for (int i = 0; i < _count - 1; i++)
            {
                _patients[i] = _patients[i + 1];
            }
            _patients[_count - 1] = null!;
            _count--;
            return true;
        }
    
        public void DisplayAll()
        {
            if(_count == 0)
            {
                Console.WriteLine("Порожній список.");
                return;
            }

            Console.WriteLine($"\n====Пацієнти ({_count} / {MaxPatients}) ===");
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null)
                {
                    Console.WriteLine(_patients[i].ToString());
                }
            }
            Console.WriteLine(new string('=', 40));
        }

        public void DisplayStats()
        {
            if( _count == 0)
            {
                Console.WriteLine("Порожній список, статистика недоступна.");
                return;
            }
            double sumAge = 0;
            int minAgeIdx = 0;
            int maxAgeIdx = 0;
            int adultCount = 0;
            for (int i = 0; i < _count; i++) 
            {
                if(_patients[i] != null)
                {
                    int currentAge = _patients[i].Age;
                    sumAge += currentAge;
                    if (_patients[i].IsAdult)
                    {
                        adultCount++;
                    }
                    if(currentAge < _patients[minAgeIdx].Age)
                    {
                        minAgeIdx = i;
                    }
                    if (currentAge > _patients[maxAgeIdx].Age) 
                    {
                        maxAgeIdx = i;
                    }
                }
            }
            double avgAge = sumAge / _count;
            Console.WriteLine("\n=== Статистика пацієнтів ===");
            Console.WriteLine($"Всього:         {_count}");
            Console.WriteLine($"Середній вік:   {avgAge}p.");
            Console.WriteLine($"Наймолодший:    {_patients[minAgeIdx].FullName} ({_patients[minAgeIdx].Age} p.)");
            Console.WriteLine($"Найстарший:     {_patients[maxAgeIdx].FullName} ({_patients[maxAgeIdx].Age} p.)");
            Console.WriteLine($"Дорослих:       {adultCount} з {_count}");
            Console.WriteLine(new string('=', 30));

        }
    }
}
