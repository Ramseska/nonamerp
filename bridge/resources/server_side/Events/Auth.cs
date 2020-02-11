using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using server_side.DBConnection;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using server_side.Data;
using Newtonsoft.Json;

namespace server_side.Events
{
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
                                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `p_name` = '" + args[1] + "'", con);
                                MySqlDataReader read = cmd.ExecuteReader();
                                List<string> LoadedData = new List<string>();

                                if (!read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Данный аккаунт не зарегистрирован!");
                                    read.Close();
                                    break;
                                }

                                while (read.Read())
                                {
                                    LoadedData.Add(read["p_id"].ToString());
                                    LoadedData.Add(read["p_name"].ToString());
                                    LoadedData.Add(read["p_password"].ToString());
                                    LoadedData.Add(read["p_ip"].ToString());
                                    LoadedData.Add(read["p_mail"].ToString());
                                    LoadedData.Add(read["p_lvl"].ToString());
                                    LoadedData.Add(read["p_money"].ToString());
                                    client.SetData("TGFBD_PCST", read["p_customize"]);
                                }
                                read.Close();

                                if (LoadedData[2] != args[2].ToString())
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Неверный пароль!");
                                    break;
                                }

                                LogInPlayerAccount(client, LoadedData);

                                break;
                            }
                        case 1: // reg
                            {
                                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `p_name` = '" + args[1] + "'", con);
                                MySqlDataReader read = cmd.ExecuteReader();

                                if (read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", "[Ошибка]: Данный аккаунт уже зарегистрирован!");
                                    read.Close();
                                    break;
                                }
                                read.Close();

                                client.SetData("R_TempName", (string)args[1]);
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

                    string query = "INSERT INTO `accounts` (`p_name`, `p_password`, `p_ip`, `p_mail`, `p_customize`) VALUES ('" + client.GetData("R_TempName") + "', '" + client.GetData("R_TempPassword") + "', '" + client.Address + "', '" + client.GetData("R_TempMail") + "', '" + customize + "')";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.ExecuteNonQuery();

                    CreatePlayerAccount(client, client.GetData("R_TempName"));

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

                    string query = "SELECT * FROM `accounts` WHERE `p_name` = '" + name + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader read = cmd.ExecuteReader();
                    List<string> data = new List<string>();

                    while (read.Read())
                    {
                        data.Add(read["p_id"].ToString());
                        data.Add(read["p_name"].ToString());
                        data.Add(read["p_password"].ToString());
                        data.Add(read["p_ip"].ToString());
                        data.Add(read["p_mail"].ToString());
                        data.Add(read["p_lvl"].ToString());
                        data.Add(read["p_money"].ToString());
                        client.SetData("TGFBD_PCST", read["p_customize"]);
                    }
                    read.Close();

                    LogInPlayerAccount(client, data);

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

        public void LogInPlayerAccount(Client client, List<string> data) 
        {
            PlayerInfo player = new PlayerInfo(client);

            player.SetAuthorized(true);
            player.SetDbID(Convert.ToInt32(data[0]));
            player.SetLVL(Convert.ToInt32(data[5]));
            player.SetMail(data[4]);
            player.SetMoney(Convert.ToInt32(data[6]));
            player.SetName(data[1]);
            player.SetPassword(data[2]);
            player.SetRegIP(data[3]);
            player.SetCustomize(client.GetData("TGFBD_PCST"));
            
            client.ResetData("TGFBD_PCST");
            client.ResetData("R_TempName");
            client.ResetData("R_TempPassword");
            client.ResetData("R_TempMail");

            client.Name = data[1];

            client.Position = new Vector3(-143.7677, 6438.123, 31.4298);
            client.Rotation.Z = -49.8411f;
            client.Dimension = 0;

            NAPI.Entity.SetEntityTransparency(client, 255);

            if(player.GetCustomize() != null)
                NAPI.ClientEvent.TriggerClientEvent(client, "unevhnd", player.GetCustomize());

            NAPI.ClientEvent.TriggerClientEvent(client, "DestroyAuthBrowser");
            //client.SendNotification("~g~Вы успешно авторизировались!");
            Utilities.UtilityFuncs.SendPlayerNotify(client, 2, "Вы успешно авторизировались!");
        }
    }
}
