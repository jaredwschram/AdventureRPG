using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Weapon : Item
    {
        //properties
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }

        //constructor
        public Weapon(int id, string name, string namePlural, int minimumDamage, int maximumDamage) 
            : base(id, name, namePlural)
        {
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
        }
    }
}
