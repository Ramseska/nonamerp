using GTANetworkAPI;
using MySql.Data.MySqlClient;
using server_side.Data;
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

            foreach (var i in ItemsList)
                Console.WriteLine(i.ToString());
        }

        public static ItemEntity CreateItem(string type, int amount)
        {
            try
            {
                using (MySqlConnection con = MySqlConnector.GetDBConnection())
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

            return null;
        }

        public static void DeleteItem(int itemID)
        {
            ItemEntity item = ItemsList.Where(x => x.ItemID == itemID).FirstOrDefault();

            if (item != null)
            {
                ItemsList.Remove(item);
                MySqlConnector.RequestExecuteNonQuery($"DELETE FROM `items` WHERE `item_id` = '{itemID}'");
                return;
            }

            NAPI.Util.ConsoleOutput($"[Item Exception]: Не удалось удалить объект (ID: {itemID}), т.к. он не был найден в списке.");
        }

        async static public void LoadPlayerItemsFromDB(int playerDbId)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (MySqlConnection con = MySqlConnector.GetDBConnection())
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

                        reader.Close();
                        con.Close();
                    }
                });
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
            using (MySqlConnection con = MySqlConnector.GetDBConnection())
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
        }

        public static void UpdateItemInDB(ItemEntity item)
        {
            string query = $"" +
                $"UPDATE `items` SET " +
                $"`owner_id` = '{item.OwnerID}', " +
                $"`item_type` = '{item.ItemType}', " +
                $"`item_amount` = '{item.ItemAmount}'," +
                $"`inventory_slot` = '{item.InvenrorySlot}' " +
                $"WHERE `item_id` = '{item.ItemID}'";
            MySqlConnector.RequestExecuteNonQuery(query);
        }
    }
}
