namespace BD
{
    public class CmdCreator
	{
		public static string GetSelectCmdText(string table, string searchColumn = "")
		{
			string CmdText = "SELECT * FROM " + table + " ";

			if (searchColumn != string.Empty)
			{
				CmdText += "WHERE " + searchColumn + " = @" + searchColumn;
			}

			return CmdText;
		}
		public static string GetSelectCmdTextByRange(string table, string searchColumn, string searchColumn2)
		{
			string CmdText = "SELECT * FROM " + table + " ";

			CmdText += "WHERE " + searchColumn + " > '@"
				+ searchColumn + "' and " + searchColumn + " < " + "'@" + searchColumn2 +  "'"; 

			return CmdText;
		}

		public static string GetInsertCmdText(string Table, string[] columns)
		{
			string cmdText = "INSERT INTO ";
			cmdText += Table + " ";

			string cmdColumns = "(";
			string cmdValues = "(";

			bool FirstIteration = true;

			foreach (var col in columns)
			{
				if (FirstIteration)
				{
					cmdColumns += col;
					cmdValues += "@" + col;
					FirstIteration = false;
				}
				else
				{
					cmdColumns += ", " + col;
					cmdValues += ", @" + col;
				}
			}

			cmdColumns += ")";
			cmdValues += ")";

			cmdText += cmdColumns + " values ";

			cmdText += cmdValues;

			return cmdText;
		}

		public static string GetDeleteCmdText(string table, string column)
		{
			string text = "DELETE FROM " + table + " WHERE " + column + " = @" + column;

			return text;
		}

		public static string GetFuncValueFromDbByColumn(string table, string column, string function)
		{
			return "SELECT " + function + "(" + column + ") as id FROM " + table;
		}
	}
}
