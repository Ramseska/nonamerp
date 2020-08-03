using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace server_side.DataBase
{
    class PlayerModel
    {
        public int Id;
        public string SClubName;
        public string SClubId;
        public string Serial;
        public string Login;
        public string Password;
        public string Mail;
        public string RegIP;
        public string LastIP;
        public string DateReg;
        public string DateLastJoin;
        public string Name;
        public bool Sex;
        public int LVL;
        public int Age;
        public int Satiety;
        public int Thirst;
        public int Health;
        public double Cash;
        public double Bank;
        public double PayCheck;
        public JsonObject Customize;
        public JsonObject Clothes;
        public int House;
    }
}
