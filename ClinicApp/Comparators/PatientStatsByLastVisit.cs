using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Comparators
{
    public class PatientStatsByLastVisit : IComparer<PatientStats>
    {
        public int Compare(PatientStats? x, PatientStats? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return y.LastVisitDate.CompareTo(x.LastVisitDate);
        }
    }
}
