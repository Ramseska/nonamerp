using System;
using GTANetworkAPI;
using server_side.DBConnection;



namespace server_side.Data
{
    public class PlayerInfo : Script
    {
        private Player player { get; set; }

        private PlayerInfo() { }
        public PlayerInfo(Player player) { this.player = player; }


        public void SetDataToDefault()
        {
            player.SetData<bool>(EntityData.PLAYER_AUTHORIZED, false);
            player.SetData<int>(EntityData.PLAYER_DBID, -1);
            player.SetData<string>(EntityData.PLAYER_LOGIN, string.Empty);
            player.SetData<string>(EntityData.PLAYER_PASSWORD, string.Empty);
            player.SetData<string>(EntityData.PLAYER_MAIL, string.Empty);
            player.SetData<string>(EntityData.PLAYER_SOCIAL, string.Empty);
            player.SetData<string>(EntityData.PLAYER_IP, "0.0.0.0");
            player.SetData<string>(EntityData.PLAYER_REGIP, "0.0.0.0");
            player.SetData<string>(EntityData.PLAYER_NAME, string.Empty);
            player.SetData<int>(EntityData.PLAYER_LVL, -1);
            player.SetData<int>(EntityData.PLAYER_AGE, -1);
            player.SetData<int>(EntityData.PLAYER_PICKUPKD, 0);
            player.SetData<double>(EntityData.PLAYER_MONEY, 0.00);
            player.SetData<double>(EntityData.PLAYER_BANK, 0.00);
            player.SetData<int>(EntityData.PLAYER_HOUSE, -1);
            player.SetData<object>(EntityData.PLAYER_CUSTOMIZE, null);
            player.SetData<object>(EntityData.PLAYER_CLOTHES, null);
            player.SetData<object>(EntityData.PLAYER_DATEREG, null);
            player.SetData<object>(EntityData.PLAYER_LASTJOIN, null);
            player.SetData<int>(EntityData.PLAYER_SATIETY, 0);
            player.SetData<int>(EntityData.PLAYER_THIRST, 0);
            player.SetData<double>(EntityData.PLAYER_PAYCHECK, 0.00);

            // unext
            player.SetData<double>(EntityData.PLAYER_JOB_SALARY, 0.0);
            player.SetData<int>(EntityData.PLAYER_JOB, 0);
            player.SetData<int>(EntityData.PLAYER_TEMPJOB, 0);
        }

        public void SetAuthorized(bool status) => player.SetData<bool>(EntityData.PLAYER_AUTHORIZED, status);
        public bool GetAuthorized() => player.GetData<bool>(EntityData.PLAYER_AUTHORIZED);

        public void SetDbID(int id) => player.SetData<int>(EntityData.PLAYER_DBID, id);
        public int GetDbID() => player.GetData<int>(EntityData.PLAYER_DBID);

