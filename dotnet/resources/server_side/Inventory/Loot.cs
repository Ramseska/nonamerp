using GTANetworkAPI;
using server_side.Data;
using server_side.DataBase.Models;
using server_side.DBConnection;
using server_side.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using server_side.Items;

/*

*/

namespace server_side.InventorySystem
{
    class Loot
    {
        
        public List<ItemEntity> LootItems = new List<ItemEntity>();


    }

    class LootBag
    {
        Loot loot;
        public int Id { get; set; }
        public int Owner { get; set; }

        public static readonly List<Loot> LootBagList = new List<Loot>();

        private LootBag(int owner)
        {
            
        }

        public void OpenLootBag()
        {

        }

        public void CreateLootBag()
        {

        }

        public void AddItemsInLootBag(ItemEntity item)
        {

        }

        public void DestroyLootBag()
        {

        }
    }
}