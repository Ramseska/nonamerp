using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using server_side.DBConnection;
using System.Linq;
using System.Threading;
using server_side.Commands;
using System.Threading.Tasks;
using server_side.Data;
using server_side.Utilities;

namespace Main.events
{
    class Events : Script
    {
        [ServerEvent(Event.PlayerEnterVehicle)]
        async public void Event_PlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatID)
        {
            await Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "createSpeedo");
            });
        }
        [ServerEvent(Event.PlayerExitVehicle)]
        async public void Event_PlayerExitVehicle(Client player, Vehicle vehicle)
        {
            await Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "destroySpeedo");
            });
        }
        [ServerEvent(Event.ResourceStart)]
        async public void Event_OnResourceStart()
        {   
            await Task.Run(() =>
            {
                DateTime time = DateTime.Now;
                NAPI.World.SetTime(time.Hour, time.Minute, time.Second); // set current time
            });

            NAPI.Server.SetGlobalServerChat(false); // disable default global chat
            UtilityFuncs.SetCurrentWeatherInLA(); // set current weather how in real LA 

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server is loaded!\n");
            Console.ResetColor();
        }

        [ServerEvent(Event.PlayerConnected)]
        async public void Event_PlayerConnected(Client client)
        {
            await Task.Run(() => 
            {
                PlayerInfo player = new PlayerInfo(client);
                player.SetDataToDefault(); // reset player data

                client.Position = new Vector3(1789.294, -244.4794, 291.7196);
                client.Rotation.Z = 353.7821f;

                NAPI.Entity.SetEntityTransparency(client, 0);

                client.Dimension = Convert.ToUInt16("1234" + Convert.ToString(client.Value));

                NAPI.ClientEvent.TriggerClientEvent(client, "CreateAuthWindow");
            });
        }

        [ServerEvent(Event.ChatMessage)]
        public void Event_ChatMessage(Client client, string text)
        {
            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                double dist = Math.Sqrt(Math.Pow((p.Position.X - client.Position.X), 2) + Math.Pow((p.Position.Y - client.Position.Y), 2) + Math.Pow((p.Position.Z - client.Position.Z), 2));
                if (dist <= 10) p.SendChatMessage("- " + text + $" ({client.Name})[{client.Value}]");
            });
        }
    }
}
