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
            switch (number)
            {
                case 1: Console.WriteLine("День: Понеділок, 08:00–18:00");
                    break;
                case 2: Console.WriteLine("День: Вівторок, 08:00–18:00");
                    break;
                case 3: Console.WriteLine("День: Середа, 09:00–17:00");
                    break;
                case 4: Console.WriteLine("День: Четвер, 08:00–18:00");
                    break;
                case 5: Console.WriteLine("День: П'тниця, 08:00–16:00");
                    break;
                case 6: Console.WriteLine("День: Cубота, 09:00–14:00");
                    break;
                case 7: Console.WriteLine("День: Неділя, Вихідний");
                    break;
                default: Console.WriteLine("невідомий день");
                    break;
            }
        }
    }
}
