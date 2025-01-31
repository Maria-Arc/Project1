using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }
        public string? Name { get; set; }

        public ShoppingCart()
        {
            Name = string.Empty;
        }

        public string? Display
        {
            get 
            {
                return $"{Id}. {Name}- {Quantity}  ${Price}";
            }
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
    
}
