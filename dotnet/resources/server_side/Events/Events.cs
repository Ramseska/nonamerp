using System;
using GTANetworkAPI;
using System.Threading.Tasks;
using server_side.Data;
using server_side.Ints;
using server_side.Systems;
using server_side.Utils;
using server_side.Jobs;
using System.Linq;
using MySql.Data.MySqlClient;
using server_side.DBConnection;
using server_side.Items;

namespace Main.events
{
    class Events : Script
    {
        [ServerEvent(Event.PlayerEnterVehicle)]
        public void Event_PlayerEnterVehicle(Player player, Vehicle vehicle, sbyte seatID)
        {
            // NAPI.ClientEvent.TriggerClientEvent(player, "createSpeedo");
        }
        [ServerEvent(Event.PlayerExitVehicle)]
        public void Event_PlayerExitVehicle(Player player, Vehicle vehicle)
        {
            // NAPI.ClientEvent.TriggerClientEvent(player, "destroySpeedo");
        }

        [ServerEvent(Event.ResourceStart)]
        public void Event_OnResourceStart()
        {
            // ints
            Interiors.InitInteriors();
            
            // houses
            House.InitHouses();

            // jobs
            Job.InitJobs();

            // items
            ItemData.InitItemData();
            
            /*
            DateTime time = DateTime.Now;
            NAPI.World.SetTime(time.Hour, time.Minute, time.Second); // set current time
            */

            NAPI.Server.SetGlobalServerChat(true); // disable default global chat
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            //RealWeather.SetCurrentWeatherInLA(); // set current weather how in real LA 
            NAPI.Server.SetCommandErrorMessage("~r~[Ошибка]: ~w~Данной команды не существует!");
            NAPI.Server.SetDefaultSpawnLocation(new Vector3(1789.294, -244.4794, 291.7196), 353.7821f);
            //
            NAPI.Server.SetLogCommandParamParserExceptions(true);
            NAPI.Server.SetLogRemoteEventParamParserExceptions(true);

            Console.ForegroundColor = ConsoleColor.Green;
            NAPI.Util.ConsoleOutput("\nServer is loaded! (HI)\n");
            Console.ResetColor();
        }

        [ServerEvent(Event.PlayerConnected)]
        public void Event_PlayerConnected(Player player)
        {
            try
            {
                PlayerInfo playerInfo = new PlayerInfo(player);

                playerInfo.SetDataToDefault(); // reset player data
                playerInfo.SetSocialClub(player.SocialClubName);

                player.Position = new Vector3(1789.294, -244.4794, 291.7196);
                player.Rotation = new Vector3(player.Rotation.X, player.Rotation.Y, 353.7821f);
                NAPI.Entity.SetEntityTransparency(player, 0);

                player.Dimension = Convert.ToUInt16("1234" + Convert.ToString(player.Value));

                NAPI.ClientEvent.TriggerClientEvent(player, "createAuthBrowser");

                NAPI.Util.ConsoleOutput($"Connected: {NAPI.Player.GetPlayerAddress(player)} | ID: {player.Value}");

                //NAPI.ClientEvent.TriggerClientEvent(player, "createDebugUI");
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void Event_PlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            // unload player items from server
            new ItemController().UnloadPlayerItems(new PlayerInfo(player).GetDbID());
        }
        /*
        [ServerEvent(Event.ChatMessage)]
        public void Event_ChatMessage(Player client, string text)
        {
            
            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                double dist = Math.Sqrt(Math.Pow(p.Position.X - client.Position.X, 2) + Math.Pow(p.Position.Y - client.Position.Y, 2) + Math.Pow(p.Position.Z - client.Position.Z, 2));
                if (dist <= 10) p.SendChatMessage("- " + text + $" ({client.Name})[{client.Value}]");
            });
            
            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                p.SendChatMessage("- " + text + $" ({client.Name})[{client.Value}]");
            });
        }
        */

        [ServerEvent(Event.PlayerDeath)]
        public void Event_PlayerDeath(Player client, Player killer, uint reason)
        {
            Random rand = new Random();
            Vector3[] respawnPositions =
            {
                new Vector3(258.9378f, -1340.669f, 24.53781f),
                new Vector3(250.3403f, -1351.37f, 24.53782f),
                new Vector3(251.3651f, -1347.497f, 24.53782f)
            };
            float[] rots = { 176.6944f, 250.9094f, 137.5093f };

            NAPI.Task.Run(() =>
            {
                int randVal = rand.Next(0, respawnPositions.Length);
                NAPI.Player.SpawnPlayer(client, respawnPositions[randVal], rots[randVal]);
                client.Dimension = 1;
            }, 5000);
        }
        [ServerEvent(Event.PlayerEnterColshape)]
        public void Event_PlayerEnterColshape(ColShape colshape, Player player)
        {
            if (player.GetData<int>(EntityData.PLAYER_PICKUPKD) != 0) return;
            player.SetData<int>(EntityData.PLAYER_PICKUPKD, player.GetData<int>(EntityData.PLAYER_PICKUPKD) + 5);

            // there another "EnterColShape" events:
            Interiors.OnPlayerEnterConshape(colshape, player);
            House.OnPlayerEnterColshape(colshape, player);
            Job.OnEnterJobPickUp(colshape, player);
            server_side.InventorySystem.LootBag.EnterLootBagColShape(colshape, player);
        }
        [ServerEvent(Event.PlayerExitColshape)]
        public void OnPlayerExitColShape(ColShape colshape, Player player)
        {
            server_side.InventorySystem.LootBag.ExitLootBagColShape(colshape, player);
        }
        /*
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            switch (type)
            {
                case DisconnectionType.Left:
                    NAPI.Util.ConsoleOutput($"{player.Name} has left from server.");
                    break;

                case DisconnectionType.Timeout:
                    NAPI.Util.ConsoleOutput($"{player.Name} has timed out.");
                    break;

                case DisconnectionType.Kicked:
                    NAPI.Util.ConsoleOutput($"{player.Name} has been kicked for {reason}");
                    break;
            }

            Job.IsLeaveOnJob(player);
        }
        */
    }
}
