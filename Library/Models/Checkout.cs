using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Checkout
    {
        public int Id {get; set;}
        public int BookId {get; set;}
        public int PatronId {get; set;}
        public DateTime CheckoutDate {get; set;}
        public DateTime DueDate {get; set;}
        public bool Returned {get; set;}

        public Checkout(int bookId, int patronId, DateTime checkoutDate, DateTime dueDate, bool returned, int id = 0)
        {
            BookId = bookId;
            PatronId = patronId;
            CheckoutDate = checkoutDate;
            DueDate = dueDate;
            Returned = returned;
            Id = id;
        }

        public override bool Equals(System.Object otherCheckout)
        {
            if(!(otherCheckout is Checkout))
            {
                return false;
            }
            else
            {
                Checkout newCheckout = (Checkout) otherCheckout;
                bool idEquality = this.Id.Equals(newCheckout.Id);
                bool bookIdEquality = this.BookId.Equals(newCheckout.BookId);
                bool patronIdEquality = this.PatronId.Equals(newCheckout.PatronId);
                bool checkoutDateEquality = this.CheckoutDate.Equals(newCheckout.CheckoutDate);
                bool dueDateEquality = this.DueDate.Equals(newCheckout.DueDate);
                bool returnedEquality = this.Returned.Equals(newCheckout.Returned);
                return (bookIdEquality && patronIdEquality && idEquality && checkoutDateEquality && dueDateEquality && returnedEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO patrons (book_id, patrons_id, chekout_date, due_date, returned) VALUES (@bookId, @patronsId, @checkoutDate, @dueDate, @returned);";

            cmd.Parameters.AddWithValue("@bookId", this.BookId);
            cmd.Parameters.AddWithValue("@patronsId", this.PatronId);
            cmd.Parameters.AddWithValue("@checkoutDate", this.CheckoutDate);
            cmd.Parameters.AddWithValue("@dueDate", this.DueDate);
            cmd.Parameters.AddWithValue("@returned", this.Returned);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Checkout Find(int checkoutId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM checkouts WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", checkoutId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            int bookId = 0;
            int patronId = 0;
            DateTime checkout = DateTime.MinValue;
            DateTime due = DateTime.MinValue;
            bool returned = false;
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                bookId = rdr.GetString(1);
                patronId = rdr.GetString(2);
                checkout = rdr.GetDateTime(3);
                due = rdr.GetDateTime(4);
                returned = rdr.GetBoolean(5);
            }
            Checkout foundCheckout = new Checkout(bookId, patronId, checkoutDate, dueDate, returned, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundCheckout;
        }

        public static List<Checkout> GetAll()
        {
            List<Checkout> allCheckouts = new List<Checkout>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM checkouts;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int bookId = rdr.GetString(1);
                int patronId = rdr.GetString(2);
                DateTime checkout = rdr.GetDateTime(3);
                DateTime due = rdr.GetDateTime(4);
                bool returned = rdr.GetBoolean(5);
                Checkout foundCheckout = new Checkout(bookId, patronId, checkoutDate, dueDate, returned, id);
                allCheckouts.Add(foundCheckout);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCheckouts;

        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM checkouts;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM checkouts WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void CheckIn()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE patrons SET returned = true WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.Returned = true;
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
