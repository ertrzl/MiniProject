using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniProject
{
    public static class Helper
    {
        public static string GetEmail()
        {
            while(true)
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
    }
}
