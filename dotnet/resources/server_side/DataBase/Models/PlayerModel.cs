using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace server_side.DataBase.Models
{
    class PlayerModel
    {
        [Key]
        public int Id { get; set; }
        public string SClubName { get; set; }
        public ulong SClubId { get; set; }
        public string Serial { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string RegIP { get; set; }
        public string LastIP { get; set; }
        public string DateReg { get; set; }
        public string DateLastJoin { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public int LVL { get; set; }
        public int Age { get; set; }
        public int Satiety { get; set; } = 90;
        public int Thirst { get; set; } = 90;
        public int Health { get; set; } = 100;
        public double Cash { get; set; } = 0.0;
        public double Bank { get; set; } = 0.0;
        public double PayCheck { get; set; } = 0.0;
        public JsonObject<object> Customize { get; set; }
        public JsonObject<object> Clothes { get; set; }
        public int House { get; set; }
    }
}
