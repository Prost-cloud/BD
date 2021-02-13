using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace BD
{
    public class DBConnection
    {

        private static MySqlConnection DBConnect = GetConnectString();
        private static MySqlConnection GetConnectString()
        {
            string host = "localhost";
            int port = 3306;
            string database = "emails";
            string username = "root";
            string password = "root";

            return new MySqlConnection("server=" + host + ";port=" + port +
               ";username=" + username + ";password=" + password + ";Database=" + database);
        }

        private static void openConnection()
        {
            if(DBConnect.State == ConnectionState.Closed)
            {
                DBConnect.Open();
            }
        }
        
        private static void closeConnection()
        {
            if (DBConnect.State == ConnectionState.Open)
            {
                DBConnect.Close();
            }
        }

       

        public static EMail[] SelectFromDb(int idMessage)
        {


            openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string cmdText = idMessage == -1 ? "SELECT * FROM `mail` " : "SELECT * FROM `mail` WHERE `id` = @id";
            MySqlCommand cmd = new MySqlCommand(cmdText, DBConnect);

            cmd.Parameters.Add("id", MySqlDbType.Int64).Value = idMessage;

            adapter.SelectCommand = cmd;

            adapter.Fill(table);

            closeConnection();

            if (table.Rows.Count > 0) {
                EMail[] emails = new EMail[table.Rows.Count];

                for (int i = 0; i < table.Rows.Count; i++)
                    emails[i] = new EMail(table.Rows[i]);

                return emails;
            }
            return new EMail[0];
        }

        public static void InsertToDb(EMail mail)
        {
            int CurrentId = GetMaxMailId() + 1;

            openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string CmdText = "INSERT INTO `mail`(`id`, `title`, `mailfrom`, `mailto`, `message`, `date`, `authorname`) " +
                            "VALUES (@id, @title, @mailfrom, @mailto, @message, @date, @authorname)";
            MySqlCommand cmd = new MySqlCommand(CmdText, DBConnect);

            cmd.Parameters.Add("id", MySqlDbType.Int64).Value = CurrentId;
            cmd.Parameters.Add("title", MySqlDbType.VarChar).Value = mail.Title;
            cmd.Parameters.Add("mailfrom", MySqlDbType.VarChar).Value = mail.MailFrom;
            cmd.Parameters.Add("mailto", MySqlDbType.VarChar).Value = mail.MailTo;
            cmd.Parameters.Add("message", MySqlDbType.VarChar).Value = mail.Message;
            cmd.Parameters.Add("date", MySqlDbType.DateTime).Value = mail.Date;
            cmd.Parameters.Add("authorname", MySqlDbType.VarChar).Value = mail.AuthorName;

            adapter.InsertCommand = cmd;

            cmd.ExecuteNonQuery();

            closeConnection();

        }

        private static int GetMaxMailId()
        {
            openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand cmd = new MySqlCommand("SELECT max(id) as id FROM mail", DBConnect);

            adapter.SelectCommand = cmd;

            adapter.Fill(table);

            closeConnection();   
            
            return table.Rows[0].Field<int>("id");
        }

        public static void UpdateToDb(EMail mail)
        {

        }
    }
}