using System;
using System.Linq;
using System.IO;
using GTANetworkAPI;
using server_side.Utilities;
using System.Threading.Tasks;

namespace server_side.Commands
{
    class MainCommands : Script
    {
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
        [Command("gm")]
        public void CMD_gm(Player client)
        {
            client.SendChatMessage("Undefined");
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
        void CMD_givemoney(Player client, double money)
        {
            Data.PlayerInfo playerInfo = new Data.PlayerInfo(client);
            playerInfo.GiveMoney(money);

            string msg = money < 0 ? "" : "+";
            UtilityFuncs.SendPlayerNotify(client, 0, $"{msg}{money}$");
        }

        [Command("getmoney")]
        void CMD_getmoney(Player client)
        {
            client.SendChatMessage($"Money: {new Data.PlayerInfo(client).GetMoney()}");
        }
        [Command("goto")]
        void CMD_goto(Player client, int playerid)
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
        void CMD_gethere(Player client, int playerid)
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
            client.Health = 100;
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
        public void CMD_veh(Player client, string vehicle_name)
        {
            if (client.HasData("temp_vehicle"))
            {
                client.ResetData("temp_vehicle");
            }
                
            Random rand = new Random();
            byte clr = (byte)rand.Next(0, 255);

            Vehicle veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(vehicle_name), UtilityFuncs.GetPosFrontOfPlayer(client, 3.0), client.Rotation.Z / 2, clr, clr, numberPlate: "Admin");
            veh.NumberPlate = "Admin";

            veh.SetData<string>("type", "admin");

            veh.EngineStatus = false;

            client.SetData<Vehicle>("temp_vehicle", veh);
        }
        [Command("delv")]
        public void CMD_delv(Player client)
        {
            if (client.HasData("temp_vehicle"))
            {
                client.GetData<Vehicle>("temp_vehicle").ResetData("type");
                client.GetData<Vehicle>("temp_vehicle").Delete();
            }

            else if (client.Vehicle != null && client.Vehicle.GetData<string>("type") == "admin")
            {
                client.Vehicle.ResetData("type");
                client.Vehicle.Delete();
            }
            else
            {
                client.SendChatMessage("Vehicle is not finded");
                return;
            }
            client.ResetData("temp_vehicle");
        }
    }
}
