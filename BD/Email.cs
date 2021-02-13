using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BD
{

    public class EMail
    {
        public int ID { get; private set; }
        public string Title { get; private set; }
        public string MailFrom { get; private set; }
        public string MailTo { get; private set; }
        public string Message { get; private set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; private set; }
        public string AuthorName { get; private set; }
        public byte[] attachments { get; private set; }
        public Tags[] tags { get; private set; }



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
            this.tags = new Tags[8];
        }

        //public EMail(DataTable table)
        //{

        //    this.ID = table.Rows[0].Field<int>("id");
        //    Title = table.Rows[0].Field<string>("Title");
        //    MailFrom = table.Rows[0].Field<string>("mailfrom");
        //    MailTo = table.Rows[0].Field<string>("mailto");
        //    Message = table.Rows[0].Field<string>("message");
        //    Date = table.Rows[0].Field<DateTime>("date");
        //    AuthorName = table.Rows[0].Field<string>("authorname");
        //}

        public EMail(DataRow email) : this
        (
            email.Field<int>("id"),
            email.Field<string>("Title"),
            email.Field<string>("mailfrom"),
            email.Field<string>("mailto"),
            email.Field<string>("message"),
            email.Field<DateTime>("date"),
            email.Field<string>("authorname")
        ){ }
        

        public EMail() : this(0, "", "", "", "", DateTime.Now, "") { }

    }

    public class Tags
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        
        public Tags(int I, string name)
        {
            Id = I;
            Name = name;
        }

        
    }



}