        public void SetLogin(string login) => player.SetData<string>(EntityData.PLAYER_LOGIN, login);
        public string GetLogin()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_LOGIN)))
                return player.GetData<string>(EntityData.PLAYER_LOGIN);

            return "Undefined";
        }

        public void SetPassword(string password) => player.SetData<string>(EntityData.PLAYER_PASSWORD, password);
        public string GetPassword()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_PASSWORD)))
                return player.GetData<string>(EntityData.PLAYER_PASSWORD);

            return "Undefined";
        }

        public void SetMail(string mail) => player.SetData<string>(EntityData.PLAYER_MAIL, mail);
        public string GetMail()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_MAIL)))
                return player.GetData<string>(EntityData.PLAYER_MAIL);

            return "Undefined";
        }

        public void SetSocialClub(string clubName) => player.SetData<string>(EntityData.PLAYER_SOCIAL, clubName);
        public string GetSocialClub()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_SOCIAL)))
                return player.GetData<string>(EntityData.PLAYER_SOCIAL);

            return "Undefined";
        }

        public void SetCurrentIP(string ip)
        {
            new DBConnection.MySqlConnector().RequestExecuteNonQuery($"UPDATE `accounts` SET `p_lastip` = '{ip}' WHERE `p_id` = '{GetDbID()}'");
            
            player.SetData<string>(EntityData.PLAYER_IP, ip);
        }
        public string GetCurrentIP() => player.GetData<string>(EntityData.PLAYER_IP);

        public void SetRegIP(string ip) => player.SetData<string>(EntityData.PLAYER_REGIP, ip);
        public string GetRegIP() => player.GetData<string>(EntityData.PLAYER_REGIP);

        public void SetName(string name) => player.SetData<string>(EntityData.PLAYER_NAME, name);
        public string GetName()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_NAME)))
                return player.GetData<string>(EntityData.PLAYER_NAME);

            return "Undefined";
        }

        public void SetLVL(int lvl) => player.SetData<int>(EntityData.PLAYER_LVL, lvl);
        public int GetLVL() => player.GetData<int>(EntityData.PLAYER_LVL);

        public int GetAge() => player.GetData<int>(EntityData.PLAYER_AGE);
        public void SetAge(int age) => player.SetData<int>(EntityData.PLAYER_AGE, age);

        public void GiveMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData<double>(EntityData.PLAYER_MONEY, Math.Round(money, 2) + (player.GetData<double>(EntityData.PLAYER_MONEY)));

            if (updateindb)
            {
                string query = $"UPDATE `accounts` SET `p_money` = '{Convert.ToString(GetMoney()).Replace(',', '.')}' WHERE `p_id` = '{GetDbID()}'";
                new DBConnection.MySqlConnector().RequestExecuteNonQuery(query);
            }

            Utils.UtilityFuncs.UpdatePlayerHud(player);
        }
        public double GetMoney() => Math.Round(player.GetData<double>(EntityData.PLAYER_MONEY), 2);

        public void GiveBankMoney(double money, string reason = null, bool updateindb = true)
        {
            player.SetData<double>(EntityData.PLAYER_BANK, Math.Round(money, 2) + (player.GetData<double>(EntityData.PLAYER_BANK)));

            if (updateindb)
            {
                string query = $"UPDATE `accounts` SET `p_bank` = '{Convert.ToString(GetBankMoney()).Replace(',', '.')}' WHERE `p_id` = '{GetDbID()}'";
                new DBConnection.MySqlConnector().RequestExecuteNonQuery(query);
            }

            Utils.UtilityFuncs.UpdatePlayerHud(player);
        }
        public double GetBankMoney() => Math.Round(player.GetData<double>(EntityData.PLAYER_BANK), 2);

        public void SetHouse(int houseid) => player.SetData<int>(EntityData.PLAYER_HOUSE, houseid);
        public int GetHouse() => player.GetData<int>(EntityData.PLAYER_HOUSE);

        public void SetCustomize(object args) => player.SetData<object>(EntityData.PLAYER_CUSTOMIZE, args);
        public object GetCustomize() => player.GetData<object>(EntityData.PLAYER_CUSTOMIZE);

        public void SetClothes(object args) => player.SetData<object>(EntityData.PLAYER_CLOTHES, args);
        public object GetClothes() => player.GetData<object>(EntityData.PLAYER_CLOTHES);

        public void SetDateReg(string date) => player.SetData<string>(EntityData.PLAYER_DATEREG, date);
        public string GetDateReg()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_DATEREG)))
                return player.GetData<string>(EntityData.PLAYER_DATEREG);

            return "Undefined";
        }

        public void SetLastJoin(string date)
        {
            string query = $"UPDATE `accounts` SET `p_lastjoin` = '{date}' WHERE `p_id` = '{GetDbID()}'";

            new DBConnection.MySqlConnector().RequestExecuteNonQuery(query);

            player.SetData<string>(EntityData.PLAYER_LASTJOIN, date);
        }
        public string GetLastJoin()
        {
            if (!string.IsNullOrEmpty(player.GetData<string>(EntityData.PLAYER_LASTJOIN)))
                return player.GetData<string>(EntityData.PLAYER_LASTJOIN);

            return "Undefined";
        }

        public void SetSatiety(int value)
        {
            if (value > 100) value = 100;
            else if (value < 0) value = 0;

            player.SetData<int>(EntityData.PLAYER_SATIETY, value);

            Utils.UtilityFuncs.UpdatePlayerHud(player);
        }
        public int GetSatiety() => player.GetData<int>(EntityData.PLAYER_SATIETY);

        public void SetThirst(int value)
        {
            if (value > 100) value = 100;
            else if (value < 0) value = 0;

            player.SetData<int>(EntityData.PLAYER_THIRST, value);

            Utils.UtilityFuncs.UpdatePlayerHud(player);
        }
        public int GetThirst() => player.GetData<int>(EntityData.PLAYER_THIRST);

        public double GetPayCheck() => Math.Round(player.GetData<double>(EntityData.PLAYER_PAYCHECK), 2);
        public void AddToPayCheck(double money, string reason = null)
        {
            double paycheck = Math.Round(GetPayCheck() + money, 2);

            new DBConnection.MySqlConnector().RequestExecuteNonQuery($"UPDATE `accounts` SET `p_paycheck` = '{paycheck}' WHERE `p_id` = '{GetDbID()}'");

            player.SetData<double>(EntityData.PLAYER_PAYCHECK, paycheck);
        }
        public void InitPayCheck(double money) => player.SetData<double>(EntityData.PLAYER_PAYCHECK, money);
    }
}
