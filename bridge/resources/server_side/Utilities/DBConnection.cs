using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

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

            String connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }
}
