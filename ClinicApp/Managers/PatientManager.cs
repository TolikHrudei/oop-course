using ClinicApp.Enums;
using ClinicApp.Models;
using System;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
        private readonly List<Patient> _patients = new List<Patient>();
        public int Count => _patients.Count;
        public Patient? this[int index]
        {
            get
            {
                if (index < 0 || index >= _patients.Count)
                {
                    return null;
                }
                return _patients[index];
            }
        }

        public Patient? Add(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            _patients.Add(patient);
            return patient;
        }
        public bool Remove(int id)
        {
            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].Id == id)
                {
                    _patients.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public Patient? FindById(int id)
        {
            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].Id == id)
                {
                    return _patients[i];
                }
            }
            return null;
        }

        public Patient[] FindByName(string name)
        {
            List<Patient> result = new List<Patient>();
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToArray();
            }

            string searchName = name.Trim().ToLower();
            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].FullName.ToLower().Contains(searchName))
                {
                    result.Add(_patients[i]);
                }
            }

            return result.ToArray();
        }
        public Patient[] FindByBloodType(BloodType bloodType)
        {
            List<Patient> result = new List<Patient>();
            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].BloodType == bloodType)
                {
                    result.Add(_patients[i]);
                }
            }
            return result.ToArray();
        }
        public Patient[] GetAll()
        {
            return _patients.ToArray();
        }
        public void DisplayAll()
        {
            if (_patients.Count == 0)
            {
                Console.WriteLine("Список пацієнтів порожній.");
                return;
            }
            for (int i = 0; i < _patients.Count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }
    }
}