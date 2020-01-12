using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;
 
namespace server_side.Ints
{ 
    class Interiors : Script
    {
        static public List<Interiors> InteriorsList = new List<Interiors>();

        public Vector3 EnterPointCoords;
        public Vector3 ExitPointCoords;
        public float EnterPointRotation;
        public float ExitPointRotation;
        private Marker EnterMarker;
        private Marker ExitMarker;
        private ColShape EnterColShape;
        private ColShape ExitColShape;
        public Blip InterBlip;
        public uint Dimension;

        public static void CreateInterior(Vector3 enterPos, Vector3 exitPos, float enterRot, float exitRot, uint dimension, Blip blip = null)
        {
            InteriorsList.Add(
                new Interiors
                {
                    EnterPointCoords = enterPos,
                    ExitPointCoords = exitPos,
                    EnterPointRotation = enterRot,
                    ExitPointRotation = exitRot,
                    Dimension = dimension,
                    EnterMarker = NAPI.Marker.CreateMarker(0, enterPos, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1f, new Color(255, 239, 185), false, 0),
                    ExitMarker = NAPI.Marker.CreateMarker(0, exitPos, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1f, new Color(255, 239, 185), false, dimension),
                    EnterColShape = NAPI.ColShape.CreateCylinderColShape(enterPos, 1f, 2f, 0),
                    ExitColShape = NAPI.ColShape.CreateCylinderColShape(exitPos, 1f, 2f, dimension),
                    InterBlip = blip
                }
            );            
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        private void Event_PlayerEnterInterColShape(ColShape shape, Client client)
        {
            if (!client.HasData("PickupKD")) return;
            if (client.GetData("PickupKD") != 0) return;

            Interiors inter = InteriorsList.Where(x => x.EnterColShape == shape || x.ExitColShape == shape).First();

            if(shape == inter.EnterColShape)
            {
                client.Position = inter.ExitPointCoords;
                client.Rotation = new Vector3(0f, 0f, inter.ExitPointRotation);
                client.Dimension = inter.Dimension;
            }
            else if(shape == inter.ExitColShape)
            {
                client.Position = inter.EnterPointCoords;
                client.Rotation = new Vector3(0f, 0f, inter.EnterPointRotation);
                client.Dimension = 0;
            }

            client.SetData("PickupKD", client.GetData("PickupKD") + 5);
        }
    }
}
