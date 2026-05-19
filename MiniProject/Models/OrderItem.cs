using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } 
        public string ProductName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal => Price * Count;

        public OrderItem()
        {
        }

        public OrderItem(Product product, int count)
        {
            Id = Guid.NewGuid();
            ProductId = product.Id; 
            ProductName = product.Name;
            Count = count;
            Price = product.Price;
        }
    }
}
