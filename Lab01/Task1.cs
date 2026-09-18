using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task1
    {
        public static void Run()
        {
            double weight = double.Parse(Console.ReadLine()!);
            double height = double.Parse(Console.ReadLine()!);

            double bmi = weight / (height * height);

            Console.WriteLine($"IMT: {bmi:F2}");
        }
    }
}
