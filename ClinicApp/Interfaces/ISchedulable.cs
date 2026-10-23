using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Interfaces
{
    public interface ISchedulable
    {
        bool CanSchedule(DateTime at);
        DateTime[] GetAvailableSlots(DateTime date, int slotCount);
    }
}