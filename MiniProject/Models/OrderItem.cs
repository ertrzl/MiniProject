using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class OrderItem
    {
        private static int _count { get; set; }
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get { return (int)(Price * Count); } }

        public OrderItem()
        {
            _count++;
            Id = _count;
        }
    }
}
