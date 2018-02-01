using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Item //item is a base class for healing potion and weapons
    {
        //default properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }

        //constructor
        public Item(int id, string name, string namePlural)
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
        }
    }
}
