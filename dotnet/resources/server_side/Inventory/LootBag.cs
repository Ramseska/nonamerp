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



namespace server_side.InventorySystem
{
    class LootBag
    {
        public int Id { get; private set; }
        public int Owner { get; private set; }

        public static readonly List<LootBag> LootBagsList = new List<LootBag>();


        public LootBag() {}
        private LootBag(int owner)
        {
            this.Owner = owner;

            LootBagsList.Add(this);

            this.Id = LootBagsList.IndexOf(this);
        }


        public void Create(int ownerid)
        {

        }

        private void Destroy(LootBag bag)
        {

        }

        public void Open(int id)
        {

        }

        public void AddItem(int id)
        {

        }

        public void TakeItem(int id)
        {

        }
    }
}