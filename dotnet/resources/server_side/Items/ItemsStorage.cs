using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;



namespace server_side.Items
{
    class ItemsStorage : Script
    {
        public readonly List<ItemEntity> ItemsList = new List<ItemEntity>();

        public void AddItem(ItemEntity item) => ItemsList.Add(item);
        public void RemoveItem(ItemEntity item) => ItemsList.Remove(item);
        public ItemEntity GetItemById(int id) 
        {
            return ItemsList.Where(x => x.ItemID == id).FirstOrDefault();
        }
    }
}