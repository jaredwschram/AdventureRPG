using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class PlayerQuest
    {
        //properties
        public Quest Details { get; set; }
        public bool IsCompleted { get; set; }
        
        //constructor
        public PlayerQuest(Quest details)
        {
            Details = details;
        }
    }
}
