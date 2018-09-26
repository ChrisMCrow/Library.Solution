using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Book
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public float Cost {get; set;}
        public int CurrentCount {get; set;}
        public int TotalCount {get; set;}

        public Book(string title, int count, float cost=15.99f, int id=0)
        {
            Title = title;
            CurrentCount = count;
            TotalCount = count;
            Cost = cost;
            Id = id;
        }

        public Book(string title, int currentCount, int totalCount, float cost, int id)
        {
            Title = title;
            CurrentCount = currentCount;
            TotalCount = totalCount;
            Cost = cost;
            Id = id;
        }


        public override bool Equals(System.Object otherBook)
        {
            if(!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book) otherBook;
                bool idEquality = this.Id.Equals(newBook.Id);
                bool titleEquality = this.Title.Equals(newBook.Title);
                return (titleEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO books (title, cost, current_count, total_count) VALUES (@title, @cost, @currentCount, @totalCount);";

            cmd.Parameters.AddWithValue("@title", this.Title);
            cmd.Parameters.AddWithValue("@cost", this.Cost);
            cmd.Parameters.AddWithValue("@currentCount", this.CurrentCount);
            cmd.Parameters.AddWithValue("@totalCount", this.TotalCount);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Book Find(int bookId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", bookId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string title = "";
            float cost = 0;
            int currentCount = 0;
            int totalCount = 0;
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                title = rdr.GetString(1);
                cost = rdr.GetFloat(2);
                currentCount = rdr.GetInt32(3);
                totalCount = rdr.GetInt32(4);
            }
            Book newBook = new Book(title, currentCount, totalCount, cost, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBook;
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM books;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
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

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM books;";

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
            cmd.CommandText = @"DELETE FROM books WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public void Update(string title, int totalCount, int cost)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET title = @newTitle, total_count = @newTotalCount, cost = @newCost WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newTitle", title);
            cmd.Parameters.AddWithValue("@newTotalCount", totalCount);
            cmd.Parameters.AddWithValue("@newCost", cost);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.Title = title;
            this.TotalCount = totalCount;
            this.Cost = cost;
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void BookCheckout()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET current_count = @newCount WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newCount", this.CurrentCount--);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.CurrentCount--;

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void BookReturn()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET current_count = @newCount WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newCount", this.CurrentCount++);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.CurrentCount++;

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Author> GetAuthors()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT authors.* FROM books
                JOIN authors_books ON (books.id = authors_books.book_id)
                JOIN authors ON (authors_books.author_id = authors.id)
                WHERE books.id = @bookId;";

            cmd.Parameters.AddWithValue("@bookId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Author> allAuthors = new List<Author> {};
            while(rdr.Read())
            {
                int authorId = rdr.GetInt32(0);
                string authorLast = rdr.GetString(1);
                string authorGiven = rdr.GetString(2);
                allAuthors.Add(new Author(authorLast, authorGiven, authorId));
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allAuthors;
        }
    }
}
