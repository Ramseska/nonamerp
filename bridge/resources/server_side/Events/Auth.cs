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
    class Auth : Script
    {
        private int exceptionCount { get; set; } // счетчик исключений для отладки 

        [RemoteEvent("Event_CancelAuth")] // событие при нажатии "отмена" во время авторизации\регистрации
        public void Event_CancelAuth(Client client, object[] args)
        {
            client.SendNotification("Was kicked for cancel auth/reg.");
            client.Kick();
        }

        [RemoteEvent("Event_GetAccountFromBD")] // чек аккаунта в бд с разветвлением при авторизации
        async public void Event_GetAccountFromBD(Client client, object[] args)
        {
            await Task.Run(() =>
            {
                MySqlConnection con = MySqlConnector.GetDBConnection();
                try
                {
                    Console.WriteLine("[MySQL]: Connection..");
                    con.Open();
                    Console.WriteLine("[MySQL]: Connected!");

                    switch (args[0])
                    {
                        case 0: // auth
                            {
                                string query = "SELECT * FROM `accounts` WHERE `p_name` = '" + args[1] + "'";
                                MySqlCommand cmd = new MySqlCommand(query, con);
                                MySqlDataReader read = cmd.ExecuteReader();
                                List<string> LoadedData = new List<string>();
                                
                                if (!read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Данный аккаунт не зарегистрирован!");
                                    read.Close();
                                    break;
                                }
                                
                                while(read.Read())
                                {
                                    LoadedData.Add(read["p_id"].ToString());
                                    LoadedData.Add(read["p_name"].ToString());
                                    LoadedData.Add(read["p_password"].ToString());
                                    LoadedData.Add(read["p_ip"].ToString());
                                    LoadedData.Add(read["p_mail"].ToString());
                                    LoadedData.Add(read["p_lvl"].ToString());
                                    LoadedData.Add(read["p_money"].ToString());
                                }
                                read.Close();

                                // dbg
                                foreach (var i in LoadedData)
                                    Console.WriteLine(i);

                                if(LoadedData[2] != args[2].ToString())
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", "[Ошибка]: Неверный пароль!");
                                    break;
                                }

                                LogInPlayerAccount(client, LoadedData);

                                break;
                            }
                        case 1: // reg
                            {
                                string query = "SELECT * FROM `accounts` WHERE `p_name` = '" + args[1] + "'";
                                MySqlCommand cmd = new MySqlCommand(query, con);
                                MySqlDataReader read = cmd.ExecuteReader();
                                List<string> LoadedData = new List<string>();

                                if (read.HasRows)
                                {
                                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", "[Ошибка]: Данный аккаунт уже зарегистрирован!");
                                    read.Close();
                                    break;
                                }
                                read.Close();

                                query = "INSERT INTO `accounts` (`p_name`, `p_password`, `p_ip`, `p_mail`) VALUES ('" + args[1] + "', '" + args[2] + "', '" + client.Address + "', '" + args[3] + "')";

                                cmd = new MySqlCommand(query, con);
                                cmd.ExecuteNonQuery();

                                CreatePlayerAccount(client, (string)args[1]);

                                break;
                            }
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[MySQL Error (#{++exceptionCount})]: " + e.Message);
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                }
            });
        }

        async public void CreatePlayerAccount(Client client, string name) // создание аккаунта
        {
            PlayerInfo player = new PlayerInfo(client);

            await Task.Run(() =>
            {
                MySqlConnection con = MySqlConnector.GetDBConnection();
                try
                {
                    Console.WriteLine("[MySQL]: Connection..");
                    con.Open();
                    Console.WriteLine("[MySQL]: Connected!");

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
                    }
                    read.Close();

                    // dbg
                    foreach (var i in data)
                        Console.WriteLine(i);

                    LogInPlayerAccount(client, data);

                    con.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[MySQL Error (#{++exceptionCount})]: " + e.Message);
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswer", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                    NAPI.ClientEvent.TriggerClientEvent(client, "SendBadAnswerReg", $"[Ошибка]: MySQL Exception! Обратитесь к администрации сервера. (#{exceptionCount})");
                }
            });
        }

        // тут логика при авторизации игрока. присвоение данных из бд, спавн и прочая дичь
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

            client.Name = data[1];

            client.Position = new Vector3(-143.7677, 6438.123, 31.4298);
            client.Rotation.Z = -49.8411f;
            client.Dimension = 0;

            NAPI.Entity.SetEntityTransparency(client, 255);

            NAPI.ClientEvent.TriggerClientEvent(client, "DestroyAuthBrowser");
            client.SendNotification("~g~Вы успешно авторизировались!");
        }
    }
}
