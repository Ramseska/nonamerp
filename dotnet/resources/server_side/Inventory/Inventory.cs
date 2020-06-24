using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using server_side.Data;
using server_side.Items;

namespace server_side.Inventory
{
    class Inventory
    {
        [RemoteEvent("sOnLoadInventory")]
        public void sOnLoadInventory(Player player)
        {
            List<Item> playerItems = Item.ListItems.Where(x => x.OwnerID == new PlayerInfo(player).GetDbID()).ToList();

            var ooo = NAPI.Util.ToJson(playerItems.ToArray<Item>());

        }
    }
}
