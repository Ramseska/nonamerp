using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace server_side.Items
{
    class ItemEntity
    {
        [JsonProperty("itemID")]
        public int ItemID { get; private set; }
        [JsonProperty("ownerID")]
        public int OwnerID { get; set; }
        [JsonProperty("itemType")]
        public string ItemType { get; private set; }
        [JsonProperty("itemAmount")]
        public int ItemAmount { get; set; }
        [JsonProperty("inventorySlot")]
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
