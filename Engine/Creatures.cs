using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Creatures
    {
        //creatures is a base class for monsters and player these are the base attributes for each
        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; set; }

        //creatures constructor
        public Creatures(int currentHitPoints, int maximumHitPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
        }
    }
}
