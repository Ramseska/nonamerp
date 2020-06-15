using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace server_side.Utilities
{
    abstract class Area
    {
        public string AreaID { get; private set; }
        private static List<Area> ListArea = new List<Area>();

        public delegate void AccountHandler(Entity entity);
        public static event AccountHandler OnPlayerEnterInArea;
    }

    class CircleArea : Area
    {
        public void CreateArea(float xMin, float yMin, float xMax, float yMax)
        {

        }

        public void DestroyArea()
        {

        }

        public void IsInArea(Entity entity)
        {
            
        }
    }

    class QuadArea : Area
    {
        public void CreateArea()
        {

        }

        public void DestroyArea()
        {

        }
    }
}
