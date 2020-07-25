using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RAGE;
using System.Reflection.Metadata;

namespace server_side.Systems
{
    class Test : Script
    {
        [RemoteProc("CalledFromCef")]
        public void RemoteProc_CalledFromCef(Player player, string text)
        {
            player.SendChatMessage("Called proc from cef: " + text);
        }
    }
}
