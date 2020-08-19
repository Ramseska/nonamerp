using System;
using System.Linq;
using System.IO;
using GTANetworkAPI;
using server_side.Utils;
using System.Threading.Tasks;
using server_side.Items;
using System.Security.Policy;
using server_side.Data;
using System.Collections.Generic;

namespace server_side.Commands
{
    class MainCommands : Script
    { 
        [Command("lbc")]
        void CMD_lbc(Player player)
        {
            new server_side.InventorySystem.LootBag().Create(player);

            Console.WriteLine("LootBag created!");
        }

        [Command("idt")]
        void CMD_itd(Player player)
        {
            foreach (var i in ItemData.ItemDataList)
                Console.WriteLine(i.ToString());
        }
        [Command("sethp")]
        public void CMD_sethp(Player player, int hp)
        {
            new PlayerInfo(player).SetHealth(hp);
            UtilityFuncs.SendPlayerNotify(player, 2, $"Ваше здоровье было изменено на {hp}");
        }
        [Command("useitem")]
        public void CMD_useitem(Player player, int itemid)
        {
            ItemEntity item = ItemController.ItemsList.Where(x => x.ItemID == itemid).FirstOrDefault();
            if(item == null)
            {
                player.SendChatMessage($"{Utils.Colors.RED}[Ошибка]:{Utils.Colors.WHITE} предмет не найден!");
                return;
            }

            new ItemController().UseItem(player, item);
        }
        [Command("gitem")]
        public void CMD_gitem(Player player, string itemType, int amount)
        {
            if (amount < 1)
            {
                player.SendChatMessage($"{Utils.Colors.RED}[Ошибка]:{Utils.Colors.WHITE} Неверное кол-во предмета!");
                return;
            }
            else if (ItemData.ItemDataList.Where(x => x.Type == itemType).FirstOrDefault() == null)
            {
                player.SendChatMessage($"{Utils.Colors.RED}[Ошибка]:{Utils.Colors.WHITE} Неверный тип предмета!");
                return;
            }

            new ItemController().GivePlayerItem(player, itemType, amount);
        }
        [Command("inv")]
        public void CMD_inv(Player player)
        {
            List<ItemEntity> items = ItemController.ItemsList.Where(x => x.OwnerID == new PlayerInfo(player).GetDbID()).ToList();
            foreach (var i in items)
                player.SendChatMessage(i.ToString());
        }

        [Command("sp", GreedyArg = true)]
        public void CMD_sp(Player client, string namepos = null)
        {
            string text;
            Vector3 pos = !client.IsInVehicle ? client.Position : client.Vehicle.Position;
            float rot = !client.IsInVehicle ? client.Rotation.Z : client.Vehicle.Rotation.Z;

            text = client.IsInVehicle ? "[Veh]: " : "[OnF]: ";
            text = text + Math.Round(pos.X, 4).ToString().Replace(",", ".") + ", " + Math.Round(pos.Y, 4).ToString().Replace(",", ".") + ", " + Math.Round(pos.Z, 4).ToString().Replace(",", ".") + " : " + Math.Round(rot, 4).ToString().Replace(",", ".");
            if (namepos != null) 
                text = text + " | " + namepos;

            using(var s = File.AppendText("savepos.txt"))
                s.WriteLine(text);

            client.SendChatMessage($"Saved: {text}");
        }
        [Command("gtbm")]
        public void CMD_gbm(Player client)
        {
            Data.PlayerInfo playerInfo = new Data.PlayerInfo(client);
            client.SendChatMessage($"Now: {playerInfo.GetBankMoney()} in bank.");
        }
        [Command("gbm")]
        public void CMD_gbm(Player client, double money)
        {
            Data.PlayerInfo playerInfo = new Data.PlayerInfo(client);
            playerInfo.GiveBankMoney(money);
            client.SendChatMessage($"+{money} in bank. Now: {playerInfo.GetBankMoney()} in bank.");
        }
        [Command("givemoney")]
        public void CMD_givemoney(Player client, double money)
        {
            Data.PlayerInfo playerInfo = new Data.PlayerInfo(client);
            playerInfo.GiveMoney(money);

            string msg = money < 0 ? "" : "+";
            UtilityFuncs.SendPlayerNotify(client, 0, $"{msg}{money}$");
        }

