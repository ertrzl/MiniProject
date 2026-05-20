using MiniProject.Enums;
using MiniProject.Models;
using MiniProject.Repositories;
using MiniProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class OrderService
    {
        private List<Product> _products = new List<Product>();
        private List<Order> _orders = new List<Order>();

        private readonly string _productFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Product.json";
        private readonly string _ordersFilePath = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Orders.json";

        //private void LoadData()
        //{
        //    _orders = Repository.Deserialize<Order>(_ordersFilePath) ?? new List<Order>();
        //}

        //private void SaveData()
        //{
        //    Repository.Serialize(_ordersFilePath, _orders);
        //}
        public OrderService()
        {
            LoadData();
        }

        public void OrderProduct()
        {
            LoadData();
            string customerEmail;

            while (true)
            {
                ProductService productService = new ProductService();
                productService.ShowAllProducts();
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

                    if (input?.ToLower() == "menu")
                    {
                        Console.WriteLine("Returning to menu...");
                        return;
                    }

                    if (input == "0")
                    {
                        Console.WriteLine("Order cancelled. Returning to menu...");
                        foreach (var orderItem in newOrder.Items)
                        {
                            var prod = _products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                            if (prod != null)
                            {
                                prod.Stock += orderItem.Count;
                            }
                        }
                        SaveData();
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
                        Console.WriteLine($"Product with ID {productId} not found.");
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

                    var existingItem = newOrder.Items.FirstOrDefault(item => item.ProductId == product.Id);

                    if (existingItem != null)
                    {
                        existingItem.Count += count;
                        Console.WriteLine($"Updated '{existingItem.ProductName}' total quantity to x{existingItem.Count}. SubTotal: {existingItem.SubTotal:F2}");
                    }
                    else
                    {
                        OrderItem item = new OrderItem(product, count);
                        newOrder.Items.Add(item);
                        Console.WriteLine($"'{item.ProductName}' x{count} added. SubTotal: {item.SubTotal:F2}");
                    }

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
                SaveData();
                Console.Beep(800, 300);
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
                    Console.WriteLine($"  - {item.ProductName} (Quantity: {item.Count}) | SubTotal: {item.SubTotal} AZN");
                }
                Console.WriteLine($"TOTAL AMOUNT: {order.Total} AZN");
                Console.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine("================ END OF ORDERS ==================\n");
        }

        public void ChangeOrderStatus()
        {
            Console.WriteLine("Enter Order ID: ");
            string? input = Console.ReadLine()?.Trim();

            if (!Guid.TryParse(input, out Guid orderId))
            {
                Console.WriteLine("Invalid input. Please enter a valid GUID ID.");
                return;
            }

            Order order = _orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                Console.WriteLine($"Order with ID {orderId} not found.");
                return;
            }

            Console.WriteLine("Select new status: 1. Confirmed, 2. Completed, 3. Cancelled");
            string statusInput = Console.ReadLine();

            switch (statusInput)
            {
                case "1":
                    if (order.Status == OrderStatus.CANCELLED || order.Status == OrderStatus.COMPLETED)
                    {
                        Console.WriteLine("Cannot confirm an order that is already completed or cancelled!");
                        return;
                    }
                    order.Status = OrderStatus.CONFIRMED;
                    Console.WriteLine("Order status updated to CONFIRMED.");
                    break;

                case "2":
                    if (order.Status == OrderStatus.CANCELLED)
                    {
                        Console.WriteLine("Cannot complete a cancelled order!");
                        return;
                    }
                    order.Status = OrderStatus.COMPLETED;
                    Console.WriteLine("Order status updated to COMPLETED.");
                    break;

                case "3":
                    if (order.Status == OrderStatus.CANCELLED)
                    {
                        Console.WriteLine("This order is already cancelled!");
                        return;
                    }
                    if (order.Status == OrderStatus.COMPLETED)
                    {
                        Console.WriteLine("Cannot cancel an order that has already been COMPLETED!");
                        return;
                    }

                    order.Status = OrderStatus.CANCELLED;

                    foreach (var orderItem in order.Items)
                    {
                        var prod = _products.FirstOrDefault(p => p.Id.ToString().ToLower() == orderItem.ProductId.ToString().ToLower());
                        if (prod != null)
                        {
                            prod.Stock += orderItem.Count;
                        }
                    }
                    Console.WriteLine("Order CANCELLED successfully. Items returned to stock.");
                    break;

                default:
                    Console.WriteLine("Invalid selection. Status not changed.");
                    return;
            }

            SaveData();
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
                    Console.WriteLine($"  - {item.ProductName} (Quantity: {item.Count}) | SubTotal: {item.SubTotal} AZN");
                }
                Console.WriteLine($"TOTAL AMOUNT: {order.Total} AZN");
                Console.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine("=================== END OF LIST ===================\n");
        }

        public void ShowBestSellers()
        {
            if (_orders == null || _orders.Count == 0)
            {
                Console.WriteLine("\n--- No sales data available yet. ---");
                return;
            }

            var bestSellers = _orders
                .Where(o => o.Status == OrderStatus.CONFIRMED || o.Status == OrderStatus.COMPLETED)
                .SelectMany(o => o.Items)
                .GroupBy(item => item.ProductName)
                .Select(group => new
                {
                    ProductName = group.Key,
                    TotalQuantity = group.Sum(item => item.Count),
                    TotalRevenue = group.Sum(item => item.SubTotal)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(3)
                .ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n ============= TOP 3 BEST SELLING PRODUCTS =============");
            Console.ResetColor();

            int rank = 1;
            foreach (var product in bestSellers)
            {
                Console.WriteLine($"{rank}. {product.ProductName} | Total Sold: {product.TotalQuantity} units | Total Revenue: {product.TotalRevenue:F2} AZN");
                rank++;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===========================================================\n");
            Console.ResetColor();
        }
        public void ShowFinancialReport()
        {
            var successfulOrders = _orders
                .Where(o => o.Status == OrderStatus.CONFIRMED || o.Status == OrderStatus.COMPLETED).ToList();  

            if (successfulOrders.Count == 0)
            {
                Console.WriteLine("\n--- No financial data available yet. ---");
                return;
            }

            int totalSalesCount = successfulOrders.Count;
            decimal totalRevenue = successfulOrders.Sum(o => o.Total);
            decimal averageOrderValue = successfulOrders.Average(o => o.Total);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n ============= FINANCIAL REPORT =============");
            Console.ResetColor();

            Console.WriteLine($"Total Successful Sales : {totalSalesCount} orders");
            Console.WriteLine($"Total Gross Revenue    : {totalRevenue:F2} AZN");
            Console.WriteLine($"Average Order Value    : {averageOrderValue:F2} AZN per order");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================================\n");
            Console.ResetColor();
        }

        private void LoadData()
        {
            _orders = Repository.Deserialize<Order>(_ordersFilePath) ?? new List<Order>();
            _products = Repository.Deserialize<Product>(_productFilePath) ?? new List<Product>();
        }
        

        private void SaveData()
        {
            Helper.ShowLoadingAnimation("Processing order and updating stock...",2);
            Repository.Serialize(_ordersFilePath, _orders);
            Repository.Serialize(_productFilePath, _products);
        }
    }
}
