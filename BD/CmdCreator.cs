using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BD
{
    public class CmdCreator
    {
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

        public static string GetSelectCmdText(string table, string searchColumn = "")
        {
            string CmdText = "SELECT * FROM " + table + " ";

            if (searchColumn != string.Empty)
            {
                CmdText += "WHERE " + searchColumn + " = @" + searchColumn;
            }

            return CmdText;
        }

        public static string GetMaxValueFromDbByColumn(string table, string column)
        {
            return "SELECT max(" + column + ") as id FROM " + table;
        }
    }
}
