using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class Product
    {
        private static int _count { get; set; }
        public int Id { get;  }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string lower = value.ToLower();
                    _name = char.ToUpper(lower[0]) + lower.Substring(1);
                }
                else
                {
                    _name = value;
                }
            }
        }
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public Product()
        {
            _count++;
            Id= _count;
        }
        public void PrintInfo()
        {
            Console.WriteLine($"[{Id}] {_name} - {Price} AZN (Stock: {Stock})");
        }

    }
}
