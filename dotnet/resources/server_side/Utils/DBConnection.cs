using System;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace server_side.DBConnection
{
    public class MySqlConnector
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "lognet";
            string username = "root";
            string password = "";

            string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

        async public static void RequestExecuteNonQuery(string request)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (MySqlConnection con = GetDBConnection())
                    {
                        con.Open();
                        new MySqlCommand(request, con).ExecuteNonQuery();
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
