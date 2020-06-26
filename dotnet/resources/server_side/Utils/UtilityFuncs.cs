using System;
using GTANetworkAPI;
using server_side.Data;

namespace server_side.Utils
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

        /// <summary>
        /// Типы уведомлений:<br/><br/>
        /// <item>
        /// <term>0</term>
        /// <description>Обычное уведомление</description>
        /// </item><br/>
        /// <item>
        /// <term>1</term>
        /// <description>Сообщение от игрока</description>
        /// </item><br/>
        /// <item>
        /// <term>2</term>
        /// <description>Информация о чем-либо</description>
        /// </item>
        /// </summary>
        static public void SendPlayerNotify(Player client, int type, string content, string sendername = null) => NAPI.ClientEvent.TriggerClientEvent(client, "pushNotify", type, content, sendername);

        static public void UpdatePlayerHud(Player player)
        {
            PlayerInfo playerInfo = new PlayerInfo(player);

            NAPI.ClientEvent.TriggerClientEvent(player, "updateHud", playerInfo.GetSatiety(), playerInfo.GetThirst(), playerInfo.GetMoney(), playerInfo.GetBankMoney());
        }

        static public float GetDistToPoint3D(Vector3 from, Vector3 to) => (float)Math.Abs(Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2) + Math.Pow(to.Z - from.Z, 2)));
        static public float GetDistToPoint2D(Vector3 from, Vector3 to) => (float)Math.Abs(Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2)));
    }
}
