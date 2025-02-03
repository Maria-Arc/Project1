using Spring2025_Samples.Models;
using Library.eCommerce.Services1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace Library.eCommerce.Services
{
    public class ShoppingCartServiceProxy
    {
        public List<Product?> Cart { get; private set; }
        private ShoppingCartServiceProxy()
        {
            Cart = new List<Product?>();
        }

        private int LastKey
        {
            get
            {
                if (!Cart.Any())
                {
                    return 0;
                }

                return Cart.Select(p => p?.Id ?? 0).Max();
            }
        }

        private static ShoppingCartServiceProxy? instance;
        private static object instanceLock = new object();
        public static ShoppingCartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ShoppingCartServiceProxy();
                    }
                }

                return instance;
            }
        }

  
        //pass in SC item and product it affects, modify both

        //lowkey should have just passed in name

        //x = what exists, i = what changing to 
        public Product AddOrUpdate(Product x, Product Product, int i = 0)
        {
           // Console.WriteLine("comparing" + i +  "with" + Product.Quantity + " + " + x.Quantity );
            if (i <= Product.Quantity + x.Quantity )
            {
                //&& x.Quantity >= 0
                if (x.Id == 0 )
                {
                    x.Id = LastKey + 1;
                    Cart.Add(x);
                }

                Product.Quantity -= i - x.Quantity;
                x.Quantity = i;
            }

            if (x.Quantity == 0)
            {
                Delete(x.Id);
            }

            return Product;
        }

        public Product? Return(int x, List<Product?> Product)
        {
            if (x == 0)
                return null;

            Product? item = Cart.FirstOrDefault(p => p?.Id == x);
            var prod = Product.FirstOrDefault(p => p?.Name == item?.Name);
            prod.Quantity += item.Quantity;
            Cart.Remove(item);

            if (item.Quantity == 0)
            {
                Delete(item.Id);
            }
            return prod;
        }

        public Product? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Product? inventory = Cart.FirstOrDefault(p => p.Id == id);
            Cart.Remove(inventory);

            return inventory;
        }

        public void Checkout()
        {
            double total = 0;
            Console.WriteLine("\n\nItemized Receipt:");
            Console.WriteLine("{0,-20} {1,8} {2,15:N2} {3,15:N2}", "Name", "Amount", " Unit Price", "Total Price\n");
            for (int i = 0; i < Cart.Count; ++i)
            {
                Console.WriteLine("{0,-20} {1,8} {2,15:N2} {3,15:N2}", Cart[i]?.Name,
                            Cart[i]?.Quantity, Cart[i]?.Price, Cart[i]?.Quantity * Cart[i]?.Price);
                total += Cart[i].Quantity * Cart[i].Price;
            }

            Console.WriteLine("");
            Console.WriteLine("{0,45} {1,15:N2}", "Subtotal:", total);
            Console.WriteLine("{0,45} {1,15:N2}" , "Total (including 7% sales tax:", total * 1.07);
        }
    }
}
