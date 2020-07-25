using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace server_side.Items
{
    class ItemEntity : Script
    {
        public int ItemID { get; private set; }
        public int OwnerID { get; set; }
        public string ItemType { get; private set; }
        public int ItemAmount { get; set; }
        public int InvenrorySlot { get; set; }

        private ItemEntity() { }

        public ItemEntity(int itemid, int ownerid, string itemtype, int itemamount, int inventoryslot)
        {
            this.ItemID = itemid;
            this.OwnerID = ownerid;
            this.ItemType = itemtype;
            this.ItemAmount = itemamount;
            this.InvenrorySlot = inventoryslot;
        }

        public override string ToString() => $"ItemID: {this.ItemID}, OwnerID: {this.OwnerID}, ItemType: {this.ItemType}, ItemAmount: {this.ItemAmount}, InvSlot: {this.InvenrorySlot}";
    }
}
