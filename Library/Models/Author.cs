using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _first;
    private string _last;

    public Author(string first, string last, int id = 0)
    {
      _id = id;
      _first = first;
      _last = last;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFirst()
    {
      return _first;
    }

    public void SetFirst(string first)
    {
      _first = first;
    }

    public string GetLast()
    {
      return _last;
    }

    public void SetLast(string last)
    {
      _last = last;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else 
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId() == newAuthor.GetId());
        bool firstEq = (this.GetFirst() == newAuthor.GetFirst());
        bool lastEq = (this.GetLast() == newAuthor.GetLast());
        return (idEquality && firstEq && lastEq);
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
      cmd.CommandText = @"INSERT INTO authors (first, last) VALUES (@first, @last);";
      MySqlParameter authorFirst = new MySqlParameter("@first", _first);
      MySqlParameter authorLast = new MySqlParameter("@last", _last);
      cmd.Parameters.Add(authorFirst);
      cmd.Parameters.Add(authorLast);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }   
    public static Author Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter("@thisId", id);
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      var authorId = 0;
      string authorFirst = "";
      string authorLast = "";
      while (rdr.Read())
      {
        authorId = rdr.GetInt32(0);
        authorFirst = rdr.GetString(1);
        authorLast = rdr.GetString(2);
      }
      Author foundAuthor = new Author(authorFirst, authorLast, authorId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundAuthor;
    }
    public static void ClearAll()
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
    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorFirst = rdr.GetString(1);
        string authorLast = rdr.GetString(2);
        Author newAuthor = new Author(authorFirst, authorLast, authorId);
        allAuthors.Add(newAuthor);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allAuthors;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter("@thisId", _id);
      cmd.Parameters.Add(thisId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}