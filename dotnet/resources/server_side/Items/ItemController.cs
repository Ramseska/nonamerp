using GTANetworkAPI;
using MySql.Data.MySqlClient;
using server_side.Data;
using server_side.DataBase.Models;
using server_side.DBConnection;
using server_side.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace server_side.Items
{
    class ItemController : Script
    {
        public static readonly List<ItemEntity> ItemsList = new List<ItemEntity>();

        public ItemController() { }

        public void GivePlayerItem(Player player, string type, int amount = 1)
        {
            if(ItemData.ItemDataList.Where(x => x.Type == type).FirstOrDefault() == null)
            {
                NAPI.Util.ConsoleOutput("[Ошибка]: Выдаваемого предмета с типом " + type + " не существует!");
                return;
            }

            ItemEntity item = CreateItem(type, amount);
            item.OwnerID = new PlayerInfo(player).GetDbID();
            
            UpdateItemInDB(item);

            new InventorySystem.Inventory(player).GiveItem(item);

            foreach (var i in ItemsList)
                Console.WriteLine(i.ToString());
        }

        public ItemEntity CreateItem(string type, int amount)
        {
            using (DataBase.AppContext db = new DataBase.AppContext())
            {
                var md = new ItemModel();
                md.Type = type;
                md.Amount = amount;

                db.Items.Add(md);

                db.SaveChanges();

                ItemEntity item = new ItemEntity(md.Id, md.OwnerId, md.Type, md.Amount, md.Slot);

                ItemsList.Add(item);

                return item;
            }
        }

        public void DeleteItem(int itemID)
        {
            ItemEntity item = ItemsList.Where(x => x.ItemID == itemID).FirstOrDefault();

            if (item != null)
            {
                ItemsList.Remove(item);
                
                using(var db = new DataBase.AppContext())
                {
                    var md = db.Items.Where(x => x.Id == itemID).FirstOrDefault();
                    if(md != null)
                        db.Items.Remove(md);

                    db.SaveChanges();
                }
                return;
            }

            NAPI.Util.ConsoleOutput($"[Item Exception]: Не удалось удалить объект (ID: {itemID}), т.к. он не был найден в списке.");
        }

        public void LoadPlayerItemsFromDB(Player player, int playerDbId)
        {
            try
            {
                using (var db = new DataBase.AppContext())
                {
                    var mds = db.Items.Where(x => x.OwnerId == playerDbId).ToList();

                    if(mds.Any())
                    {
                        foreach(var i in mds)
                        {
                            ItemsList.Add(new ItemEntity(i.Id, i.OwnerId, i.Type, i.Amount, i.Slot));
                        }
                        NAPI.Util.ConsoleOutput($"[{playerDbId}]: Загружено {mds.Count} предметов.");
                    }
                    else { NAPI.Util.ConsoleOutput($"[Ахтунг]: Предметы для игрока {playerDbId} не найдены в базе данных!"); }
                }
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
        public void UnloadPlayerItems(int playerDbId) => ItemsList.RemoveAll(x => x.OwnerID == playerDbId);

        public ItemEntity UseItem(Player player, ItemEntity item)
        {
            ItemData itemData = ItemData.ItemDataList.Where(x => x.Type == item.ItemType).FirstOrDefault();

            /*

            if (itemData.Action != null)
            {
                itemData.Action(player);


                if (--item.ItemAmount <= 0)
                {
                    DeleteItem(item.ItemID);
                }   
                else
                {
                    UpdateItemInDB(item);
                }
                return;
            }
            NAPI.Util.ConsoleOutput($"[Ошибка]: Не удалось использовать предмет, т.к. его действие не описано в классе ItemData\nItem: {item}");
            */

            if(itemData.Action == null)
            {
                NAPI.Util.ConsoleOutput($"[Ошибка]: Не удалось использовать предмет, т.к. его действие не описано в классе ItemData\nItem: {item}");
                return item;
            }

            item.ItemAmount--;

            itemData.Action(player);

            ItemEntity _item = item;

            if (item.ItemAmount <= 0)
            {
                DeleteItem(item.ItemID);
            }   
            else
            {
                UpdateItemInDB(item);
            } 

            return _item;
        }

        private void UpdateItemInDB(ItemEntity item)
        {
            using(var db = new DataBase.AppContext())
            {
                var md = db.Items.Where(x => x.Id == item.ItemID).FirstOrDefault();

                md.OwnerId = item.OwnerID;
                md.Type = item.ItemType;
                md.Amount = item.ItemAmount;
                md.Slot = item.InvenrorySlot;

                db.SaveChanges();
            }
            /*
            string query = $"" +
                $"UPDATE `items` SET " +
                $"`owner_id` = '{item.OwnerID}', " +
                $"`item_type` = '{item.ItemType}', " +
                $"`item_amount` = '{item.ItemAmount}'," +
                $"`inventory_slot` = '{item.InvenrorySlot}' " +
                $"WHERE `item_id` = '{item.ItemID}'";
            new MySqlConnector().RequestExecuteNonQuery(query);
            */
        }
    }
}
