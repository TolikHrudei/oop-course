using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task5
    {
        public static void Run()
        {
            int number = int.Parse(Console.ReadLine()!);
            string result = number switch
            {
                1 => "День: Понеділок, 08:00–18:00",
                2 => "День: Вівторок, 08:00–18:00",
                3 => "День: Середа, 09:00–17:00",
                4 => "День: Четвер, 08:00–18:00",
                5 => "День: П'тниця, 08:00–16:00",
                6 => "День: Cубота, 09:00–14:00",
                7 => "День: Неділя, Вихідний",
                _ => "День: невідомий день"
            };
            Console.WriteLine(result);
        }
    }
}
