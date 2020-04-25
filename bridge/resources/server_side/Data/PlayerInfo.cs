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
            player.SetData(EntityData.PLAYER_AUTHORIZED, false);
            player.SetData(EntityData.PLAYER_DBID, -1);
            player.SetData(EntityData.PLAYER_LOGIN, string.Empty);
            player.SetData(EntityData.PLAYER_PASSWORD, string.Empty);
            player.SetData(EntityData.PLAYER_MAIL, string.Empty);
            player.SetData(EntityData.PLAYER_SOCIAL, string.Empty);
            player.SetData(EntityData.PLAYER_IP, "0.0.0.0");
            player.SetData(EntityData.PLAYER_REGIP, "0.0.0.0");
            player.SetData(EntityData.PLAYER_NAME, string.Empty);
            player.SetData(EntityData.PLAYER_LVL, -1);
            player.SetData(EntityData.PLAYER_AGE, -1);
            player.SetData(EntityData.PLAYER_PICKUPKD, 0);
            player.SetData(EntityData.PLAYER_MONEY, 0.00);
            player.SetData(EntityData.PLAYER_BANK, 0.00);
            player.SetData(EntityData.PLAYER_HOUSE, -1);
            player.SetData(EntityData.PLAYER_CUSTOMIZE, null);
            player.SetData(EntityData.PLAYER_CLOTHES, null);
        }

        public void SetAuthorized(bool status) => player.SetData(EntityData.PLAYER_AUTHORIZED, status);
        public bool GetAuthorized() => player.GetData(EntityData.PLAYER_AUTHORIZED);

        public void SetDbID(int id) => player.SetData(EntityData.PLAYER_DBID, id);
        public int GetDbID() => player.GetData(EntityData.PLAYER_DBID);

        public void SetLogin(string login) => player.SetData(EntityData.PLAYER_LOGIN, login);
        public string GetLogin()
        {
            if (!string.IsNullOrEmpty(player.GetData(EntityData.PLAYER_LOGIN)))
                return player.GetData(EntityData.PLAYER_LOGIN);

            return "Undefined";
        }

        public void SetPassword(string password) => player.SetData(EntityData.PLAYER_PASSWORD, password);
        public string GetPassword()
        {
            if (!string.IsNullOrEmpty(player.GetData(EntityData.PLAYER_PASSWORD)))
                return player.GetData(EntityData.PLAYER_PASSWORD);

            return "Undefined";
        }

        public void SetMail(string mail) => player.SetData(EntityData.PLAYER_MAIL, mail);
        public string GetMail()
        {
            if (!string.IsNullOrEmpty(player.GetData(EntityData.PLAYER_MAIL)))
                return player.GetData(EntityData.PLAYER_MAIL);

            return "Undefined";
        }

        public void SetSocialClub(string clubName) => player.SetData(EntityData.PLAYER_SOCIAL, clubName);
        public string GetSocialClub()
        {
            if (!string.IsNullOrEmpty(player.GetData(EntityData.PLAYER_SOCIAL)))
                return player.GetData(EntityData.PLAYER_SOCIAL);

            return "Undefined";
        }

        public void SetCurrentIP(string ip) => player.SetData(EntityData.PLAYER_IP, ip);
        public string GetCurrentIP() => player.GetData(EntityData.PLAYER_IP);

        public void SetRegIP(string ip) => player.SetData(EntityData.PLAYER_REGIP, ip);
        public string GetRegIP() => player.GetData(EntityData.PLAYER_REGIP);

        public void SetName(string name) => player.SetData(EntityData.PLAYER_NAME, name);
        public string GetName()
        {
            if (!string.IsNullOrEmpty(player.GetData(EntityData.PLAYER_NAME)))
                return player.GetData(EntityData.PLAYER_NAME);

            return "Undefined";
        }

        public void SetLVL(int lvl) => player.SetData(EntityData.PLAYER_LVL, lvl);
        public int GetLVL() => player.GetData(EntityData.PLAYER_LVL);

        public int GetAge() => player.GetData(EntityData.PLAYER_AGE);
        public void SetAge(int age) => player.SetData(EntityData.PLAYER_AGE, age);

        async public void GiveMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData(EntityData.PLAYER_MONEY, Math.Round(money, 2) + (player.GetData(EntityData.PLAYER_MONEY)));
            if (updateindb)
            {
                await Task.Run(() =>
                {
                    string query = $"UPDATE `accounts` SET `p_money` = '{Convert.ToString(GetMoney()).Replace(',', '.')}' WHERE `p_login` = '{GetLogin()}'";

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
        public double GetMoney() => Math.Round(player.GetData(EntityData.PLAYER_MONEY), 2);

        async public void GiveBankMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData(EntityData.PLAYER_BANK, Math.Round(money, 2) + (player.GetData(EntityData.PLAYER_BANK)));

            if (updateindb)
            {
                await Task.Run(() =>
                {
                    string query = $"UPDATE `accounts` SET `p_bank` = '{Convert.ToString(GetBankMoney()).Replace(',', '.')}' WHERE `p_login` = '{GetLogin()}'";

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
        public double GetBankMoney() => Math.Round(player.GetData(EntityData.PLAYER_BANK), 2);

        public void SetHouse(int houseid) => player.SetData(EntityData.PLAYER_HOUSE, houseid);
        public int GetHouse() => player.GetData(EntityData.PLAYER_HOUSE);

        public void SetCustomize(object args) => player.SetData(EntityData.PLAYER_CUSTOMIZE, args);
        public object GetCustomize() => player.GetData(EntityData.PLAYER_CUSTOMIZE);

        public void SetClothes(object args) => player.SetData(EntityData.PLAYER_CLOTHES, args);
        public object GetClothes() => player.GetData(EntityData.PLAYER_CLOTHES);
    }
}
