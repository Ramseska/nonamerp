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
        // В дальнейшем, во имя оптимизации, сделать список предметов для каждого игрока отдельным экземпляром.
        static public readonly List<ItemEntity> ItemsList = new List<ItemEntity>();


        public static void GivePlayerItem(Player player, string type, int amount = 1)
        {
            if(ItemData.ItemDataList.Where(x => x.Type == type).FirstOrDefault() == null)
            {
                NAPI.Util.ConsoleOutput("[Ошибка]: Выдаваемого предмета с типом " + type + " не существует!");
                return;
            }

            ItemEntity item = CreateItem(type, amount);
            item.OwnerID = new PlayerInfo(player).GetDbID();
            
            UpdateItemInDB(item);

            new Inventory.Inventory(player).GiveItem(item);

            foreach (var i in ItemsList)
                Console.WriteLine(i.ToString());
        }

        public static ItemEntity CreateItem(string type, int amount)
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
            /*
            try
            {
                using (MySqlConnection con = new MySqlConnector().GetDBConnection())
                {
                    con.Open();

                    new MySqlCommand($"INSERT INTO `items` (`item_type`, `item_amount`) VALUES ('{type}', '{amount}')", con).ExecuteNonQuery();

                    ItemEntity item = GetItemFromDB(Convert.ToInt32(MySqlHelper.ExecuteScalar(con, "SELECT LAST_INSERT_ID()")));

                    con.Close();

                    if(item != null)
                    {
                        ItemsList.Add(item);
                        return item;
                    }
                    else
                    {
                        NAPI.Util.ConsoleOutput("[Item Exception]: Не удалось создать объект.");
                    }
                }
                
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
            */
        }

        public static void DeleteItem(int itemID)
        {
            ItemEntity item = ItemsList.Where(x => x.ItemID == itemID).FirstOrDefault();

            if (item != null)
            {
                ItemsList.Remove(item);
                //new MySqlConnector().RequestExecuteNonQuery($"DELETE FROM `items` WHERE `item_id` = '{itemID}'");
                using(var db = new DataBase.AppContext())
                {
                    var md = db.Items.Where(x => x.Id == itemID).FirstOrDefault();
                    if(md != null)
                        db.Items.Remove(md);

                    db.SaveChangesAsync();
                }
                return;
            }

            NAPI.Util.ConsoleOutput($"[Item Exception]: Не удалось удалить объект (ID: {itemID}), т.к. он не был найден в списке.");
        }

        static public void LoadPlayerItemsFromDB(Player player, int playerDbId)
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
                /*
                await Task.Run(() =>
                {
                    using (MySqlConnection con = new MySqlConnector().GetDBConnection())
                    {
                        con.Open();

                        MySqlDataReader reader = new MySqlCommand($"SELECT * FROM `items` WHERE `owner_id` = {playerDbId}", con).ExecuteReader();

                        if(reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ItemsList.Add(new ItemEntity((int)reader["item_id"], (int)reader["owner_id"], (string)reader["item_type"], (int)reader["item_amount"], (int)reader["inventory_slot"]));
                            }

                            NAPI.Util.ConsoleOutput($"[{playerDbId}]: Загружено {ItemsList.Where(x => x.OwnerID == playerDbId).Count()} предметов.");
                        }
                        else { NAPI.Util.ConsoleOutput($"[Ахтунг]: Предметы для игрока {playerDbId} не найдены в базе данных!"); }

                        NAPI.Task.Run(() => { new Inventory.Inventory(player).Init(); });

                        reader.Close();
                        con.Close();
                    }
                });
                */
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
        static public void UnloadPlayerItems(int playerDbId) => ItemsList.RemoveAll(x => x.OwnerID == playerDbId);

        public static void UseItem(Player player, ItemEntity item)
        {
            try
            {
                ItemData itemData = ItemData.ItemDataList.Where(x => x.Type == item.ItemType).FirstOrDefault();

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
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }

        private static ItemEntity GetItemFromDB(int itemid)
        {
            /*
            using (MySqlConnection con = new MySqlConnector().GetDBConnection())
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `items` WHERE `item_id` = '{itemid}'", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                ItemEntity item = null;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new ItemEntity((int)reader["item_id"], (int)reader["owner_id"], (string)reader["item_type"], (int)reader["item_amount"], (int)reader["inventory_slot"]);
                    }
                }

                reader.Close();
                con.Close();

                return item;
            }
            */
            return null;
        }

        public static void UpdateItemInDB(ItemEntity item)
        {
            using(var db = new DataBase.AppContext())
            {
                var md = db.Items.Where(x => x.Id == item.ItemID).FirstOrDefault();

                NAPI.Util.ConsoleOutput($"\n\n\n>>> Item: {item} | Model: {md.Id}, {md.OwnerId}, {md.Type}, {md.Amount}, {md.Slot}");

                md.OwnerId = item.OwnerID;
                md.Type = item.ItemType;
                md.Amount = item.ItemAmount;
                md.Slot = item.InvenrorySlot;

                NAPI.Util.ConsoleOutput($"\n\n\n>>> Item: {item} | Model: {md.Id}, {md.OwnerId}, {md.Type}, {md.Amount}, {md.Slot}");

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
