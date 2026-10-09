using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class GrowablePatientManager
    {
        private Patient[] _patients = new Patient[4];
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _patients.Length;
        private void Grow()
        {
            int oldCapacity = _patients.Length;
            int newCapacity = oldCapacity * 2;

            Console.WriteLine($"Масив заповнений! Розширення: {oldCapacity} -> {newCapacity}");

            Patient[] newArray = new Patient[newCapacity];
            for (int i = 0; i < _count; i++)
            {
                newArray[i] = _patients[i];
            }

            _patients = newArray;
        }

        public void Add(Patient patient)
        {
            if (_count == _patients.Length)
            {
                Grow();
            }

            _patients[_count] = patient;
            _count++;
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

            if (foundIndex == -1) return false;
            for (int i = foundIndex; i < _count - 1; i++)
            {
                _patients[i] = _patients[i + 1];
            }
            _patients[_count - 1] = null!;
            _count--;

            return true;
        }

        public void DisplayAll()
        {
            Console.WriteLine($"\n=== Пацієнти ({_count} / {Capacity}) ===");
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i] != null)
                {
                    Console.WriteLine(_patients[i].ToString());
                }
            }
        }
    }
}
