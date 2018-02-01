using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class QuestCompletionItem
    {
        //properties
        public Item Details { get; set; }
        public int Quantity { get; set; }

        //constructor
        public QuestCompletionItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
    }
}
