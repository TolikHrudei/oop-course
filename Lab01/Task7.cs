using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    public class Task7
    {
        public static void Run(){
            int n = int.Parse(Console.ReadLine()!);
            decimal[] prices = new decimal[n];
            decimal sum = 0;

            for (int i = 0; i < n; i++)
            {
                prices[i] = decimal.Parse(Console.ReadLine()!);
            }

            decimal min = prices[0];
            decimal max = prices[0];

            foreach (decimal price in prices) 
            {
                if(price < min)
                {
                    min = price;
                }
                if (price > max)
                {
                    max = price;
                }
                sum += price;
            }

            decimal avg = sum / n;
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                if (prices[i] > avg)
                {
                    count++;
                }
            }
            int v = 0;
            int expensive = -1;
            while (v < n)
            {
                if (prices[v] > 1000)
                {
                    expensive = v;
                    break;
                }
                v++;
            }
            Console.WriteLine($"***Звіт по прийомах***\nКількість : {n}\nЗагальна сума : {sum:F2} грн\nСередня : {avg:F2} грн\nМін / Макс : {min:F2} грн / {max:F2} грн\nВище середнього : {count} з {n}\nПерший > 1000 : ");
            if (expensive != -1) 
            {
                Console.WriteLine($"#{expensive + 1} - {prices[expensive]:F2} грн\n");
            }
            else
            {
                Console.WriteLine("немає");
            }
            Console.WriteLine("============================");
        }
    }
}
