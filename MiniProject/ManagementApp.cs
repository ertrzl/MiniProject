using MiniProject.Models;
using MiniProject.Services;
using MiniProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace MiniProject
{
    internal class ManagementApp
    {
       
        public void Run()
        {
            {
                ProductService productService = new ProductService();

                OrderService orderService = new OrderService();

                while (true)
                {
                    
                    productService.CheckLowStockAlert();
                    Console.WriteLine("1. Create Product\n2. Delete Product\n3. Get Product By Id\n4. Show All Products\n5. Refill Product\n6. Order Product\n7. Show All Orders\n8. Change Order Status\n9. Show According To Email\n10. Show Best Sellers\n11. Show Financial Report\n\n\n0. Exit");
                    
                    Console.Write("Select an option: ");
                    var choice = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(choice)) continue;




                    switch (choice.Trim())
                    {
                        case "0":
                            Console.Clear();

                            return;
                        case "1":
                            Console.Clear();

                            productService.CreateProduct();
                            break;
                        case "2":
                            Console.Clear();
                            productService.DeleteProduct();
                            break;
                        case "3":
                            Console.Clear();
                            productService.GetProductById();
                            break;
                        case "4":
                            Console.Clear();
                            productService.ShowAllProducts();
                            break;
                        case "5":
                            Console.Clear();
                            productService.RefillProduct();
                            break;
                        case "6":
                            Console.Clear();
                            orderService.OrderProduct();
                            break;
                        case "7":
                            Console.Clear();
                            orderService.ShowAllOrders();
                            break;
                        case "8":
                            Console.Clear();
                            orderService.ChangeOrderStatus();
                            break;
                        case "9":
                            Console.Clear();
                            orderService.ShowCustomerOrders();
                            break;
                        case "10":
                            Console.Clear();
                            orderService.ShowBestSellers();
                            break;
                        case "11":
                            Console.Clear();
                            orderService.ShowFinancialReport();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid option, try again.");
                            break;
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}




                    