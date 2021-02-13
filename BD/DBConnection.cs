using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BD
{
    public class DBConnection
    {


        private static MySqlConnection GetConnectString()
        {
            string host = "localhost";
            int port = 3306;
            string database = "sys";
            string username = "root";
            string password = "8963";

            return new MySqlConnection ("Server=" + host + ";Database=" + database 
                + ";port=" + port + ";User=" + username + ";password=" + password); 
        }


        public static string GetSomeData()
        {
            //string answer = "1 Hello i'm from Russia Vlad Prost";
            string answer = "";
            MySqlConnection conn = GetConnectString();


            MySqlCommand cmd = new MySqlCommand("select * from email", conn);

            conn.Open();



            var reader = cmd.ExecuteReader();


            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    answer = answer + "1";
                }
            }


            return answer;
        }

        public static bool GetBool()
        {
            return true;
        }


    }
}
