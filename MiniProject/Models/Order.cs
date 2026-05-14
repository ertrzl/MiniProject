using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniProject.Enums;

namespace MiniProject.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (value.Contains("@"))
                    Email = value;
                else
                    Console.WriteLine("Error: Email must contain '@'. Try again.");
            }
        }
        public OrderStatus Status { get; set; }
        public DateTime OrderedAt { get; set; }

        public decimal Total => Items.Sum(i => i.SubTotal);

        public void PrintInfo()
        {
            Console.WriteLine($"Order ID: {Id} | Date: {OrderedAt:dd.MM.yyyy HH:mm} | Customer: {Email} | Status: {Status} | Items: {string.Join(", ", Items.Select(i => $"{i.Product.Name} (x{i.Count})"))} | Total: {Total} AZN");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"GRAND TOTAL: {Total} AZN");
            Console.WriteLine("-------------------------------------------");
        }

    }
}

