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
    // This book gets a list of authors for a single book.
    public List<Author> GetBookAuthors()
    {
        List<Author> authors = new List<Author> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT authors.* FROM authors JOIN books_authors ON (authors.id = books_authors.author_id) WHERE books_authors.book_id;";
        MySqlParameter thisId = new MySqlParameter("@thisId", _id);
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            int authorId = rdr.GetInt32(0);
            string first = rdr.GetString(1);
            string last = rdr.GetString(2);
            Author author = new Author(first, last, authorId);
            authors.Add(author);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }        
        return authors;
    }
    // This method will set a single book author for a single book.  Needs to be called multiple times for multiple authors of a single book.
    public void SetBookAuthor(int authorId)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO books_authors (book_id, author_id) VALUES (@bookId, @authorId);";
        MySqlParameter bookId = new MySqlParameter("@bookId", _id);
        MySqlParameter authorIdParameter = new MySqlParameter("@authorId", authorId);
        cmd.Parameters.Add(bookId);
        cmd.Parameters.Add(authorIdParameter);
        cmd.ExecuteNonQuery();
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
    public int GetAvailable()
    {   
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @thisId;";
        MySqlParameter thisId = new MySqlParameter("@thisId", this._id);
        cmd.Parameters.Add(thisId);
        int available = 0;
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            available = rdr.GetInt32(3);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return available;                    
    }
    public int GetCopiesId()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @thisId;";
        MySqlParameter thisId = new MySqlParameter("@thisId", this._id);
        cmd.Parameters.Add(thisId);
        int copiesId = 0;
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            copiesId = rdr.GetInt32(0);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return copiesId;
    }
    public int GetCheckOutId(int patronId)
    {
        int copiesId = GetCopiesId();
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM checkouts WHERE patron_id = @patronId AND copies_id = @copiesId;";
        MySqlParameter patronIdParameter = new MySqlParameter("@patronId", patronId);
        MySqlParameter copiesIdParameter = new MySqlParameter("@copiesId", copiesId);
        cmd.Parameters.Add(patronIdParameter);
        cmd.Parameters.Add(copiesIdParameter);
        int checkOutId = 0;
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            checkOutId = rdr.GetInt32(0);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return checkOutId;        
    }
    public DateTime GetDueDate(int patronId)
    {
        int copiesId = GetCopiesId();
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM checkouts WHERE patron_id = @patronId AND copies_id = @copiesId;";
        MySqlParameter patronIdParameter = new MySqlParameter("@patronId", patronId);
        MySqlParameter copiesIdParameter = new MySqlParameter("@copiesId", copiesId);
        cmd.Parameters.Add(patronIdParameter);
        cmd.Parameters.Add(copiesIdParameter);
        DateTime dueDate = new DateTime();
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            dueDate = rdr.GetDateTime(3);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return dueDate;             
    }
    public DateTime GetReturnedDate(int patronId)
    {
        int copiesId = GetCopiesId();
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM checkouts WHERE patron_id = @patronId AND copies_id = @copiesId;";
        MySqlParameter patronIdParameter = new MySqlParameter("@patronId", patronId);
        MySqlParameter copiesIdParameter = new MySqlParameter("@copiesId", copiesId);
        cmd.Parameters.Add(patronIdParameter);
        cmd.Parameters.Add(copiesIdParameter);
        DateTime returnedDate = new DateTime();
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            returnedDate = rdr.GetDateTime(5);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return returnedDate;             
    }
    public bool GetReturnedStatus(int patronId)
    {
        int copiesId = GetCopiesId();
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM checkouts WHERE patron_id = @patronId AND copies_id = @copiesId;";
        MySqlParameter patronIdParameter = new MySqlParameter("@patronId", patronId);
        MySqlParameter copiesIdParameter = new MySqlParameter("@copiesId", copiesId);
        cmd.Parameters.Add(patronIdParameter);
        cmd.Parameters.Add(copiesIdParameter);
        bool returned = false;
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            returned = rdr.GetBoolean(6);
        }
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
        return returned;             
    }
    public void CheckOut(int patronId)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE copies SET available = @available WHERE id = @copiesId; INSERT INTO checkouts (patron_id, copies_id, due_date, checkout_date, returned) VALUES (@thisPatronId, @copiesId, @dueDate, @checkoutDate, @returned);";
        MySqlParameter available = new MySqlParameter("@available", GetAvailable() - 1);
        // MySqlParameter thisBookId = new MySqlParameter("@thisBookId", _id);
        MySqlParameter thisPatronId = new MySqlParameter("@thisPatronId", patronId);
        MySqlParameter copiesId = new MySqlParameter("@copiesId", GetCopiesId());
        MySqlParameter dueDate = new MySqlParameter("@dueDate", DateTime.Now.AddDays(14));
        MySqlParameter checkoutDate = new MySqlParameter("@checkoutDate", DateTime.Now);
        MySqlParameter returned = new MySqlParameter("@returned", false);
        cmd.Parameters.Add(available);
        // cmd.Parameters.Add(thisBookId);
        cmd.Parameters.Add(thisPatronId);
        cmd.Parameters.Add(copiesId);
        cmd.Parameters.Add(dueDate);
        cmd.Parameters.Add(checkoutDate);
        cmd.Parameters.Add(returned);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    // Checkin changes checkouts table - 
    //  sets returned_date = today
    //  sets returned to true
    // and copies table adds 1 to available
    public void CheckIn(int checkoutId, int patronId)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE copies SET available = @available WHERE id = @copiesId;
            UPDATE checkouts SET returned_date = @returnedDate, returned = @returned WHERE copies_id = @copiesId;";
        MySqlParameter available = new MySqlParameter("@available", GetAvailable() + 1);
        // MySqlParameter thisBookId = new MySqlParameter("@thisBookId", _id);
        MySqlParameter copiesId = new MySqlParameter("@copiesId", GetCopiesId());
        MySqlParameter returnedDate = new MySqlParameter("@returnedDate", DateTime.Now);
        MySqlParameter returned = new MySqlParameter("@returned", true);
        cmd.Parameters.Add(available);
        // cmd.Parameters.Add(thisBookId);
        cmd.Parameters.Add(copiesId);
        cmd.Parameters.Add(returnedDate);
        cmd.Parameters.Add(returned);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}
