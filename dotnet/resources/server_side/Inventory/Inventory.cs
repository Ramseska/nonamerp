using System;
using System.Linq;
using GTANetworkAPI;
using server_side.Data;
using server_side.Items;
using System.IO;
using Newtonsoft.Json;

namespace server_side.InventorySystem
{

    class Inventory : Script
    {
        private Player _player = null;
        private PlayerInfo player = null;

        private Inventory() { }
        public Inventory(Player player)
        {
            this._player = player;
            this.player = new PlayerInfo(player);
        }


        public void Init()
        {
            //name, cash, bank, health, hungry, thirst, items, itemsData
            NAPI.ClientEvent.TriggerClientEvent(_player, "InventoryLoad", player.GetName(), player.GetMoney(), player.GetBankMoney(), _player.Health, player.GetSatiety(), player.GetThirst(), JsonConvert.SerializeObject(ItemController.ItemsList.Where(x => x.OwnerID == player.GetDbID())), File.ReadAllText(@"dotnet/itemData.json"));

            NAPI.Util.ConsoleOutput("[Inventory]: " + player.GetName() + " - инвентарь инициализирован");
        }

        public void Update()
        {

        }

        public void UpdateItem(object item)
        {

        }

        [RemoteEvent("sUseItem")]
        public void UseItem(Player player, int id)
        {
            ItemEntity item = ItemController.ItemsList.Where(x => x.ItemID == id).FirstOrDefault();

            if (item == null)
            {
                Utils.UtilityFuncs.SendPlayerNotify(player, 0, "Ошибка! Данного предмета не существует на сервере!");
                return;
            }

            var temp = JsonConvert.SerializeObject(new ItemController().UseItem(player, item));

            NAPI.ClientEvent.TriggerClientEvent(player, "InventoryUpdateItem", temp);

            Console.WriteLine($"Serialized object in UseItem: {temp}");
        }

        public void GiveItem(ItemEntity item)
        {
            NAPI.ClientEvent.TriggerClientEvent(_player, "InventoryAddItem", JsonConvert.SerializeObject(item));
        }

        /*
        [RemoteEvent("sRemoveItem")]
        public void RemoveItem(Player player, int id)
        {
            ItemEntity item = ItemController.ItemsList.Where(x => x.ItemID == id).FirstOrDefault();

            if (item == null)
            {
                Utils.UtilityFuncs.SendPlayerNotify(player, 0, "Ошибка! Данного предмета не существует на сервере!");
                return;
            }

            else if(item.OwnerID != new PlayerInfo(player).GetDbID())
            {
                Utils.UtilityFuncs.SendPlayerNotify(player, 0, "Ошибка! Данный предмет не пренадлежит Вам!");
                return;
            }

            new ItemController().DeleteItem(item.ItemID);
        }
        */

        public void UpdateBar()
        {
            //name, cash, money, health, hunger, thirst
            NAPI.ClientEvent.TriggerClientEvent(_player, "InventoryUpdateBar", player.GetName(), player.GetMoney(), player.GetBankMoney(), _player.Health, player.GetSatiety(), player.GetThirst());
        }
    }
}
