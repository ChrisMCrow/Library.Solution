using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Patron
    {
        public int Id {get; set;}
        public string LastName {get; set;}
        public string GivenName {get; set;}
        public bool OverDue {get; set;}

        public Patron(string last, string given, int id = 0)
        {
            LastName = last;
            GivenName = given;
            OverDue = this.HasOverDue();
            Id = id;
        }

        public override bool Equals(System.Object otherPatron)
        {
            if(!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron) otherPatron;
                bool idEquality = this.Id.Equals(newPatron.Id);
                bool lastEquality = this.LastName.Equals(newPatron.LastName);
                bool givenEquality = this.GivenName.Equals(newPatron.GivenName);
                bool overdueEquality = this.OverDue.Equals(newPatron.OverDue);
                return (lastEquality && givenEquality && idEquality && overdueEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO patrons (last_name, given_name, overdue) VALUES (@lastName, @givenName, @overdue);";

            cmd.Parameters.AddWithValue("@lastName", this.LastName);
            cmd.Parameters.AddWithValue("@givenName", this.GivenName);
            cmd.Parameters.AddWithValue("@overdue", this.OverDue);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Patron Find(int patronId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", patronId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string last = "";
            string given = "";
            bool overdue = false;
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                last = rdr.GetString(1);
                given = rdr.GetString(2);
                overdue = rdr.GetBoolean(3);
            }
            Patron foundPatron = new Patron(last, given, overdue, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundPatron;
        }

        public static List<Patron> GetAll()
        {
            List<Patron> allPatrons = new List<Patron>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM patrons;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string last = rdr.GetString(1);
                string given = rdr.GetString(2);
                bool overdue = rdr.GetBoolean(3);
                Patron newPatron = new Patron(last, given, overdue, id);
                allPatrons.Add(newPatron);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPatrons;

        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons;";

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
            cmd.CommandText = @"DELETE FROM patrons WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public void Update(string last, int given)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE patrons SET last_name = @newLastName, given_name = @newGivenName WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newLastName", last);
            cmd.Parameters.AddWithValue("@newGivenName", given);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.LastName = last;
            this.GivenName = given;
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Checkout> GetCurrentCheckouts()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT checkouts.* FROM patrons
                JOIN patrons ON (checkouts.patrons_id = patrons.id)
                WHERE checkouts.patrons_id = @patronId
                AND returned = false
                SORT BY due_date ASC;";
            cmd.Parameters.AddWithValue("@patronId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Checkout> allCheckouts = new List<Checkout> {};
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int book_id = rdr.GetInt32(1);
                int patrons_id = rdr.GetInt32(2);
                DateTime checkout = rdr.GetDateTime(3);
                DateTime due = rdr.GetDateTime(4);
                bool returned = rdr.GetBoolean(5);
                Checkout newCheckout = new Checkout(book_id, patrons_id, checkout, due, returned, id);
                allCheckouts.Add(newCheckout);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCheckouts;
        }

        public List<Checkout> GetReturnedCheckouts()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT checkouts.* FROM patrons
                JOIN patrons ON (checkouts.patrons_id = patrons.id)
                WHERE checkouts.patrons_id = @patronId
                AND returned = true
                SORT BY due_date ASC;";
            cmd.Parameters.AddWithValue("@patronId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Checkout> allCheckouts = new List<Checkout> {};
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int book_id = rdr.GetInt32(1);
                int patrons_id = rdr.GetInt32(2);
                DateTime checkout = rdr.GetDateTime(3);
                DateTime due = rdr.GetDateTime(4);
                bool returned = rdr.GetBoolean(5);
                Checkout newCheckout = new Checkout(book_id, patrons_id, checkout, due, returned, id);
                allCheckouts.Add(newCheckout);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCheckouts;
        }

        public List<Checkout> GetCheckoutHistory()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT checkouts.* FROM patrons
                JOIN patrons ON (checkouts.patrons_id = patrons.id)
                WHERE checkouts.patrons_id = @patronId SORT BY checkout_date ASC;";
            cmd.Parameters.AddWithValue("@patronId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Checkout> allCheckouts = new List<Checkout> {};
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int book_id = rdr.GetInt32(1);
                int patrons_id = rdr.GetInt32(2);
                DateTime checkout = rdr.GetDateTime(3);
                DateTime due = rdr.GetDateTime(4);
                bool returned = rdr.GetBoolean(5);
                Checkout newCheckout = new Checkout(book_id, patrons_id, checkout, due, returned, id);
                allCheckouts.Add(newCheckout);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCheckouts;
        }

        public bool HasOverDue()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT COUNT(*) FROM patrons
                JOIN patrons ON (checkouts.patrons_id = patrons.id)
                WHERE checkouts.patrons_id = @patronId
                AND checkouts.due_date < GETDATE();";
            cmd.Parameters.AddWithValue("@patronId", this.Id);

            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            return (count > 0) ? true : false;
        }

        public float GetAmountDue()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT books.cost FROM patrons
                JOIN checkouts ON (checkouts.patrons_id = patrons.id)
                JOIN books ON (checkouts.book_id = books.id)
                WHERE due_date < GETDATE()
                AND patrons.id = @patronId;";
            cmd.Parameters.AddWithValue("@patronId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            float amountDue = 0f;
            while(rdr.Read())
            {
                amountDue += rdr.GetFloat(2);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return amountDue;

        }
    }
}
