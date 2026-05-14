using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Utilities
{
    public class ProductService

    {
        private List<Product> _products = new List<Product>();

        public void CreateProduct()
        {
            Console.WriteLine("Enter Product Name");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name)) 
            {
                Console.WriteLine("Error: Product name cannot be empty.");
                return;
            }
            if (name.All(char.IsDigit)) 
            {
                Console.WriteLine("Error: Product name cannot be a number.");
                return;
            }
            if (_products.Any(p => p.Name.ToLower() == name.ToLower()))
            {
                Console.WriteLine("Error: A product with this name already exists.");
                return;
            }

            Console.Write("Enter Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Error: Please enter a valid price!");
                return;
            }
            Console.Write("Enter Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.WriteLine("Error: Please enter a valid stock amount!");
                return;
            }

            Product newProduct = new Product
            {
                Name = name,
                Price = price,
                Stock = stock
            };
            _products.Add(newProduct);

            Console.WriteLine("\nProduct successfully created!");
            newProduct.PrintInfo();


        }

        public void DeleteProduct()
        {
            Console.Write("Enter the ID of the product you want to delete: ");
            if(!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Please enter a valid product Id!");
                return;
            }

            Product product =_products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine($"Error: Product with ID {id} not found.");
                return;
            }

            _products.Remove(product);
            Console.WriteLine($"Product '{product.Name}' deleted from memory!");
        }

        public void GetProductById()
        {
            Console.Write("Enter Product Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Please enter a valid product Id!");
                return;
            }

            Product product = _products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                product.PrintInfo();
            }
            else
            {
                Console.WriteLine("Error: Product not found!");
            }
        }
        public void ShowAllProducts()
        {
            if (_products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }
            Console.WriteLine("Available Products:");
            foreach (var product in _products)
            {
                string stockStatus = product.Stock == 0 ? "Out of Stock" : product.Stock.ToString();
                product.PrintInfo();
            }
        }
        public void RefillProduct()
        {
            Console.Write("Enter the ID of the product you want to refill: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Please enter a valid product Id!");
                return;
            }
            Product product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine($"Error: Product with ID {id} not found.");
                return;
            }
            Console.Write($"Current stock for '{product.Name}' is {product.Stock}. Enter the amount to add: ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Error: Refill amount must be a positive number!");
                return;
            }
            product.Stock += amount;
            Console.WriteLine($"{amount} units added. New stock for '{product.Name}' : {product.Stock}");
            

        }

        

    }
}

    
   

