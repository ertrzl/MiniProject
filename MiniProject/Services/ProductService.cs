using MiniProject.Enums;
using MiniProject.Models;
using MiniProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Utilities
{
    public class ProductService

    {
        private List<Product> _products = new List<Product>();
        private List<Order> _orders = new List<Order>();

        private readonly string _productFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Product.json";
        private readonly string _ordersFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Orders.json";

        public ProductService()
        {
            LoadData();
        }

        //private void LoadData()
        //{
        //    _products = Repository.Deserialize<Product>(_productFilePath) ?? new List<Product>();
        //    _orders = Repository.Deserialize<Order>(_ordersFilePath) ?? new List<Order>();
        //}

        //private void SaveData()
        //{
        //    Repository.Serialize(_productFilePath, _products);
        //    Repository.Serialize(_ordersFilePath, _orders);
        //}

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
            SaveData();

            Console.WriteLine("\nProduct successfully created!");
            newProduct.PrintInfo();


        }

        public void DeleteProduct()
        {
            Console.Write("Enter the ID of the product you want to delete: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
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

            _products.Remove(product);
            SaveData();
            Console.WriteLine($"Product '{product.Name}' deleted from memory!");
        }


        public void GetProductById()
        {
            Console.Write("Enter Product Id: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
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
            Console.Clear();
            Console.WriteLine("==================================================================");
            Console.WriteLine(string.Format("| {0,-36} | {1,-10} | {2,-8} |", "PRODUCT ID", "NAME", "STOCK"));
            Console.WriteLine("==================================================================");

            foreach (var product in _products)
            {
                if (product.Stock <= 5)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(string.Format("| {0,-36} | {1,-10} | {2,-8} |", product.Id, product.Name, product.Stock));
                Console.ResetColor();
            }

            Console.WriteLine("==================================================================");
        }
        //public void ShowAllProducts()
        //{
        //    if (_products.Count == 0)
        //    {
        //        Console.WriteLine("No products available.");
        //        return;
        //    }
        //    Console.WriteLine("Available Products:");
        //    foreach (var product in _products)
        //    {
        //        string stockStatus = product.Stock == 0 ? "Out of Stock" : product.Stock.ToString();
        //        product.PrintInfo();
        //    }
        //}
        public void RefillProduct()
        {
            Console.Write("Enter the ID of the product you want to refill: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
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
            SaveData();
            Console.WriteLine($"{amount} units added. New stock for '{product.Name}': {product.Stock}");
        }
        private void LoadData()
        {
            _products = Repository.Deserialize<Product>(_productFilePath) ?? new List<Product>();
        }

        private void SaveData()
        {
            Repository.Serialize(_productFilePath, _products);
        }

        public void CheckLowStockAlert()
        {
            int threshold = 5;
            var lowStockProducts = _products.Where(p => p.Stock <= threshold).ToList();

            if (lowStockProducts.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[INFO] All products have sufficient stock levels.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️  ============= LOW STOCK ALERT =============");
            Console.WriteLine($"The following products have less than {threshold} items left!");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            foreach (var prod in lowStockProducts)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"- Product: {prod.Name} ");
                Console.ResetColor();

                Console.Write($"| ID: {prod.Id} | CURRENT STOCK: ");
                //Console.WriteLine($"| ID: {prod.Id} | CURRENT STOCK: {prod.Stock} left");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{prod.Stock} left");
                Console.ResetColor();
            }

            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("==============================================\n");
            //Console.ResetColor();
        }

    }
}



















