using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;

namespace server_side.Interiors
{
    class Interiors : Script
    {
        static public List<Interiors> InteriorsList = new List<Interiors>();

        public Vector3 EnterPointCoords;
        public Vector3 ExitPointCoords;
        public float EnterPointRotation;
        public float ExitPointRotation;
        private Checkpoint EnterCheckpoint;
        private Checkpoint ExitCheckpoint;
        public uint Dimension;

        public static void CreateInterior(Vector3 enterPos, Vector3 exitPos, float enterRot, float exitRot, uint dimension)
        {
            Checkpoint entch = NAPI.Checkpoint.CreateCheckpoint(0, enterPos, new Vector3(0, 0, 0), 1f, new Color(208, 240, 129), 0);
            Checkpoint extch = NAPI.Checkpoint.CreateCheckpoint(0, exitPos, new Vector3(0, 0, 0), 1f, new Color(208, 240, 129), dimension);

            InteriorsList.Add(
                new Interiors
                {
                    EnterPointCoords = enterPos,
                    ExitPointCoords = exitPos,
                    EnterPointRotation = enterRot,
                    ExitPointRotation = exitRot,
                    Dimension = dimension,
                    EnterCheckpoint = entch,
                    ExitCheckpoint = extch
                }
            );            
        }

        [ServerEvent(Event.PlayerEnterCheckpoint)]
        private void Event_EnterCheckpointInteriors(Checkpoint checkpoint, Client client)
        {
            Interiors inter = InteriorsList.Where(x => x.EnterCheckpoint == checkpoint || x.ExitCheckpoint == checkpoint).First();

            if(checkpoint == inter.EnterCheckpoint)
            {
                client.Position = inter.ExitPointCoords;
                client.Rotation.Z = inter.ExitPointRotation;
                client.Dimension = inter.Dimension;
            }
            else if(checkpoint == inter.ExitCheckpoint)
            {
                client.Position = inter.EnterPointCoords;
                client.Rotation.Z = inter.EnterPointRotation;
                client.Dimension = 0;
            }
        }
    }
}
