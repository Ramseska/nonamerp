using System;
using GTANetworkAPI;
using AngleSharp;
using System.Linq;
using server_side.Data;

namespace server_side.Utilities
{
    class UtilityFuncs : Script
    {
        static public Vector3 GetPosFrontOfPlayer(Player client, double distantion)
        {
            double heading = client.Rotation.Z * Math.PI / 180;
            double x = client.Position.X + (distantion * Math.Sin(-heading));
            double y = client.Position.Y + (distantion * Math.Cos(-heading));
            return new Vector3(x,y,client.Position.Z);
        }

        static public void SendPlayerNotify(Player client, int type, string content, string sendername = null) => NAPI.ClientEvent.TriggerClientEvent(client, "pushNotify", type, content, sendername);

        static public void UpdatePlayerHud(Player player)
        {
            PlayerInfo playerInfo = new PlayerInfo(player);

            NAPI.ClientEvent.TriggerClientEvent(player, "updateHud", playerInfo.GetSatiety(), playerInfo.GetThirst(), playerInfo.GetMoney(), playerInfo.GetBankMoney());

            // player.SendChatMessage($">> UpdateHud: {playerInfo.GetSatiety()} {playerInfo.GetThirst()} {playerInfo.GetMoney()} {playerInfo.GetBankMoney()}");
        }
    }
}