        [Command("getmoney")]
        public void CMD_getmoney(Player client)
        {
            client.SendChatMessage($"Money: {new Data.PlayerInfo(client).GetMoney()}");
        }
        [Command("goto")]
        public void CMD_goto(Player client, int playerid)
        {
            if (playerid < 0 || playerid >= NAPI.Server.GetMaxPlayers())
            {
                client.SendChatMessage("Неверный ID"); 
                return;
            }
            Player player = NAPI.Pools.GetAllPlayers().Where(p => p.Value == playerid).FirstOrDefault();
            if (player == null)
            {
                client.SendChatMessage($"Игрок с {playerid} ID не найден!"); 
                return;
            }
            else if(NAPI.Player.IsPlayerDead(player))
            {
                client.SendChatMessage("Игрок не заспавнен!");
                return;
            }

            client.Position = UtilityFuncs.GetPosFrontOfPlayer(player, 1.0);
            client.Dimension = player.Dimension;

            client.SendChatMessage($"Вы успешно телепортировались к игроку {new Data.PlayerInfo(player).GetName()}[{playerid}]");
        }
        [Command("gethere")]
        public void CMD_gethere(Player client, int playerid)
        {
            if (playerid < 0 || playerid >= NAPI.Server.GetMaxPlayers())
            {
                client.SendChatMessage("Неверный ID");
                return;
            }
            Player player = NAPI.Pools.GetAllPlayers().Where(p => p.Value == playerid).FirstOrDefault();
            if (player == null)
            {
                client.SendChatMessage($"Игрок с {playerid} ID не найден!");
                return;
            }
            else if (NAPI.Player.IsPlayerDead(player))
            {
                client.SendChatMessage("Игрок не заспавнен!");
                return;
            }

            player.Position = UtilityFuncs.GetPosFrontOfPlayer(client, 1.0);
            player.Dimension = client.Dimension;

            client.SendChatMessage($"Вы успешно телепортировались к себе игрока {new Data.PlayerInfo(player).GetName()}[{playerid}]");
        }
        [Command("settime")]
        public void CMD_settime(Player client, int hour)
        {
            NAPI.World.SetTime(hour, 0, 0);
            client.SendChatMessage($"Время изменено на {hour} часов");
        }
        [Command("setweather")]
        public void CMD_setweather(Player player, string weathername)
        {
            NAPI.World.SetWeather(weathername);
            player.SendChatMessage($"Погода изменена на {weathername}");
        }
        [Command("stopsound")]
        public void CMD_stopsound(Player client)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "sound:cancel");
        }
        [Command("playsound")]
        public void CMD_playsound(Player client, string soundname)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "sound:play", soundname);
        }
        [Command("hp")]
        public void CMD_hp(Player client)
        {
            if(client.IsInVehicle)
                client.Vehicle.Repair();
            new PlayerInfo(client).SetHealth(100);
        }
        [Command("mcd")]
        public void CMD_mcd(Player client)
        {
            if (client.Vehicle == null)
            {
                client.SendChatMessage($"On foot::\tX: {client.Position.X} | Y: {client.Position.Y} | Z: {client.Position.Z} | R: {client.Rotation.Z}");
                NAPI.Util.ConsoleOutput($"On foot::{client.Position.X}, {client.Position.Y}, {client.Position.Z}, {client.Rotation.Z}");
            }
            else
            {
                client.SendChatMessage($"In Vehicle::\tX: {client.Vehicle.Position.X} | Y: {client.Vehicle.Position.Y} | Z: {client.Vehicle.Position.Z} | R: {client.Vehicle.Rotation.Z}");
                NAPI.Util.ConsoleOutput($"In Vehicle::{client.Vehicle.Position.X}, {client.Vehicle.Position.Y}, {client.Vehicle.Position.Z}, {client.Vehicle.Rotation.Z}");
            }
        }
        [Command("tpc")]
        public void CMD_tpc(Player client, float x, float y, float z)
        {
            client.Position = new Vector3(x,y,z);
        }

        [Command("veh")]
        public void CMD_veh(Player player, string vehname)
        {
            if (player.HasData("admin_car"))
            {
                player.GetData<Vehicle>("admin_car").Delete();
                player.ResetData("admin_car");
            }

            Vehicle veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(vehname), UtilityFuncs.GetPosFrontOfPlayer(player, 3.0), player.Rotation.Z / 2, new Random().Next(0, 255), new Random().Next(0, 255), numberPlate: "Admin");
            veh.NumberPlate = "Admin";

            player.SetData<Vehicle>("admin_car", veh);
            veh.SetData<bool>("temp_vehicle", true);
        }
        [Command("delv")]
        public void CMD_delv(Player player)
        {
            if(player.IsInVehicle)
            {
                if (player.GetData<Vehicle>("admin_car") == player.Vehicle)
                {
                    player.Vehicle.Delete();
                    player.ResetData("admin_car");
                }
                else
                {
                    player.SendChatMessage("Вы не можете удалить данный транспорт!");
                    return;
                }
            }
            else
            {
                player.SendChatMessage("Вы не в машине!");
            }
        }
    }
}
