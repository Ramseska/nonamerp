using GTANetworkAPI;
using MySql.Data.MySqlClient;
using server_side.Data;
using server_side.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_side.Items
{
    class Item : Script
    {
        static public readonly List<Item> ListItems = new List<Item>();

        public int ItemID { get; private set; }
        public int OwnerID { get; private set; }
        public int ItemType { get; private set; }
        public int ItemAmount { get; private set; }
        public int InvenrorySlot { get; set; }

        public override string ToString()
        {
            return $"ItemID: {this.ItemID}, OwnerID: {this.OwnerID}, ItemType: {this.ItemType}, ItemAmount: {this.ItemAmount}, InvSlot: {this.InvenrorySlot}";
        }

        public Item() { }
        public Item(int itemid, int ownerid, int itemtype, int itemamount, int inventoryslot)
        {
            this.ItemID = itemid;
            this.OwnerID = ownerid;
            this.ItemType = itemtype;
            this.ItemAmount = itemamount;
            this.InvenrorySlot = inventoryslot;
        }

        public static Item CreateItem(int type, int amount = 1)
        {
            try
            {
                using (MySqlConnection con = MySqlConnector.GetDBConnection())
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand($"INSERT INTO `items` (`item_type`, `item_amount`) VALUES ('{type}', '{amount}')", con);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand("SELECT * FROM `items` WHERE `item_id` = LAST_INSERT_ID()", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Item item = new Item();

                    while (reader.Read())
                    {
                        item.ItemID = (int)reader["item_id"];
                        item.OwnerID = (int)reader["owner_id"];
                        item.ItemType = (int)reader["item_type"];
                        item.ItemAmount = (int)reader["item_amount"];
                        item.InvenrorySlot = (int)reader["inventory_slot"];
                    }
                    reader.Close();
                    con.Close();

                    ListItems.Add(item);

                    return item;
                }
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }

            return null;
        }

        public static void GivePlayerItem(Player player, Item item)
        {
            PlayerInfo pInfo = new PlayerInfo(player);

            item.OwnerID = pInfo.GetDbID();

            MySqlConnector.RequestExecuteNonQuery($"UPDATE `items` SET = `owner_id`='{item.OwnerID}',`item_type`='{item.ItemType}',`item_amount`='{item.ItemAmount}',`inventory_slot`='{item.InvenrorySlot}' WHERE `item_id` = '{item.ItemID}'");
        }

        public static void UseItem(Player player, Item item)
        {

        }

        public static void DeleteItem(int itemID)
        {
            ListItems.Remove(ListItems.Where(x => x.ItemID == itemID).First());
            MySqlConnector.RequestExecuteNonQuery($"DELETE FROM `items` WHERE `item_id` = '{itemID}'");
        }

        async static public void LoadAllItemsFromDB()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (MySqlConnection con = MySqlConnector.GetDBConnection())
                    {
                        int count = 0;

                        con.Open();

                        MySqlDataReader reader = new MySqlCommand("SELECT * FROM `items`", con).ExecuteReader();

                        while(reader.Read())
                        {
                            Item item = new Item();

                            item.ItemID = (int)reader["item_id"];
                            item.OwnerID = (int)reader["owner_id"];
                            item.ItemType = (int)reader["item_type"];
                            item.ItemAmount = (int)reader["item_amount"];
                            item.InvenrorySlot = (int)reader["inventory_slot"];

                            ListItems.Add(item);

                            count++;
                        }
                        reader.Close();
                        con.Close();

                        NAPI.Task.Run(() => NAPI.Util.ConsoleOutput($"[MySQL]: Загружено {count} предметов"));
                    }
                });
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }

        async static public void LoadUnownedItemsFromDB()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (MySqlConnection con = MySqlConnector.GetDBConnection())
                    {
                        con.Open();

                        MySqlDataReader reader = new MySqlCommand("SELECT * FROM `items`", con).ExecuteReader();

                        while (reader.Read())
                        {
                            if ((int)reader["owner_id"] == -1) continue;

                            ListItems.Add(new Item((int)reader["item_id"], (int)reader["owner_id"], (int)reader["item_type"], (int)reader["item_amount"], (int)reader["inventory_slot"]));
                        }
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

        async static public void LoadPlayerItemsFromDB(int playerDbId)
        {
            try
            {
                await Task.Run(() =>
                {
                    using(MySqlConnection con = MySqlConnector.GetDBConnection())
                    {
                        con.Open();

                        MySqlDataReader reader = new MySqlCommand($"SELECT * FROM `items` WHERE `owner_id` = {playerDbId}", con).ExecuteReader();

                        while (reader.Read())
                        {
                            Item item = new Item();

                            item.ItemID = (int)reader["item_id"];
                            item.OwnerID = (int)reader["owner_id"];
                            item.ItemType = (int)reader["item_type"];
                            item.ItemAmount = (int)reader["item_amount"];
                            item.InvenrorySlot = (int)reader["inventory_slot"];

                            ListItems.Add(item);
                        }
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

        async static public void UnloadPlayerItems(int playerDbId)
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (var i in ListItems.Where(x => x.OwnerID == playerDbId))
                    {
                        ListItems.Remove(i);
                    }
                });
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
    }
}
