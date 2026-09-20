using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task3
    {
        public static void Run()
        {
            int birth_year = int.Parse(Console.ReadLine()!);
            if (birth_year > 2026 || birth_year < 1906)
            {
                return;
            }
            int age = 2026 - birth_year;
            string age_category;
            if (age <= 17)
            {
                age_category = "дитина";
            }
            else if (age <= 59)
            {
                age_category = "дорослий";
            }
            else
            {
                age_category = "пенсіонер";
            }
            Console.WriteLine($"Вік: {age}");
            Console.WriteLine($"Категорія: {age_category}");
        }
    }
}