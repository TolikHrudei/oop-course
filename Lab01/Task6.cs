using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task6
    {
        public static void Run()
        {
            int card = int.Parse(Console.ReadLine()!);
            int last_digit = card % 10;
            string department = last_digit switch
            {
                0 or 1 => "відділення: загальна терапія",
                2 or 3 => "відділення: хірургія",
                4 or 5 => "відділення: кардіологія",
                6 or 7 => "відділення: неврологія",
                8 or 9 => "відділення: офтальмологія",
                _ => "відділення: невідоме"
            };
            string privilege = (card % 2 == 0) ? "пільгова картка: так" : "пільгова категорія: ні";
            string regularity = (card % 3 == 0) ? "черговий огляд: так" : "черговий огляд: ні";

            Console.WriteLine(department);
            Console.WriteLine(privilege);
            Console.WriteLine(regularity);
        }
    }
}
