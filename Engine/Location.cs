using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Location
    {
        //default properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        //location detail properties
        public Item ItemRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public Monster MonsterLivingHere { get; set; }

        //properties used for connecting locations together
        public Location LocationToNorth { get; set; }
        public Location LocationToEast { get; set; }
        public Location LocationToSouth { get; set; }
        public Location LocationToWest { get; set; }

        //location constructor
        public Location(int id, string name, string description,
            Item itemRequiredToEnter = null,
                Quest questAvailableHere = null,
                    Monster monsterLivingHere = null)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            MonsterLivingHere = monsterLivingHere;
        }
    }
}
