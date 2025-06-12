using Exercici5.Models;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Exercici5
{
    public class Program() {
            public static AuxBattery Battery { get; set; }
            public static List<Product> Products { get; set; }
            public static object Locker = new object();
            public static int ProductsCharged = 0;
            public static bool ChargeNextProduct = true;
            public static void Main()
            {
                Stopwatch sw = new Stopwatch();
                int userInput;
                const string Menu = "Insert 1 to use user inputs as data. \nInsert 2 for example number one. \nInsert 3 for example number two.";
                const string BatteryCapacityInput = "Insert the battery capacity:";
                const string ProductCountInput = "Insert the number of products:";
                const string ProductNameInput = "Insert the name of product number {0}: ";
                const string ProductCapacityInput = "Insert the capacity of product number {0}: ";
                const string ProductConsumeInput = "Insert the consumption of product number {0}: ";
                string simulationResult = "{0} out of {1} were charged in a span of {2} milliseconds";
                const string TotalCharges = "Total charges done by the battery: ";
                int totalCharges = 0;
                Console.WriteLine(Menu);
                try
                {
                    userInput = int.Parse(Console.ReadLine());
                    switch (userInput)
                    {
                        case 1:
                            int capacity, productCount;
                            Console.WriteLine(BatteryCapacityInput);
                            capacity = int.Parse(Console.ReadLine());
                            Battery = CreateBattery(capacity);
                            Console.WriteLine(ProductCountInput);
                            productCount = int.Parse(Console.ReadLine());
                            for (int i = 0; i < productCount; i++)
                            {
                                Console.WriteLine(ProductNameInput, i + 1);
                                string productName = Console.ReadLine();
                                Console.WriteLine(ProductCapacityInput, i + 1);
                                int productCapacity = int.Parse(Console.ReadLine());
                                Console.WriteLine(ProductConsumeInput, i + 1);
                                int productConsume = int.Parse(Console.ReadLine());
                                Products.Add(CreateProduct(productName, productCapacity, productConsume));
                            }
                            break;
                        case 2:
                            Battery = new AuxBattery { Capacity = 100000, CurrentCharge = 100000 };
                            Products = new List<Product>{
                                    new Product { ProductName = "1", Capacity = 30000, Consume = 10000 },
                                    new Product { ProductName = "2", Capacity = 20000, Consume = 12000 },
                                    new Product { ProductName = "3", Capacity = 5000, Consume = 1000 }
                        };
                            break;
                        case 3:
                            Battery = new AuxBattery { Capacity = 100000, CurrentCharge = 100000 };
                            Products = new List<Product>{
                                    new Product { ProductName = "1", Capacity = 25000, Consume = 23000 },
                                    new Product { ProductName = "2", Capacity = 20000, Consume = 12000 },
                                    new Product { ProductName = "3", Capacity = 8000, Consume = 1000 },
                                    new Product { ProductName = "4", Capacity = 10000, Consume = 1000 }
                        };
                            break;
                    }
                    Thread[] threads = new Thread[Products.Count];
                    sw.Start();
                    for (int i = 0; i < Products.Count; i++)
                    {
                        if (ChargeNextProduct)
                        {
                            int index = i;
                            threads[i] = new Thread(() => ProductBehaivour(Products[index]));
                            threads[i].Start();
                            threads[i].Join();
                            totalCharges += Products[i].ChargesDone;
                        }
                    }
                    sw.Stop();
                    Console.WriteLine(simulationResult, ProductsCharged, Products.Count, sw.ElapsedMilliseconds);
                    Console.WriteLine(TotalCharges + totalCharges);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        public static void ProductBehaivour(Product product)
        {
            string noBattery = $"The battery doesn't have enough charge to charging {product.ProductName}";
            string fullyCharged = "The product {0} took {1} charges to fully charge\n";
            if (Battery.CurrentCharge >= product.Capacity)
            {
                lock (Locker)
                {
                    while (product.CurrentCharge < product.Capacity) {
                        Console.WriteLine($"Charging product {product.ProductName}:");
                        if (product.CurrentCharge + product.Consume <= product.Capacity)
                        {
                            Battery.CurrentCharge -= product.Consume;
                            product.CurrentCharge += product.Consume;
                        }
                        else
                        {
                            int chargeMissing = product.Capacity - product.CurrentCharge;
                            Battery.CurrentCharge -= chargeMissing;
                            product.CurrentCharge += chargeMissing;
                        }
                        Console.WriteLine($"{product.CurrentCharge} out of {product.Capacity}\n");
                        product.ChargesDone++;
                    }
                    ProductsCharged++;
                    Console.WriteLine(fullyCharged, product.ProductName, product.ChargesDone);
                }
            } else
            {
                Console.WriteLine(noBattery);
                ChargeNextProduct = false;
            }
        }
        public static AuxBattery CreateBattery(int capacity) => new AuxBattery { Capacity = capacity, CurrentCharge = capacity };
        public static Product CreateProduct(string name, int capacity, int consumption) => new Product { ProductName = name, Capacity = capacity, Consume = consumption };

    }
}