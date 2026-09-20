using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task2
    {
        public static void Run()
        {
            double price = double.Parse(Console.ReadLine()!);
            int appointment = int.Parse(Console.ReadLine()!);
            int sale = int.Parse(Console.ReadLine()!);

            double sum = price * appointment * (1 - sale / 100.0);

            Console.WriteLine($"Сума: {sum:F2}");
        }
    }
}
