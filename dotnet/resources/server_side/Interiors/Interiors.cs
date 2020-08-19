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
        private Blip InterBlip;
        public uint Dimension;


        private Interiors() {}
        public Interiors(Vector3 enterPos, Vector3 exitPos, float enterRot, float exitRot, uint dimension, Blip blip = null)
        {
            EnterPointCoords = enterPos;
            ExitPointCoords = exitPos;
            EnterPointRotation = enterRot;
            ExitPointRotation = exitRot;
            InterBlip = blip;
            Dimension = dimension;
        }

        public static void InitInteriors()
        {
            new Interiors(new Vector3(1839.098f, 3673.332f, 34.2767f), new Vector3(275.9121, -1361.429, 24.5378), 211.3162f, 51.81643f, 1, NAPI.Blip.CreateBlip(61, new Vector3(343.0853f, -1399.852f, 32.5092f), 1f, 0, name: "Hospital", drawDistance: 15.0f, shortRange: true, dimension: 0)).CreateInterior();
        }

        public void CreateInterior()
        {
            ColShape[] shapes =
            {
                NAPI.ColShape.CreateCylinderColShape(this.EnterPointCoords, 1f, 2f, 0),
                NAPI.ColShape.CreateCylinderColShape(this.ExitPointCoords, 1f, 2f, this.Dimension),
            };
            
            foreach(var i in shapes) i.SetData("InteriorsColShape", true);

            this.EnterMarker = NAPI.Marker.CreateMarker(0, this.EnterPointCoords, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1f, new Color(255, 239, 185), false, 0);
            this.ExitMarker = NAPI.Marker.CreateMarker(0, this.ExitPointCoords, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1f, new Color(255, 239, 185), false, this.Dimension);
            this.EnterColShape = shapes[0];
            this.ExitColShape = shapes[1];

            InteriorsList.Add(this);
        }

        public static void OnPlayerEnterConshape(ColShape shape, Player client)
        {
            if (!shape.HasData("InteriorsColShape")) return;

            Interiors inter = InteriorsList.Where(x => x.EnterColShape == shape || x.ExitColShape == shape).First();

            if (shape == inter.EnterColShape)
            {
                client.Position = inter.ExitPointCoords;
                client.Rotation = new Vector3(0f, 0f, inter.ExitPointRotation);
                client.Dimension = inter.Dimension;
            }
            else if (shape == inter.ExitColShape)
            {
                client.Position = inter.EnterPointCoords;
                client.Rotation = new Vector3(0f, 0f, inter.EnterPointRotation);
                client.Dimension = 0;
            }
        }
    }
}
