using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Text;

namespace ClinicApp
{
    public class DoctorManager
    {
        private const int MaxDoctors = 50;
        private Doctor[] _doctors = new Doctor[MaxDoctors];
        public int _count = 0;
        public int Count => _count;
        public void Add(Doctor doctor)
        {
            if (_count >= MaxDoctors){
                Console.WriteLine("Перевищено ліміт лікарів");
                return;
            }
            _doctors[_count] = doctor;
            _count++;
            Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
        }

        public Doctor[] GetAll()
        {
            Doctor[] copy = new Doctor[MaxDoctors];
            for (int i = 0; i < _count; i++)
            {
                copy[i] = _doctors[i];
            }
            return copy;
        }
        public Doctor? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Id == id)
                {
                    return _doctors[i];
                }
            }
            return null;
        }
        public Doctor[] FindBySpecialty(Speciality spec)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i] != null && _doctors[i].Specialty == spec)
                {
                    matchCount++;
                }
            }
            Doctor[] result = new Doctor[matchCount];
            int resultIndex = 0;
            for (int i = 0; i < _count; i++)
            {
               if (_doctors[i] != null && _doctors[i].Specialty == spec)
               {
                    result[resultIndex] = _doctors[i];
                    resultIndex++;
               }
            }
            return result;
        }
        public bool Remove(int id)
        {
            int foundIndex = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors != null && _doctors[i].Id == id)
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
                _doctors[i] = _doctors[i + 1];
            }
            _doctors[_count - 1] = null!;
            _count--;
            return true;
        }

        public void DisplayAll()
        {
            if (_count == 0)
            {
                Console.WriteLine("Порожній список.");
                return;
            }
            Console.WriteLine($"\n===Лікарі ({_count} / {MaxDoctors}) ===");
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i] != null)
                {
                    Console.WriteLine(_doctors[i].ToString());
                }
            }
            Console.WriteLine(new string('-', 60));
        }

        public void DisplayStats()
        {
            if (_count == 0)
            {
                return;
            }
            int availableCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i] != null && _doctors[i].IsAvailableNow)
                {
                    availableCount++;
                }
            }
            Console.WriteLine("\n=== Статистика лікарів ===");
            Console.WriteLine($"Всього:            {_count}");
            Console.WriteLine($"Доступні зараз:    {availableCount}");
            Console.WriteLine($"По спеціальностям:");
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i] == null) continue;
                Speciality currentSpec = _doctors[i].Specialty;
                bool isDuplicate = false;
                for (int j = 0; j < i; j++)
                {
                    if (_doctors[j] != null && _doctors[j].Specialty == currentSpec)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    int specCount = 0;
                    for (int k = 0; k < _count; k++)
                    {
                        if(_doctors[k] != null && _doctors[k].Specialty == currentSpec)
                        {
                            specCount++;
                        }
                    }
                    Console.WriteLine($"{currentSpec}: {specCount}");
                }
            }
            Console.WriteLine(new string('=', 30));
        }
    }
}
