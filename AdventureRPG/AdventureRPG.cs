using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace AdventureRPG
{
    public partial class AdventureRPG : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public AdventureRPG()
        {
            InitializeComponent();

            Location location = new Location(1, "home", "This is your house");

            //instatiate the play and their starting location+item+attributes
            _player = new Player(10, 0, 1, 100, 100);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUST_SWORD), 1));

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }
        private void MoveTo(Location newLocation)
        {
            //Does the location have any required item to enter
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You must have a " + " to enter this location." + Environment.NewLine;
                return;
            }
            //update player's current location
            _player.CurrentLocation = newLocation;
            
            //show/hide available movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);

            //display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            //completely heal player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            //update HP in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            //quest available?
            if(newLocation.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                //see if player already has quest
                if (playerAlreadyHasQuest)
                {
                    //if player has not completed it
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool playerHasAllItemsToCompleteQuest = 
                            _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);
                        //has item and enough of it to complete quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            //display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the" +
                                newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);
                            //give reard
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() +
                                " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() +
                                " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name +
                                Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    //player does not have quest
                    //display messages
                    rtbMessages.Text += "You receive the " +
                        newLocation.QuestAvailableHere.Name +
                        " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it return here with: " + Environment.NewLine;
                    foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;
                    //add quest to players quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }
            //does location have monster?
            if(newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, 
                    standardMonster.Name, standardMonster.MaximumDamage, 
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, 
                    standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

                foreach(LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }
            //refresh players inventory
            UpdateInventoryListInUI();
            //refresh players quest list
            UpdateQuestListInUI();
            //refresh players weapons menu
            UpdateWeaponListInUI();
            //refresh players potions menu
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 160;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[]
                    {
                        inventoryItem.Details.Name,
                        inventoryItem.Quantity.ToString()});
                }
            }

        }
         
        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 160;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach(PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[]
                {
                    playerQuest.Details.Name,
                    playerQuest.IsCompleted.ToString()});
            }
        }
        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is Weapon)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }
            if(weapons.Count == 0)
            {
                //player has no weapons - hide buttons and boxes for weapons
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }
        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }
            if(healingPotions.Count == 0)
            {
                //Player has no potions so hide the button and box for potions
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            //Get currently selected weapon from the cboWeapons comboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            //Determine amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(
                currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            //apply the damage to the monster's current hitpoints
            _currentMonster.CurrentHitPoints -= damageToMonster;

            //display to messages
            rtbMessages.Text += "You hit the " 
                + _currentMonster.Name + " for " + damageToMonster.ToString() + 
                " points" + Environment.NewLine;

            //check if monster died
            if(_currentMonster.CurrentHitPoints < 0)
            {
                //monster is ded
                rtbMessages.Text = Environment.NewLine;
                rtbMessages.Text = "You killed the " + _currentMonster.Name +
                    Environment.NewLine;
                //give xp to player
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.Text = "You receive " + _currentMonster.RewardExperiencePoints.ToString() +
                    " experience points" + Environment.NewLine;

                //give gold to player
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text = "You receive " + _currentMonster.RewardGold.ToString() +
                    " gold pieces" + Environment.NewLine;

                //get random loot items
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                //add items to lootitems lsit comparin a random number to drop percentage
                foreach(LootItem lootItem in _currentMonster.LootTable)
                {
                    if(RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }
                //if no items were selected then add default loot
                if(lootedItems.Count == 0)
                {
                    foreach(LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                //add looted items to players inv
                foreach(InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if(inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " +
                            inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " +
                            inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }

                //refresh players information and inv controls
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                //add a blank line to the message box for aesthetics
                rtbMessages.Text += Environment.NewLine;

                // move player to current location (heals player and provides new monster to fight)
                MoveTo(_player.CurrentLocation);
            }
            //monster still alive
            else
            {
                //determine amount of damage monster does to player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(
                    0, _currentMonster.MaximumDamage);

                //display msg
                rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() +
                    " points of damage to you!" + Environment.NewLine;

                //remove hp from player
                _player.CurrentHitPoints -= damageToPlayer;

                //refresh player data in ui
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();

                if(_player.CurrentHitPoints < 0)
                {
                    //display message player ded
                    rtbMessages.Text += _currentMonster.Name + " killed you!" + Environment.NewLine;

                    //move player home
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }
        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            //get currently selected potion from combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            //Add healing amount to players current hit points
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            //current hp cannot exceed maxium
            if(_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            //remove the used potion from players inventory
            foreach(InventoryItem ii in _player.Inventory)
            {
                if(ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }
            //Display message that you drank a potion
            rtbMessages.Text += "You use a " + potion.Name + " to restore " + 
                potion.AmountToHeal + " health points" + Environment.NewLine;

            //monsters turn to attack

            //determine amount of damage monster inflicts

            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            //display the message

            rtbMessages.Text += "The " + _currentMonster.Name + " hit you for " + damageToPlayer.ToString() +
                " points of damage!" + Environment.NewLine;

            //remove hp from player

            _player.CurrentHitPoints -= damageToPlayer;

            if(_player.CurrentHitPoints < 0)
            {
                //tell player they ded
                rtbMessages.Text += "You were killed by a " + _currentMonster.Name + Environment.NewLine;

                //move player home
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            //refresh player ui data
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }
    }
}
