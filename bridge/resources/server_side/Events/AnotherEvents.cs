using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace server_side.Events
{
    class AnotherEvents : Script
    {
        [RemoteEvent("turnVehicleEngine")]
        public void Event_TurnVehicleEngine(Client client, object[] args)
        {
            if (!client.IsInVehicle)
                return;

            client.Vehicle.EngineStatus = !client.Vehicle.EngineStatus;
        }
        [RemoteEvent("pickerChange")]
        public void Event_PickerChange(Client client, object[] colors)
        {
            if (!client.IsInVehicle)
                return;

            client.Vehicle.CustomPrimaryColor = client.Vehicle.CustomSecondaryColor = new Color(Convert.ToInt32(colors[0]), Convert.ToInt32(colors[1]), Convert.ToInt32(colors[2]));
        }
    }
}
