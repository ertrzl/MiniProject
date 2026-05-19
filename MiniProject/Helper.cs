using MiniProject.Enums;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniProject
{
    internal class Helper
    {
        public static string GetEmail()
        {
            while (true)
            {
                Console.WriteLine("Enter customer email: ");
                string? email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return email;
                }
                Console.WriteLine("Error: Invalid email. Please try again.");
            }

        }


        public static bool AskToContinue()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Add another product? (y to continue / any key to finish): ");
            string cont = Console.ReadLine()?.Trim().ToLower();
            return cont == "y"; 
        }
        public static void PrintStatusWithColor(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.PENDING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case OrderStatus.CONFIRMED:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case OrderStatus.COMPLETED:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case OrderStatus.CANCELLED:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine($"STATUS: [{status}]");
            Console.ResetColor();
        }
        public static void ShowLoadingAnimation(string message, int durationInSeconds)
        {
            Console.Write($"{message} ");
            char[] counter = { '/', '-', '\\', '|' };
            int loops = durationInSeconds * 4;

            for (int i = 0; i < loops; i++)
            {
                Console.Write(counter[i % 4]);
                Thread.Sleep(250); 
                Console.Write("\b"); 
                Console.WriteLine(" Done! ");
            }

        }

    }
}
  
        
    





