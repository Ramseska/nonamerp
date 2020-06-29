using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server_side.Items
{
    public enum eItems : int
    {
        ITEM_AID_KIT,
        ITEM_APPLE,
        ITEM_SPRUNK
    }

    class ItemData : Script
    {
        public int Type { get; private set; }
        public int MaxItemsInStack { get; private set; }
        public Action<Player> Action;

        private ItemData() { }
        protected ItemData(eItems type) => this.Type = (int)type;


        public override string ToString() => $"Type: {this.Type}, MaxInStack: {this.MaxItemsInStack}";


        public readonly static List<ItemData> ItemDataList = new List<ItemData>()
        {
            new ItemData(eItems.ITEM_AID_KIT)
            {
                MaxItemsInStack = 3,
                Action = delegate (Player player)
                {
                    if(player.Health + 30 > 100) player.Health = 100;
                    else player.Health += 30;

                    Utils.UtilityFuncs.SendPlayerNotify(player, 0, "Вы использовали аптечку (+30 hp)");
                }
            }
        };
    }
}
