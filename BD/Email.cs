using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using MySql.Data.MySqlClient;

namespace BD
{

	public class EMail
	{
		private static readonly string tableName = "mail";

		public int ID { get; private set; }
		public string Title { get; private set; }
		public string MailFrom { get; private set; }
		public string MailTo { get; private set; }
		public string Message { get; private set; }
		[DataType(DataType.DateTime)]
		public DateTime Date { get; private set; }
		public string AuthorName { get; private set; }
		public byte[] Attachments { get; private set; }
		public Tags[] Tags { get; private set; }

		public EMail(int ID, string title, string mailFrom, string mailTo, string message, DateTime date, string authorName)
		{
			this.ID = ID;
			Title = title;
			MailFrom = mailFrom;
			MailTo = mailTo;
			Message = message;
			Date = date;
			AuthorName = authorName;
			//this.attachments = attachments;
			this.Tags = new Tags[8];
		}
		public EMail(DataRow email) : this
		(
			email.Field<int>("id"),
			email.Field<string>("Title"),
			email.Field<string>("mailfrom"),
			email.Field<string>("mailto"),
			email.Field<string>("message"),
			email.Field<DateTime>("date"),
			email.Field<string>("authorname")
		)
		{ }

		public EMail() : this(0, "", "", "", "", DateTime.Now, "") { }


		public static EMail[] SelectFromDb(string column = "", string SearchValue = "")
		{
			string cmdText = SearchValue == (-1).ToString() ? CmdCreator.GetSelectCmdText(tableName) : CmdCreator.GetSelectCmdText(tableName, column);

			MySqlCommand cmd = new MySqlCommand(cmdText);

			if (column != string.Empty)
				cmd.Parameters.Add(column, MySqlDbType.String).Value = SearchValue;

			var data = DBConnection.SelectFromDb(cmd);

			if (data.Rows.Count > 0)
			{
				EMail[] emails = new EMail[data.Rows.Count];

				for (int i = 0; i < data.Rows.Count; i++)
					emails[i] = new EMail(data.Rows[i]);

				return emails;
			}
			return new EMail[0];
		}

		public static EMail[] selectFromDbByRange(string column, string lowRange, string highRange)
        {
			string cmdText = CmdCreator.GetSelectCmdTextByRange(tableName, column, column +"1");

			MySqlCommand cmd = new MySqlCommand(cmdText);

			cmd.Parameters.Add(column, MySqlDbType.String).Value = lowRange;
			cmd.Parameters.Add(column + "1", MySqlDbType.String).Value = highRange;

			var data = DBConnection.SelectFromDb(cmd);

			if (data.Rows.Count > 0)
			{
				EMail[] emails = new EMail[data.Rows.Count];

				for (int i = 0; i < data.Rows.Count; i++)
					emails[i] = new EMail(data.Rows[i]);

				return emails;
			}
			return new EMail[0];
		}

		public void InsertToDB()
		{
			int CurrentId = GetMaxMailId() + 1;

			string CmdText = CmdCreator.GetInsertCmdText(tableName, new string[7] { "id", "title", "mailfrom", "mailto", "message", "date", "authorname" });
			MySqlCommand cmd = new MySqlCommand(CmdText);

			cmd.Parameters.Add("id", MySqlDbType.Int64).Value = CurrentId;
			cmd.Parameters.Add("title", MySqlDbType.VarChar).Value = Title;
			cmd.Parameters.Add("mailfrom", MySqlDbType.VarChar).Value = MailFrom;
			cmd.Parameters.Add("mailto", MySqlDbType.VarChar).Value = MailTo;
			cmd.Parameters.Add("message", MySqlDbType.VarChar).Value = Message;
			cmd.Parameters.Add("date", MySqlDbType.DateTime).Value = Date;
			cmd.Parameters.Add("authorname", MySqlDbType.VarChar).Value = AuthorName;

			DBConnection.InsertToDb(cmd);

			foreach (var tag in Tags)
			{
				if (tag != null)
					tag.UpdateTags(this);
			}
		}

		public void DeleteFromDB()
		{
			string cmdText = CmdCreator.GetDeleteCmdText(tableName, "id");

			MySqlCommand cmd = new MySqlCommand(cmdText);

			cmd.Parameters.Add("id", MySqlDbType.Int32).Value = ID;

			DBConnection.DeleteFromDB(cmd);
			this.DeleteTags();
		}

		public void DeleteTags()
        {
			string cmdText = CmdCreator.GetDeleteCmdText("tagjoins", "idmail");

			MySqlCommand cmd = new MySqlCommand(cmdText);

			cmd.Parameters.Add("idmail", MySqlDbType.Int32).Value = ID;

			DBConnection.DeleteFromDB(cmd);
		}

		private static int GetMaxMailId()
		{
			string cmdText = CmdCreator.GetFuncValueFromDbByColumn(tableName, "id", "max");
            
            MySqlCommand cmd = new MySqlCommand(cmdText);

            DataTable table = DBConnection.SelectFromDb(cmd);

            return table.Rows[0].Field<int>("id");
		}
	}

	public class Tags
	{
		public int ID { get; private set; }
		public string Name { get; private set; }

		public Tags(int Id, string name)
		{
			ID = Id;
			Name = name;
		}

		public void UpdateTags(EMail mail)
		{
			string CmdText = CmdCreator.GetInsertCmdText("tagjoins", new string[2] { "idmail", "idtag" });

			MySqlCommand cmd = new MySqlCommand(CmdText);

			cmd.Parameters.Add("idmail", MySqlDbType.Int32).Value = mail.ID;
			cmd.Parameters.Add("idtag", MySqlDbType.Int32).Value = ID;

			DBConnection.UpdateToDb(CmdText, cmd);
		}		
	}
}