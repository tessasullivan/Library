using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _firstName;
    private string _lastName;

    public Patron(string firstName, string lastName, int id=0)
    {
      _id = id;
      _firstName = firstName;
      _lastName = lastName;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public void SetFirstName(string firstName)
    {
      _firstName = firstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public void SetLastName(string lastName)
    {
      _lastName = lastName;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons (first, last) VALUES (@first, @last);";
      MySqlParameter first = new MySqlParameter("@first", _firstName);
      MySqlParameter last = new MySqlParameter("@last", _lastName);
      cmd.Parameters.Add(first);
      cmd.Parameters.Add(last);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string first = rdr.GetString(1);
        string last = rdr.GetString(2);
        Patron newPatron = new Patron(first, last, id);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }
    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter("@thisId", id);
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int patronId = 0;
      string patronFirst = "";
      string patronLast = "";
      while(rdr.Read())
      {
        patronId = rdr.GetInt32(0);
        patronFirst = rdr.GetString(1);
        patronLast = rdr.GetString(2);
      }
      Patron foundPatron = new Patron(patronFirst, patronLast, patronId);
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return foundPatron;
    }
    public static void ClearAll()
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
    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else 
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId() == newPatron.GetId());
        bool firstEq = (this.GetFirstName() == newPatron.GetFirstName());
        bool lastEq = (this.GetLastName() == newPatron.GetLastName());
        return (idEquality && firstEq && lastEq);
      }
    } 
    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter("@thisId", _id);
      cmd.Parameters.Add(thisId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Book> GetCheckouts()
    {
      List<Book> foundBooks = new List<Book>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM books JOIN copies ON (copies.book_id = books.id) JOIN checkouts ON (checkouts.copies_id = copies.id) JOIN patrons ON (checkouts.patron_id = patrons.id) WHERE patrons.id = @thisId;";
      MySqlParameter thisId = new MySqlParameter("@thisId", _id);
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        int year = rdr.GetInt32(2);
        Book book = new Book(title, year, bookId);
        foundBooks.Add(book);
      }
      return foundBooks;          
    }
  }
}
