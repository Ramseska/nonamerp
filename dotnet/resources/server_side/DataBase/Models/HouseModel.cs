using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GTANetworkAPI;

namespace server_side.DataBase.Models
{
    class HouseModel : Script
    {
        [Key]
        public int Id { get; set; }
        public int Class { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public int Days { get; set; } = 0;
        public bool Doors { get; set; } = false;
        public string Owner { get; set; } = "None";
        public float EnterPointX { get; set; }
        public float EnterPointY { get; set; }
        public float EnterPointZ { get; set; }
        public float EnterRotation { get; set; }
        public bool Status { get; set; } = false;
        public int Interior { get; set; }
    }
}
