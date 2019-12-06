using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace server_side.Data
{
    public class PlayerInfo : Script
    {
        private Client player { get; set; }

        private PlayerInfo() { }
        public PlayerInfo(Client player) { this.player = player; }


        public void SetDataToDefault()
        {
            player.SetData("PlayerAuthorized", false); // статус авторизации
            player.SetData("PlayerDbID", -1); // ид из бд 
            player.SetData("PlayerName", "None"); // ник
            player.SetData("PlayerPassword", "None"); // пароль
            player.SetData("PlayerRegIP", "0.0.0.0"); // регистрационный ип
            player.SetData("PlayerMail", "None"); // почта
            player.SetData("PlayerLVL", 0); // lvl
            player.SetData("PlayerMoney", 0); // cash
            player.SetData("PlayerHouse", -1); // house
        }

        public void SetAuthorized(bool status) => player.SetData("PlayerAuthorized", status);
        public bool GetAuthorized()
        {
            if (!player.HasData("PlayerAuthorized"))
                return false;
            return player.GetData("PlayerAuthorized");
        }

        public void SetDbID(int id) => player.SetData("PlayerDbID", id);
        public int GetDbID()
        {
            if (!player.HasData("PlayerDbID"))
                return -1;
            return player.GetData("PlayerDbID");
        }

        public void SetName(string name) => player.SetData("PlayerName", name);
        public string GetName()
        {
            if (!player.HasData("PlayerName"))
                return "None";
            return player.GetData("PlayerName");
        }

        public void SetPassword(string password) => player.SetData("PlayerPassword", password);
        public string GetPassword()
        {
            if (!player.HasData("PlayerPassword"))
                return "None";
            return player.GetData("PlayerPassword");
        }

        public void SetRegIP(string ip) => player.SetData("PlayerRegIP", ip);
        public string GetRegIP()
        {
            if (!player.HasData("PlayerRegIP"))
                return "0.0.0.0";
            return player.GetData("PlayerRegIP");
        }

        public void SetMail(string mail) => player.SetData("PlayerMail", mail);
        public string GetMail()
        {
            if (!player.HasData("PlayerMail"))
                return "None";
            return player.GetData("PlayerMail");
        }

        public void SetLVL(int lvl) => player.SetData("PlayerLVL", lvl);
        public int GetLVL()
        {
            if (!player.HasData("PlayerLVL"))
                return 0;
            return player.GetData("PlayerLVL");
        }

        public void SetMoney(int money) => player.SetData("PlayerMoney", money);
        public int GetMoney()
        {
            if (!player.HasData("PlayerMoney"))
                return 0;
            return player.GetData("PlayerMoney");
        }
    }
}
