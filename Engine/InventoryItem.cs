using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class InventoryItem
    {
        //properties
        public Item Details { get; set; }
        public int Quantity { get; set; }

        //constructor
        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
    }
}
