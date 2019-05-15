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

    [TestMethod]
    public void ClearAll_ClearsAllBooks_EmptyList()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      Book.ClearAll();
      List<Book> expected = new List<Book>{};
      List<Book> result = Book.GetAll();
      CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetAll_ReturnsAllBooks_BookList()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      string titleTwo = "The Tempest";
      int yearTwo = 1977;
      Book bookTwo = new Book(titleTwo, yearTwo);
      bookTwo.Save();
      List<Book> expected = new List<Book>{book, bookTwo};
      List<Book> result = Book.GetAll();
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Edit_EditsBookTitle_Book()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      string newTitle = "Hamlet: Prince of Denmark";
      book.Edit(newTitle, year);
      Book foundBook = Book.Find(book.GetId());
      Book expected = new Book(newTitle, year);
      Assert.AreEqual(foundBook.GetTitle(), expected.GetTitle());      
      Assert.AreEqual(foundBook.GetYear(), expected.GetYear());      
    }
    [TestMethod]
    public void Edit_EditsBookYear_Book()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      int newYear = 2000;
      book.Edit(title, newYear);
      Book foundBook = Book.Find(book.GetId());
      Book expected = new Book(title, newYear);
      Assert.AreEqual(foundBook.GetTitle(), expected.GetTitle());      
      Assert.AreEqual(foundBook.GetYear(), expected.GetYear());      
    }
    [TestMethod]
    public void Edit_EditsBookTitleAndYear_Book()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      string newTitle = "Hamlet: Prince of Denmark";
      int newYear = 2000;
      book.Edit(newTitle, newYear);
      Book foundBook = Book.Find(book.GetId());
      Book expected = new Book(newTitle, newYear);
      Assert.AreEqual(foundBook.GetTitle(), expected.GetTitle());      
      Assert.AreEqual(foundBook.GetYear(), expected.GetYear());      
    }
    [TestMethod]
    public void GetStock_GetsNumberOfBooks_Int()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      int number = 5;
      book.SetStock(number);
      int result = book.GetStock();
      Assert.AreEqual(number, result);
    }
    [TestMethod]
    public void SetStock_SetsNumberOfBooks_Int()
    {
      string title = "Hamlet";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      int number = 5;
      book.SetStock(number);
      int result = book.GetStock();
      Assert.AreEqual(number, result);
    }
  }
}