using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class OrderItem
    {
       
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get { return (int)(Price * Count); } }

        
        public OrderItem(Product product, int count)
        {
            
            Id = Guid.NewGuid();
            Product = product;
            Count = count;
            Price = product.Price;
        }
    }
}
