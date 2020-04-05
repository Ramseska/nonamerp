using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using server_side.DBConnection;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using server_side.Data;

namespace server_side.Events
{
    class AuthData : Script
    {
        public Client Player;
        public int dbID;
        public string Login;
        public string Password;
        public string Mail;
        public string IP;
        public int LVL;
        public double Cash;
        public double BankMoney;
        public int Age;
        public string Name;

        public AuthData(Client player)
        {
            this.Player = player;
        }
        public AuthData() { }
    }
    class Auth : Script
    {
        private static int exceptionCount { get; set; } // счетчик исключений для отладки 

        [RemoteEvent("Event_CancelAuth")] // событие при нажатии "отмена" во время авторизации\регистрации
        public void Event_CancelAuth(Client client, object[] args)
        {
            client.SendNotification("Was kicked for cancel auth/reg.");
            client.Kick();
        }

        [RemoteEvent("Event_GetAccountFromBD")]
        async public void Event_GetAccountFromBD(Client client, object[] args)
        {
            await Task.Run(() =>
            {
                try
                {
                    MySqlConnection con = MySqlConnector.GetDBConnection();

                    con.Open();

                    switch (args[0])
                    {
                        case 0: // auth
                            {
                                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `p_login` = '" + args[1] + "'", con);
                                MySqlDataReader read = cmd.ExecuteReader();
                                AuthData auth = new AuthData(client);

                                if (!read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Данный аккаунт не зарегистрирован!");
                                    read.Close();
                                    break;
                                }

                                while (read.Read())
                                {
                                    auth.dbID = (int)read["p_id"];
                                    auth.Login = (string)read["p_login"];
                                    auth.Password = (string)read["p_password"];
                                    auth.IP = (string)read["p_ip"];
                                    auth.Mail = (string)read["p_mail"];
                                    auth.LVL = (int)read["p_lvl"];
                                    auth.Cash = (double)read["p_money"];
                                    auth.BankMoney = (double)read["p_bank"];
                                    auth.Age = (int)read["p_age"];
                                    auth.Name = (string)read["p_name"];

                                    client.SetData("pCustomize", read["p_customize"]);
                                    client.SetData("pClothes", read["p_clothes"]);
                                }
                                read.Close();

                                if (auth.Password != args[2].ToString())
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Неверный пароль!");
                                    break;
                                }

                                LogInPlayerAccount(client, auth);

                                break;
                            }
                        case 1: // reg
                            {
                                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `p_login` = '" + args[1] + "'", con);
                                MySqlDataReader read = cmd.ExecuteReader();

                                if (read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", "[Ошибка]: Данный аккаунт уже зарегистрирован!");
                                    read.Close();
                                    break;
                                }
                                read.Close();

                                client.SetData("R_TempLogin", (string)args[1]);
                                client.SetData("R_TempPassword", (string)args[2]);
                                client.SetData("R_TempMail", (string)args[3]);

                                NAPI.ClientEvent.TriggerClientEvent(client, "DestroyAuthBrowser");

                                JumpToCustomizeStep(client);

                                break;
                            }
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    NAPI.Util.ConsoleOutput($"[MySQL Error (#{++exceptionCount})]: " + e.Message);
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                }
            });
        }

        async public void JumpToCustomizeStep(Client client)
        {
            NAPI.ClientEvent.TriggerClientEvent(client, "StartPlayerCustomize");

            await Task.Delay(2000);

            /*
            client.Position = new Vector3(501.6888, 5603.808, 797.9096);
            client.Rotation = new Vector3(0, 0, 353.2294);
            */
            client.Position = new Vector3(402.8664, -996.4108, -99.00027);
            client.Rotation = new Vector3(0, 0, -185.0000);
            client.Dimension = (uint)client.Value;
        }

        [RemoteEvent("EndPlayerCustomize")]
        async public void EndPlayerCustomize(Client client, dynamic customize)
        {
            await Task.Run(() =>
            {
                try
                {
                    MySqlConnection con = MySqlConnector.GetDBConnection();

                    con.Open();

                    string query = "INSERT INTO `accounts` (`p_login`, `p_password`, `p_ip`, `p_mail`, `p_customize`) VALUES ('" + client.GetData("R_TempLogin") + "', '" + client.GetData("R_TempPassword") + "', '" + client.Address + "', '" + client.GetData("R_TempMail") + "', '" + customize + "')";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.ExecuteNonQuery();

                    CreatePlayerAccount(client, client.GetData("R_TempLogin"));

                    con.Close();
                }
                catch (Exception e) { Console.WriteLine(e); }
            });
        }

        async public void CreatePlayerAccount(Client client, string name)
        {
            PlayerInfo player = new PlayerInfo(client);

            await Task.Run(() =>
            {
                try
                {
                    MySqlConnection con = MySqlConnector.GetDBConnection();

                    con.Open();

                    string query = "SELECT * FROM `accounts` WHERE `p_login` = '" + name + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader read = cmd.ExecuteReader();
                    AuthData auth = new AuthData(client);

                    while (read.Read())
                    {
                        auth.dbID = (int)read["p_id"];
                        auth.Login = (string)read["p_login"];
                        auth.Password = (string)read["p_password"];
                        auth.IP = (string)read["p_ip"];
                        auth.Mail = (string)read["p_mail"];
                        auth.LVL = (int)read["p_lvl"];
                        auth.Cash = (double)read["p_money"];
                        auth.BankMoney = (double)read["p_bank"];
                        auth.Age = (int)read["p_age"];
                        client.SetData("pCustomize", read["p_customize"]);
                    }
                    read.Close();

                    LogInPlayerAccount(client, auth);

                    con.Close();
                    read.Close();
                }
                catch (Exception e)
                {
                    NAPI.Util.ConsoleOutput($"[MySQL Error (#{++exceptionCount})]: " + e.Message);
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                }
            });
        }

        public void LogInPlayerAccount(Client client, AuthData data) 
        {
            PlayerInfo player = new PlayerInfo(client);

            player.SetAuthorized(true);
            player.SetDbID(data.dbID);
            player.SetLVL(data.LVL);
            player.SetLogin(data.Login);
            player.SetMail(data.Mail);
            player.SetPassword(data.Password);
            player.SetRegIP(data.IP);
            player.SetCustomize(client.GetData("pCustomize"));
            player.GiveMoney(data.Cash, updateindb: false);
            player.GiveBankMoney(data.BankMoney, updateindb: false);
            player.SetAge(data.Age);

            client.ResetData("pCustomize");
            client.ResetData("R_TempLogin");
            client.ResetData("R_TempPassword");
            client.ResetData("R_TempMail");

            client.Name = data.Login;

            client.Position = new Vector3(-143.7677, 6438.123, 31.4298);
            client.Rotation.Z = -49.8411f;
            client.Dimension = 0;

            NAPI.Entity.SetEntityTransparency(client, 255);

            if(player.GetCustomize() != null)
                NAPI.ClientEvent.TriggerClientEvent(client, "unevhnd", player.GetCustomize());

            NAPI.ClientEvent.TriggerClientEvent(client, "DestroyAuthBrowser");
            Utilities.UtilityFuncs.SendPlayerNotify(client, 2, "Вы успешно авторизировались!");
        }
    }
}
