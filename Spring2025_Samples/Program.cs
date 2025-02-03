//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Library.eCommerce.Services1;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
    

            Console.WriteLine("S. Add an item to the shopping cart");
            Console.WriteLine("I. Read all shopping cart items");
            Console.WriteLine("N. Update number of items in the shopping cart");
            Console.WriteLine("X. Return all of a product from the shopping cart to the inventory");
            Console.WriteLine("Q. Quit");
 
            List<Product?> list = ProductServiceProxy.Current.Products;
            List<Product?> cart = ShoppingCartServiceProxy.Current.Cart;

            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = Console.ReadLine(),
                            Quantity = int.Parse(Console.ReadLine() ?? "-1"),
                            Price = double.Parse(Console.ReadLine() ?? "-1")
                        });
                        break;
                    case 'R':
                    case 'r':

                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products

                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p?.Id == selection);
                        var selectedCart = cart.FirstOrDefault(p => p?.Name == selectedProd?.Name);

                        //wrpng orderr

                        if (selectedCart != null)
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";
                            selectedProd.Quantity = int.Parse(Console.ReadLine() ?? "-1") - selectedCart.Quantity;
                            selectedProd.Price = double.Parse(Console.ReadLine() ?? "-1");
                            selectedCart.Name = selectedProd.Name;
                            selectedCart.Price = selectedProd.Price;
                            ProductServiceProxy.Current.AddOrUpdate(
                                ShoppingCartServiceProxy.Current.AddOrUpdate(selectedCart, selectedProd, selectedCart.Quantity));

                        }
                        else if (selectedProd != null)
                            {
                                selectedProd.Name = Console.ReadLine() ?? "ERROR";
                                selectedProd.Price = double.Parse(Console.ReadLine() ?? "-1");
                                selectedProd.Quantity = int.Parse(Console.ReadLine() ?? "-1");
                                ProductServiceProxy.Current.AddOrUpdate(selectedProd);

                            }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProd = list.FirstOrDefault(p => p?.Id == selection);
                        
                        selectedCart = cart.FirstOrDefault(p => p?.Name == selectedProd?.Name);
                        if (selectedCart != null)
                        {
                            ProductServiceProxy.Current.AddOrUpdate(
                                ShoppingCartServiceProxy.Current.Return(selectedCart.Id, list));
                            ProductServiceProxy.Current.Delete(selection);
                            ShoppingCartServiceProxy.Current.Delete(selectedProd.Id);
                        }
                        else
                        {
                            ProductServiceProxy.Current.Delete(selection);
                        }

                        break;
                    case 'S':
                    case 's':
                        Console.WriteLine("Which inventory item to the shopping cart?");
                        Console.WriteLine("What quantity?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");              
                        selectedProd = list.FirstOrDefault(p => p?.Id == selection);
                        selection = int.Parse(Console.ReadLine() ?? "-1");

                        if (selectedProd != null)
                        {
                            ProductServiceProxy.Current.AddOrUpdate(
                            ShoppingCartServiceProxy.Current.AddOrUpdate(new Product
                            {
                                Name = selectedProd.Name,
                                Price = selectedProd.Price
                            }, selectedProd, selection));
                        }
                        break;
                    case 'I':
                    case 'i':
                        cart.ForEach(Console.WriteLine);
                        break;
                    case 'N':
                    case 'n':
                        Console.WriteLine("Which inventory item to update quantity?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProd = list.FirstOrDefault(p => p?.Id == selection); 
                        selectedCart = cart.FirstOrDefault(p => p?.Name == selectedProd.Name);


                        if (selectedCart != null)
                        {
                            Console.WriteLine("What quantity?");
                            selection = int.Parse(Console.ReadLine() ?? "-1");

                            ProductServiceProxy.Current.AddOrUpdate(
                                ShoppingCartServiceProxy.Current.AddOrUpdate(selectedCart, selectedProd, selection));
                            
                        }
                        else
                            Console.WriteLine("DNE");

                        break;
                    case 'X':
                    case 'x':
                        Console.WriteLine("Which cart item do you want to return to inventory?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedCart = cart.FirstOrDefault(p => p?.Id == selection);
                        selectedProd = list.FirstOrDefault(p => p?.Name == selectedCart?.Name);
                        //selectedProd = list.FirstOrDefault(p => p.Id == selection);
                        if (selectedCart != null)
                        {
                            ProductServiceProxy.Current.AddOrUpdate(
                                ShoppingCartServiceProxy.Current.Return(selection, list));
                        }

                        break;
                    case 'Q':
                    case 'q':
                        ShoppingCartServiceProxy.Current.Checkout();
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            Console.ReadLine();
        }
    }


}
