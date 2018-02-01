using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Quest
    {
        //default properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        //custom properties
        public Item RewardItem { get; set; }
        //list property
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }

        //quest constructor
        public Quest(int id, string name, string description, int rewardExperiencePoints, int rewardGold)
        {
            ID = id;
            Name = name;
            Description = description;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            QuestCompletionItems = new List<QuestCompletionItem>();
        }
    }
}
