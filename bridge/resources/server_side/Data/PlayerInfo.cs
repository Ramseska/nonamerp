using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using server_side.DBConnection;
using MySql.Data.MySqlClient;

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
            player.SetData("PlayerLogin", "None"); // логин
            player.SetData("PlayerPassword", "None"); // пароль
            player.SetData("PlayerRegIP", "0.0.0.0"); // регистрационный ип
            player.SetData("PlayerCurrentIP", "0.0.0.0"); // текущий ип
            player.SetData("PlayerName", "None"); // имя игрока
            player.SetData("PlayerMail", "None"); // почта
            player.SetData("PlayerLVL", 0); // lvl
            player.SetData("PlayerMoney", 0.00); // cash
            player.SetData("PlayerHouse", -1); // house
            player.SetData("PlayerCustomize", null); // customize player params
            player.SetData("PickupKD", 0);
            player.SetData("PlayerBankMoney", 0.00); // банковский счет
            player.SetData("PlayerAge", 0); // возраст
            player.SetData("PlayerClothes", null); // одежда
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

        public void SetLogin(string login) => player.SetData("PlayerLogin", login);
        public string GetLogin()
        {
            if (!player.HasData("PlayerLogin"))
                return "None";
            return player.GetData("PlayerLogin");
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

        async public void GiveMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData("PlayerMoney", Math.Round(money, 2) + (player.GetData("PlayerMoney")));
            if (updateindb)
            {
                await Task.Run(() =>
                {
                    string query = $"UPDATE `accounts` SET `p_money` = '{Convert.ToString(GetMoney()).Replace(',', '.')}' WHERE `p_name` = '{GetLogin()}'";

                    try
                    {
                        using (MySqlConnection con = MySqlConnector.GetDBConnection())
                        {
                            con.Open();
                            new MySqlCommand(query, con).ExecuteNonQuery();
                        }

                    }
                    catch (Exception e) { NAPI.Util.ConsoleOutput($"[MySQL Exception]: Player: {player.Name}({player.Value})\nQuery: {query}\nException: {e.ToString()}"); }

                });
            }
        }

        public double GetMoney()
        {
            if (!player.HasData("PlayerMoney"))
                return -1;
            return Math.Round(player.GetData("PlayerMoney"), 2);
        }

        async public void GiveBankMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData("PlayerBankMoney", Math.Round(money, 2) + (player.GetData("PlayerBankMoney")));

            if(updateindb)
            {
                await Task.Run(() =>
                {
                    string query = $"UPDATE `accounts` SET `p_bank` = '{Convert.ToString(GetBankMoney()).Replace(',', '.')}' WHERE `p_name` = '{GetLogin()}'";

                    try
                    {
                        using (MySqlConnection con = MySqlConnector.GetDBConnection())
                        {
                            con.Open();
                            new MySqlCommand(query, con).ExecuteNonQuery();
                        }

                    }
                    catch (Exception e) { NAPI.Util.ConsoleOutput($"[MySQL Exception]: Player: {player.Name}({player.Value})\nQuery: {query}\nException: {e.ToString()}"); }
                });
            }
        }

        public double GetBankMoney()
        {
            if (!player.HasData("PlayerBankMoney"))
                return -1;
            return Math.Round(player.GetData("PlayerBankMoney"), 2);
        }

        public int GetAge() => player.GetData("PlayerAge");
        public void SetAge(int age) => player.SetData("PlayerAge", age);

        public void SetCustomize(object args) => player.SetData("PlayerCustomize", args);
        public object GetCustomize() => player.GetData("PlayerCustomize");

        public void SetClothes(object args) => player.SetData("PlayerClothes", args);
        public object GetClothes() => player.GetData("PlayerClothes");

        public void SetName(string name) => player.SetData("PlayerName", name);
        public string GetName()
        {
            if (!player.HasData("PlayerName"))
                return "None";
            return player.GetData("PlayerName");
        }
    }
}
