using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Player : Creatures
    {
        //default properties for player class specifically
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }

        //List properties for player class
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }


        //constructor for player class. note the inhereted base class(Creatures) values.
        public Player(int gold, int experiencePoints, int level, int currentHitPoints, int maximumHitPoints) 
            : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.ItemRequiredToEnter == null)
            {
                //No item is required to enter so return true and end the loop
                return true;
            }
            //see if the player has required item in inv
            foreach(InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    //there is a required item and the player does have it
                    return true;
                }
            }
            //required item was not found
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CompletedThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }
            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            //See if the player has all the items needed to complete quest here
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;
                //check if player has item in inventory and enough of it
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        foundItemInPlayersInventory = true;
                        if (ii.Quantity < qci.Quantity)
                        {
                            return false;
                        }
                    }
                }

                //Player does not have any of this quest completion item
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }
            //if we got to this part of the loop then player must meet all requirements
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach(InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        //subtract the quantity from players inv
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }
        public void AddItemToInventory(Item itemToAdd)
        {
            foreach(InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == itemToAdd.ID)
                {
                    //they have item in inv so increase quantity by 1
                    ii.Quantity++;
                        return;//item is added so exit loop
                }
            }
            //player has none of item so add new item to inventory
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }
        public void MarkQuestCompleted(Quest quest)
        {
            //find the quest in player's quest list
            foreach(PlayerQuest pq in Quests)
            {
                if(pq.Details.ID == quest.ID)
                {
                    pq.IsCompleted = true;
                    //found quest and marked complete so exit loop
                    return;
                }
            }
        }
    }
}
