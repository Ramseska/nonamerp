using System;
using GTANetworkAPI;

namespace server_side.Commands
{
    class MainCommands : Script
    {
        [Command("stopsound")]
        public void CMD_stopsound(Client client)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "sound:cancel");
        }
        [Command("playsound")]
        public void CMD_playsound(Client client, string soundname)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "sound:play", soundname);
        }
        [Command("hp")]
        public void CMD_hp(Client client)
        {
            if(client.IsInVehicle)
                client.Vehicle.Repair();
            client.Health = 100;
        }
        [Command("weap")]
        public void CMD_weap(Client client, string weapon_name)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "awfovgbawawgt", client, "weapon_"+weapon_name, 999999999);
        }
        [Command("mcd")]
        public void CMD_mcd(Client client)
        {
            if (client.Vehicle == null)
            {
                client.SendChatMessage($"On foot::\tX: {client.Position.X} | Y: {client.Position.Y} | Z: {client.Position.Z} | R: {client.Rotation.Z}");
                Console.WriteLine($"On foot::{client.Position.X}, {client.Position.Y}, {client.Position.Z}, {client.Rotation.Z}");
            }
            else
            {
                client.SendChatMessage($"In Vehicle::\tX: {client.Vehicle.Position.X} | Y: {client.Vehicle.Position.Y} | Z: {client.Vehicle.Position.Z} | R: {client.Vehicle.Rotation.Z}");
                Console.WriteLine($"In Vehicle::{client.Vehicle.Position.X}, {client.Vehicle.Position.Y}, {client.Vehicle.Position.Z}, {client.Vehicle.Rotation.Z}");
            }
        }
        [Command("tpc")]
        public void CMD_tpc(Client client, float x, float y, float z)
        {
            client.Position = new Vector3(x,y,z);
        }
        [Command("veh")]
        public void CMD_veh(Client client, string vehicle_name)
        {
            if (client.HasData("temp_vehicle"))
            {
                client.GetData("temp_vehicle").Delete();
                client.ResetData("temp_vehicle");
            }
                
            Random rand = new Random();
            byte clr = (byte)rand.Next(0, 255);

            var veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(vehicle_name), client.Position.Around(2f), client.Rotation.Z, clr, clr, "admin");

            veh.SetData("type", "admin");

            veh.EngineStatus = false;

            client.SetData("temp_vehicle", veh);
        }
        [Command("delv")]
        public void CMD_delv(Client client)
        {
            if (client.HasData("temp_vehicle"))
            {
                client.GetData("temp_vehicle").ResetData("type");
                client.GetData("temp_vehicle").Delete();
            }

            else if (client.Vehicle != null && client.Vehicle.GetData("type") == "admin")
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
