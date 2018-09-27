using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Author
    {
        public int Id {get; set;}
        public string LastName {get; set;}
        public string GivenName {get; set;}

        public Author(string last, string given, int id = 0)
        {
            LastName = last;
            GivenName = given;
            Id = id;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if(!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author) otherAuthor;
                bool idEquality = this.Id.Equals(newAuthor.Id);
                bool lastEquality = this.LastName.Equals(newAuthor.LastName);
                bool givenEquality = this.GivenName.Equals(newAuthor.GivenName);
                return (lastEquality && givenEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.LastName.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO authors (last_name, given_name) VALUES (@lastName, @givenName);";

            cmd.Parameters.AddWithValue("@lastName", this.LastName);
            cmd.Parameters.AddWithValue("@givenName", this.GivenName);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Author Find(int authorId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM authors WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", authorId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string last = "";
            string given = "";
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                last = rdr.GetString(1);
                given = rdr.GetString(2);
            }
            Author newAuthor = new Author(last, given, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newAuthor;
        }

        public static List<Author> GetAll()
        {
            List<Author> allAuthors = new List<Author>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM authors;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string last = rdr.GetString(1);
                string given = rdr.GetString(2);
                Author newAuthor = new Author(last, given, id);
                allAuthors.Add(newAuthor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allAuthors;

        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM authors;";

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
            cmd.CommandText = @"DELETE FROM authors WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public void Update(string last, string given)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE authors SET last_name = @newLastName, given_name = @newGivenName WHERE id = @searchId;";

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

        public void AddBook(int bookId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@authorId, @bookId);";

            cmd.Parameters.AddWithValue("@authorId", this.Id);
            cmd.Parameters.AddWithValue("@bookId", bookId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void RemoveBookAuthor(int bookId, int authorId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM authors_books WHERE book_id = @bookId AND author_id = @authorId;";

            cmd.Parameters.AddWithValue("@authorId", authorId);
            cmd.Parameters.AddWithValue("@bookId", bookId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }



        public List<Book> GetBooks()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT books.* FROM authors
                JOIN authors_books ON (authors.id = authors_books.author_id)
                JOIN books ON (authors_books.book_id = books.id)
                WHERE authors.id = @authorId ORDER BY books.title ASC;";
            cmd.Parameters.AddWithValue("@authorId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Book> allBooks = new List<Book> {};
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string title = rdr.GetString(1);
                float cost = rdr.GetFloat(2);
                int currentCount = rdr.GetInt32(3);
                int totalCount = rdr.GetInt32(4);
                Book newBook = new Book(title, currentCount, totalCount, cost, id);
                allBooks.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allBooks;
        }
    }
}
