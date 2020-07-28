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
        public MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "lognet";
            string username = "root";
            string password = "";

            string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection con = new MySqlConnection(connString);

            return con;
        }

        async public void RequestExecuteNonQuery(string request)
        {
            try
            {
                await Task.Run(() =>
                {
                    using(MySqlConnection con = GetDBConnection())
                    {
                        con.OpenAsync();
                        new MySqlCommand(request, con).ExecuteNonQuery();
                        con.CloseAsync();
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
