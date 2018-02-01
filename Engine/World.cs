using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public static class World
    {
        //creating lists and also creating constant variables to be added to the lists.
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        //creating the item names later used for List<Item> and assigning their numerical value
        public const int ITEM_ID_RUST_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;

        //creating monster names and assigning numerical values
        public const int MONSTER_RAT = 1;
        public const int MONSTER_SNAKE = 2;
        public const int MONSTER_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMIST_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        /*this is our constructor which runs the World method and as a result 
        the PopulateX methods are called and lists are populated*/
        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        //methods that are adding items to the lists that comprise the world

            //method to create the items list
        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUST_SWORD, "Rusty Sword", "Rusty Swords", 1, 25));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 35));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing Potion", "Healing Potions", 50));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat Tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of Fur", "Pieces of Fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake Fang", "Snake Fangs"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider Fangs"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider Silk", "Spider Silks"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer Pass", "Adventurer Passes"));
        }

        //method to create the list of monsters and what loot they have
        private static void PopulateMonsters()
        {
            //create the monsters and what loot they can potentially contain
            Monster rat = new Monster(MONSTER_RAT, "a Rat", 15, 3, 10, 45, 45);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, false));

            Monster snake = new Monster(MONSTER_SNAKE, "a Snek", 20, 5, 10, 50, 50);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, false));

            Monster spider = new Monster(MONSTER_GIANT_SPIDER, "A Giant Spider", 30, 15, 20, 60, 60);
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, false));
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 45, false));

            //adding the monsters created above to the list we created first on line 11
            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(spider);
        }

        /*method to create list of quests 
         Quest();
         item needed to complete.add();
         reward for completion();
             */
        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN, "Clear the Alchemist's Garden",
                "Kill rats in the alchemist's garden and bring back 3 rat tails." +
                "You will receive a healing potion and 10 gold pieces.", 20, 10);
            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));
            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField = new Quest(QUEST_ID_CLEAR_FARMERS_FIELD, "Clear the Farmer's field of snakes",
                "Kill snakes in the farmer's field and bring back 3 snake fangs." +
                "You will receive an adventurer's pass and 20 gold pieces.", 30, 20);
            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));
            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            //add the above quests to the quest list at line 12
            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
        }

        //method to populate the list of locations and if quests exists or monsters spawn
        private static void PopulateLocations()
        {
            //creating the locations first
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your humble home");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town Square",
                "Check out that fountain!");

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's Hut",
                "There are many strange plants on the shelves");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);
            Location alchemistGarden = new Location(LOCATION_ID_ALCHEMIST_GARDEN,"Alchemist's Garden",
                "Many plants are growing here");
            alchemistGarden.MonsterLivingHere = MonsterByID(MONSTER_RAT);

            Location farmHouse = new Location(LOCATION_ID_FARMHOUSE, "Farm House", 
                "There is a small house with a farmer out front");
            farmHouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);
            Location farmersField = new Location(LOCATION_ID_FARM_FIELD,"Farmer's Field", 
                "You see rows of vegetables growing here");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Gaurd Post", 
                "There is a tough looking guard here", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "A Bridge", 
                "A stone bridge crossing a wide river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Dark Forest", 
                "You see spider webs covering the trees in this forest.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_GIANT_SPIDER);

            //Add the locations to the static list
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistGarden);
            Locations.Add(farmHouse);
            Locations.Add(farmersField);
            Locations.Add(guardPost);
            Locations.Add(bridge);
            Locations.Add(spiderField);

            //Link the locations together
            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToSouth = home;
            townSquare.LocationToWest = farmHouse;

            farmHouse.LocationToEast = townSquare;
            farmHouse.LocationToWest = farmersField;
            farmersField.LocationToEast = farmHouse;

            alchemistHut.LocationToNorth = alchemistGarden;
            alchemistHut.LocationToSouth = townSquare;
            alchemistGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToWest = spiderField;

            spiderField.LocationToWest = bridge;
        }

        //wrapper methods being called to get values from the lists above

            //first we pass in the ID of the object
            //this is first used on line 76 to call which item is a possible LootItem for a monster
        public static Item ItemByID(int id)
        {
            //looks at each item in the list
            foreach(Item item in Items)
            {
                //if we find a match from the list based on what was passed in
                if(item.ID == id)
                {
                    //then return the item from the list that was passed initially.
                    return item;
                }
            }
            return null;
        }
        //see comments for ItemByID this works the same
        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }
            return null;
        }
        //see comments for ItemByID this works the same
        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if(quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }
        //see comments for ItemByID this works the same
        public static Location LocationByID(int id)
        {
            foreach(Location location in Locations)
            {
                if(location.ID == id)
                {
                    return location;
                }
            }
            return null;
        }
    }
}
