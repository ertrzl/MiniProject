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
        //private List<Order> _orders = new List<Order>();

        private readonly string _productFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Product.json";
        //private readonly string _ordersFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Orders.json";

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
            string name = string.Empty;
            decimal price = 0;
            int stock = 0;

            while (true)
            {
                Console.WriteLine("Enter Product Name (or write 'menu' to return): ");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Product name cannot be empty. Please try again.\n");
                    continue;
                }

                if (input.All(char.IsDigit))
                {
                    Console.WriteLine("Error: Product name cannot be a number. Please try again.\n");
                    continue;
                }

                if (_products.Any(p => p.Name.ToLower() == input.ToLower()))
                {
                    Console.WriteLine("Error: A product with this name already exists. Please try again.\n");
                    continue;
                }

                name = input;
                break;
            }

            while (true)
            {
                Console.Write("Enter Price:");
                string? priceInput = Console.ReadLine()?.Trim();

                if (priceInput?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!decimal.TryParse(priceInput, out decimal inputPrice) || inputPrice <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid positive price! Try again.\n");
                    continue;
                }

                price = inputPrice;
                break;
            }

            while (true)
            {
                Console.Write("Enter Stock:");
                string? stockInput = Console.ReadLine()?.Trim();

                if (stockInput?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!int.TryParse(stockInput, out int inputStock) || inputStock <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid stock amount! Try again.\n");
                    continue;
                }

                stock = inputStock;
                break;
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
            Product productToDelete = null;

            while (true)
            {
                Console.Write("Enter the ID of the product you want to delete: ");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!Guid.TryParse(input, out Guid id))
                {
                    Console.WriteLine("Error: Please enter a valid product Id! Try again.\n");
                    continue;
                }

                Product product = _products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine($"Error: Product with ID {id} not found. Please try again.\n");
                    continue;
                }

                productToDelete = product;
                break;
            }

            _products.Remove(productToDelete);
            SaveData();
            Console.WriteLine($"Product '{productToDelete.Name}' deleted from memory!");
        }


        public void GetProductById()
        {
            Product productToShow = null;

            while (true)
            {
                Console.Write("Enter Product Id: ");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!Guid.TryParse(input, out Guid id))
                {
                    Console.WriteLine("Error: Please enter a valid product Id! Try again.\n");
                    continue;
                }

                Product product = _products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine("Error: Product not found! Please try again.\n");
                    continue;
                }

                productToShow = product;
                break;
            }

            productToShow.PrintInfo();
        }
        public void ShowAllProducts()
        {
            LoadData();
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
            Product productToRefill = null;
            int amount = 0;

            while (true)
            {
                ShowAllProducts();
                Console.Write("\nEnter the ID of the product you want to refill (or '0' to cancel): ");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (input == "0")
                {
                    Console.WriteLine("Operation cancelled. Returning to menu...");
                    return;
                }

                if (!Guid.TryParse(input, out Guid id))
                {
                    Console.WriteLine("Error: Please enter a valid product Id! Try again.\n");
                    Console.ReadLine();
                    continue;
                }

                Product product = _products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine($"Error: Product with ID {id} not found. Please try again.\n");
                    Console.ReadLine();
                    continue;
                }

                productToRefill = product;
                break;
            }

            while (true)
            {
                Console.Write($"Current stock for '{productToRefill.Name}' is {productToRefill.Stock}. Enter the amount to add (or '0' to cancel): ");
                string? inputAmountStr = Console.ReadLine()?.Trim();

                if (inputAmountStr?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!int.TryParse(inputAmountStr, out int inputAmount) || inputAmount <= 0)
                {
                    Console.WriteLine("Error: Refill amount must be a positive number! Please try again.\n");
                    Console.ReadLine();
                    continue;
                }

                amount = inputAmount;
                break;
            }

            productToRefill.Stock += amount;
            SaveData();
            Console.WriteLine($"{amount} units added. New stock for '{productToRefill.Name}': {productToRefill.Stock}");
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
            Console.WriteLine("\n  ============= LOW STOCK ALERT =============");
            Console.WriteLine($"The following products have less than {threshold} items left!");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            foreach (var prod in lowStockProducts)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"- Product: {prod.Name} ");
                Console.ResetColor();

                Console.Write($"| ID: {prod.Id} | CURRENT STOCK: ");
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{prod.Stock} left");
                Console.ResetColor();
            }

            
        }
        private void LoadData()
        {
            _products = Repository.Deserialize<Product>(_productFilePath) ?? new List<Product>();
        }

        private void SaveData()
        {
            Repository.Serialize(_productFilePath, _products);
        }

    }
}



















