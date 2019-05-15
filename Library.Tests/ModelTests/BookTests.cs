using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.TestTools
{
  [TestClass]
  public class BookTest : IDisposable
  {
    public void Dispose()
    {
      Book.ClearAll();
    }
    public BookTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;convert zero datetime=True";
    }
    
    [TestMethod]
    public void Find_ReturnsCorrectBook_Book()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      Book foundBook = Book.Find(book.GetId());
      Assert.AreEqual(book, foundBook);
    }
  }
}