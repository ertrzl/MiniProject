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
            Console.WriteLine($"{amount} units added. New stock for '{product.Name}': {product.Stock}");
        }

        public void OrderProduct()
        {
            string customerEmail;

            while (true)
            {
                customerEmail = Helper.GetEmail();
                Order newOrder = new Order
                {
                    Email = customerEmail,
                    Status = OrderStatus.PENDING,
                    OrderedAt = DateTime.Now
                };

                bool isOrdering = true;
                while (isOrdering)
                {
                    Console.WriteLine("Enter Product ID: ");
                    string? input = Console.ReadLine()?.Trim();

                    if (input == "0")
                    {
                        Console.WriteLine("Order cancelled. Returning to menu...");
                        foreach (var orderItem in newOrder.Items)
                        {
                            orderItem.Product.Stock += orderItem.Count;
                        }

                        return;
                    }

                    if (!Guid.TryParse(input, out Guid productId))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid GUID ID.");
                        continue;
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

                    if (count > product.Stock)
                    {
                        Console.WriteLine($"Not enough stock. Available: {product.Stock}. Order not placed.");
                        continue;
                    }

                    product.Stock -= count;

                    var item = new OrderItem(product, count);
                    newOrder.Items.Add(item);

                    Console.WriteLine($"'{product.Name}' x{count} added. SubTotal: {item.SubTotal:F2}");

                    if (!Helper.AskToContinue())
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
                Console.WriteLine("\nOrder placed successfully!");
                newOrder.PrintInfo();
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

                Helper.PrintStatusWithColor(order.Status);

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

        public void ChangeOrderStatus()
        {
            Console.Write("Enter Order Id: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Error: Please enter a valid order Id!");
                return;
            }

            Order order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                Console.WriteLine($"Error: Order with ID {id} not found.");
                return;
            }

            Console.Write("Current");
            Helper.PrintStatusWithColor(order.Status);

            Console.WriteLine("Select New Status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Confirmed");
            Console.WriteLine("3. Completed");
            Console.Write("Enter choice (1-3): ");

            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    order.Status = OrderStatus.PENDING;
                    break;
                case "2":
                    order.Status = OrderStatus.CONFIRMED;
                    break;
                case "3":
                    order.Status = OrderStatus.COMPLETED;
                    break;
                default:
                    Console.WriteLine("Error: Invalid selection. Status not changed.");
                    return;
            }

            Console.WriteLine($"\nOrder status successfully updated to [{order.Status}]!");
        }
        public void ShowCustomerOrders()
        {
            string customerEmail = Helper.GetEmail();
            var customerOrders = _orders.Where(o => o.Email.ToLower() == customerEmail.ToLower()).ToList();

            if (customerOrders.Count == 0)
            {
                Console.WriteLine($"\n--- No orders found for customer: {customerEmail} ---");
                return;
            }

            Console.WriteLine($"\n================ ORDERS FOR: {customerEmail} ================");
            foreach (var order in customerOrders)
            {
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Date: {order.OrderedAt:yyyy-MM-dd HH:mm}");

                Helper.PrintStatusWithColor(order.Status);

                Console.WriteLine("Items ordered:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.Product.Name} (Quantity: {item.Count}) | SubTotal: {item.SubTotal} AZN");
                }
                Console.WriteLine($"TOTAL AMOUNT: {order.Total} AZN");
                Console.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine("=================== END OF LIST ===================\n");
        }
    }

}

            

            














