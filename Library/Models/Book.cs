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
        MySqlParameter title = new MySqlParameter();
        title.ParameterName = "@title";
        title.Value = this._title;
        MySqlParameter year = new MySqlParameter();
        year.ParameterName = "@year";
        year.Value = this._year;
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
        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
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
        cmd.CommandText = @"DELETE FROM books;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}
