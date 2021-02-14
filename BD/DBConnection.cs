using MySql.Data.MySqlClient;
using System.Data;

namespace BD
{
    public class DBConnection
	{

		private static readonly MySqlConnection DBConnect = GetConnectString();
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

		private static void OpenConnection()
		{
			if (DBConnect.State == ConnectionState.Closed)
			{
				DBConnect.Open();
			}
		}

		private static void CloseConnection()
		{
			if (DBConnect.State == ConnectionState.Open)
			{
				DBConnect.Close();
			}
		}


		public static DataTable SelectFromDb(MySqlCommand cmd)
		{
			OpenConnection();

			DataTable DataTable = new DataTable();

			MySqlDataAdapter adapter = new MySqlDataAdapter();

			cmd.Connection = DBConnect;
			
			adapter.SelectCommand = cmd;

			adapter.Fill(DataTable);

			CloseConnection();
						
			return DataTable;
		}

		public static void InsertToDb(MySqlCommand cmd)
		{
			OpenConnection();
		   
			MySqlDataAdapter adapter = new MySqlDataAdapter();

			adapter.InsertCommand = cmd;

			cmd.Connection = DBConnect;

			cmd.ExecuteNonQuery();

			CloseConnection();
		}

		public static void UpdateToDb(string CmdText, MySqlCommand cmd)
		{
			OpenConnection();

			MySqlDataAdapter adapter = new MySqlDataAdapter();

			adapter.UpdateCommand = cmd;
			
			cmd.Connection = DBConnect;

			cmd.ExecuteNonQuery();
			
			CloseConnection();
		}

		public static void DeleteFromDB(MySqlCommand cmd)
		{
			OpenConnection();

			MySqlDataAdapter adapter = new MySqlDataAdapter();

			adapter.DeleteCommand = cmd;

			cmd.Connection = DBConnect;

			cmd.ExecuteNonQuery();

			CloseConnection();
		}

	}
}