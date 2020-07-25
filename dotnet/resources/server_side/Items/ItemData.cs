﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace server_side.Items
{
    class ItemData : Script
    {
        [JsonProperty("ItemType")]
        public string Type { get; private set; }
        [JsonProperty("ItemStack")]
        public int MaxItemsInStack { get; private set; }
        [JsonProperty("ItemGroup")]
        public string Group { get; private set; }
        [JsonProperty("ItemWeight")]
        public int Weight { get; private set; }
        public Action<Player> Action { get; private set; }

        public static List<ItemData> ItemDataList = new List<ItemData>();

        private ItemData() { }
        //public ItemData(string type) => this.Type = type;
        public ItemData(string type, int stack, string group, int weight)
        {
            Type = type;
            MaxItemsInStack = stack;
            Group = group;
            Weight = weight;
            Action = null;
        }


        public override string ToString() => $"Type: {this.Type}, MaxInStack: {this.MaxItemsInStack}, Group: {Group}, Weight: {Weight}, Action: {(Action == null ? "null" : $"{Action}")}";



        public static void InitItemData()
        {
            UnPackDataFromJson();

            Dictionary<string, Action<Player>> actions = new Dictionary<string, Action<Player>>()
            {
                {
                    "ITEM_AID_KIT", (Player player) =>
                    {
                        if(player.Health + 30 > 100) player.Health = 100;
                        else player.Health += 30;

                        Utils.UtilityFuncs.SendPlayerNotify(player, 0, "Вы использовали аптечку (+30 hp)");
                    }
                },
                {
                    "ITEM_APPLE", (Player player) =>
                    {

                    }
                }
            };
            

            foreach(var i in ItemDataList)
            {
                var k = actions.FirstOrDefault(x => x.Key == i.Type).Key;


                if(k == null)
                {
                    Console.WriteLine("[Ошибка]: Не найдено действие для предмета " + i.Type + " (" + k + ")");
                    continue;
                }

                i.Action = actions.First(x => k == x.Key).Value;
            }
            foreach (var i in ItemDataList)
            {
                Console.WriteLine(i.ToString());
            }
        }

        private static void UnPackDataFromJson()
        {
            var rawJson = File.ReadAllText(@"dotnet/itemData.json");

            var j = JsonConvert.DeserializeObject<Dictionary<string, ItemData>>(rawJson);


            foreach (var r in j)
            {
                ItemData t = r.Value;
                ItemData.ItemDataList.Add(new ItemData(r.Key, t.MaxItemsInStack, t.Group, t.Weight));
            }
        }
    }
    /*
    public class ItemDatad
    {
        public static List<ItemData> ItemDataList = new List<ItemData>();

        [JsonIgnore]
        public string ItemType { get; private set; }
        [JsonProperty("ItemImg")]
        public string ItemImg { get; private set; }
        [JsonProperty("ItemStack")]
        public int ItemStack { get; private set; }
        [JsonProperty("ItemGroup")]
        public string ItemGroup { get; private set; }
        [JsonProperty("ItemName")]
        public string ItemName { get; private set; }
        [JsonProperty("ItemDescription")]
        public string ItemDescription { get; private set; }
        [JsonProperty("ItemWeigth")]
        public int ItemWeigth { get; private set; }

        private ItemDatad() { }
        public ItemDatad(string type, string img, int stack, string group, string name, string desc, int weight)
        {
            this.ItemType = type;
            this.ItemImg = img;
            this.ItemStack = stack;
            this.ItemGroup = group;
            this.ItemName = name;
            this.ItemDescription = desc;
            this.ItemWeigth = weight;
        }

        public override string ToString()
        {
            return "ItemType: " + ItemType + " | ItemImg: " + ItemImg + " | ItemStack: " + ItemStack + " | ItemGroup: " + ItemGroup + " | ItemName: " + ItemName + " | ItemDesc: " + ItemDescription + " | ItemWeight: " + ItemWeigth;
        }
    }

    public class ItemZalupa
    {
        public static void UnPackJson(string json)
        {
            var j = JsonConvert.DeserializeObject<Dictionary<string, ItemData>>(json);

            foreach (var r in j)
            {
                ItemData t = r.Value;
                ItemData.ItemDataList.Add(new ItemDatad(r.Key, t.ItemImg, t.ItemStack, t.ItemGroup, t.ItemName, t.ItemDescription, t.ItemWeigth));
            }
        }

        public static void PackJson()
        {
            Dictionary<string, ItemData> kp = new Dictionary<string, ItemData>();

            foreach (var i in ItemData.ItemDataList)
            {
                kp.Add(i.ItemType, i);
            }

            var ser = JsonConvert.SerializeObject(kp);

            System.IO.File.WriteAllText(PathData.fileName, ser);
        }

        public static void CreateItem(Label label, string type, string img, int stack, string group, string name, string desc, int weigth)
        {
            ItemData item = new ItemData(type, img, stack, group, name, desc, weigth);
            ItemData.ItemDataList.Add(item);

            label.Text = "Объект успешно добавлен!";

            PackJson();
        }
    }
    */
}
