using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Book
  {
      private string _title;
      private int _year;
      private int _id;
  
    public Book (string title, int year, int id=0)
    {
        _title = title;
        _year = year;
        _id = id;
    }
    public string GetTitle()
    {
        return _title;
    }
    public void SetTitle(string title)
    {
        _title = title;
    }
    public int GetYear()
    {
        return _year;
    }
    public void SetYear(int year)
    {
        _year = year;
    }
    public int GetId()
    {
        return _id;
    }
    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Book))
      {
        return false;
      }
      else 
      {
        Book newBook= (Book) otherItem;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool titleEq = (this.GetTitle() == newBook.GetTitle());
        bool yearEq = (this.GetYear() == newBook.GetYear());
        return (idEquality && titleEq && yearEq);
      }
    } 
    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }
    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO books (title, year) VALUES (@title, @year);";
        MySqlParameter title = new MySqlParameter("@title", this._title);
        MySqlParameter year = new MySqlParameter("@year", this._year);
        cmd.Parameters.Add(title);
        cmd.Parameters.Add(year);
        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public static Book Find(int id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM books WHERE id = @thisId;";
        MySqlParameter thisId = new MySqlParameter("@thisId", id);
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int bookId = 0;
        string bookTitle = "";
        int year = 0;
        while (rdr.Read())
        {
            bookId = rdr.GetInt32(0);
            bookTitle = rdr.GetString(1);
            year = rdr.GetInt32(2);
        }
        Book foundBook = new Book(bookTitle, year, bookId);
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return foundBook;
    }

    public static List<Book> GetAll()
    {
        List<Book> allBooks = new List<Book>{};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM books;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            int bookId = rdr.GetInt32(0);
            string bookTitle = rdr.GetString(1);
            int bookYear = rdr.GetInt32(2);
            Book newBook = new Book(bookTitle, bookYear, bookId);
            allBooks.Add(newBook);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return allBooks;
    }
    public static void ClearAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM books;DELETE FROM copies;DELETE FROM books_authors;DELETE FROM categories_books;DELETE FROM books_authors;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public void Edit (string newTitle, int newYear)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE books SET title = @newTitle, year = @newYear WHERE id = @bookId;";
        MySqlParameter bookId = new MySqlParameter("@bookId", _id);
        cmd.Parameters.Add(bookId);
        MySqlParameter title = new MySqlParameter("@newTitle", newTitle);
        cmd.Parameters.Add(title);
        MySqlParameter year = new MySqlParameter("@newYear", newYear);
        cmd.Parameters.Add(year);
        cmd.ExecuteNonQuery();
        _title = newTitle;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public static void Delete(int id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM books WHERE id = @thisId;";
        MySqlParameter thisId = new MySqlParameter("@thisId", id);
        cmd.Parameters.Add(thisId);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public void SetStock(int number)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO copies (book_id, stock, available) VALUES
            (@bookId, @stock, @available);";
        MySqlParameter bookId = new MySqlParameter("@bookId", this._id);
        MySqlParameter stock = new MySqlParameter("@stock", number);
        MySqlParameter available = new MySqlParameter("@available", number);
        cmd.Parameters.Add(bookId);
        cmd.Parameters.Add(stock);
        cmd.Parameters.Add(available);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public int GetStock()
    {
        
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @thisId;";
        MySqlParameter thisId = new MySqlParameter("@thisId", this._id);
        cmd.Parameters.Add(thisId);
        int stock = 0;
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        
        Console.WriteLine(_id);
        while(rdr.Read())
        {
            stock = rdr.GetInt32(2);
        }
        Console.WriteLine(stock);
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return stock;                    
    }
  }
}
