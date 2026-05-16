using MiniProject.Enums;
using MiniProject.Models;
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
        private readonly List<Product> _products = new List<Product>();
        private List<Order> _orders = new List<Order>();

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

        public void OrderProduct()
        {
            string customerEmail;

            while (true)
            {
                customerEmail = Helper.GetEmail();
                bool isOrdering = true;
                Order newOrder = new Order
                {
                    Email = customerEmail,
                    Status = OrderStatus.PENDING,
                    OrderedAt = DateTime.Now
                };

                while (isOrdering)
                {
                    Console.Write("\nEnter product ID (0 to exit): ");
                    string? input2 = Console.ReadLine()?.Trim();
                    if (input2 == "0")
                    {
                        Console.WriteLine("Order cancelled. Returning to menu...");
                        return;
                    }

                    while (true)
                    {
                        Console.WriteLine("Enter Product ID: ");
                        string? input = Console.ReadLine()?.Trim();

                        if (!int.TryParse(input, out int productId))
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric ID.");
                            continue;
                        }

                        if (productId == 0)
                        {
                            Console.WriteLine("Order cancelled. Returning to menu...");
                            return;
                        }

                        Product product = _products.FirstOrDefault(p => p.Id == productId);

                        if (product == null)
                        {
                            Console.WriteLine($"Product With ID {productId} not found.");
                            continue;
                        }

                        Console.Write($"How many '{product.Name}' do you want? (Available: {product.Stock}): ");

                        if (!int.TryParse(Console.ReadLine()?.Trim(), out int count) || count <= 0)
                        {
                            Console.WriteLine("Invalid quantity.");
                            continue;
                        }

                        if (count == 0)
                        {
                            Console.WriteLine("Product selection cancelled.");
                            continue;
                        }

                        if (count > product.Stock)
                        {
                            Console.WriteLine($"Not enough stock. Available: {product.Stock}. Order not placed.");
                            continue;
                        }
                        product.Stock -= count;

                        var item = new OrderItem(product, count);
                        newOrder.Items.Add(item);

                        Console.WriteLine($"'{product.Name}' x{count} added. SubTotal: {item.SubTotal:F2}");

                        Console.Write("Add another product? (y to continue / any key to finish): ");
                        string? cont = Console.ReadLine()?.Trim().ToLower();
                        if (cont != "y")
                        {
                            break;
                        }
                    }

                    if (newOrder.Items.Count == 0)
                    {
                        Console.WriteLine("No items selected. Order was not created.");
                        return;
                    }
                    _orders.Add(newOrder);

                    newOrder.PrintInfo();
                    break;
                }
                break;
            }
        }



        public void ShowAllOrders()
        {
            if (_orders == null || _orders.Count == 0)
            {
                Console.WriteLine("\n--- No orders found in the system. ---");
                return;
            }

            Console.WriteLine("\n================ ALL ORDERS LIST ================");
            foreach (var order in _orders)
            {
                Console.WriteLine($"Order ID: {order.Id} | Customer: {order.Email}");
                Console.WriteLine($"Date: {order.OrderedAt:yyyy-MM-dd HH:mm}");


                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"STATUS: [{order.Status}]");
                Console.ResetColor();

                Console.WriteLine("Items ordered:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.Product.Name} (Quantity: {item.Count}) | SubTotal: {item.SubTotal} AZN");
                }
                Console.WriteLine($"TOTAL AMOUNT: {order.Total} AZN");
                Console.WriteLine("-------------------------------------------------");
            }

            Console.WriteLine("================ END OF ORDERS ==================\n");
        }
    }
}

            

            














