using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task4
    {
        public static void Run()
        {
            int systolic = int.Parse(Console.ReadLine()!);
            int diastolic = int.Parse(Console.ReadLine()!);
            if (systolic < 120 && diastolic < 80)
            {
                Console.WriteLine("normal");
            }
            else if (systolic < 130 && diastolic < 80)
            {
                Console.WriteLine("elevated");
            }
            else if (systolic < 140 || diastolic < 90)
            {
                Console.WriteLine("1st grade of hypertension");
            }
            else
            {
                Console.WriteLine("2st grade of hypertension");
            }
        }
    }
}
