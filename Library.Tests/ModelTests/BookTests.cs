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
    [TestMethod]
    public void SetBookAuthor_SetsBookAuthor_Book()
    {
      string title = "Freedom and Necessity";
      int year = 1987;
      Book book = new Book(title, year);
      book.Save();
      string first = "Steven";
      string last = "Brust";
      Author author = new Author(first, last);
      author.Save();
      string first2 = "Emma";
      string last2 = "Bull";
      Author author2 = new Author(first2, last2);
      author2.Save();
      book.SetBookAuthor(author.GetId());
      book.SetBookAuthor(author2.GetId());
      List<Author> expected = new List<Author> {author, author2};
      List<Author> result = book.GetBookAuthors();
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void GetAvailable_GetsAvailableNumberOfBooks_Int()
    {
      string title = "Freedom and Necessity";
      int year = 1987;
      int number = 2;
      Book book = new Book(title, year);
      book.Save();
      book.SetStock(number);
      int expected = book.GetAvailable();
      Assert.AreEqual(number, expected);      

    }
    // [TestMethod]
    // public void GetCopiesId_GetsCopiesId_int()
    // {
    //   string title = "Freedom and Necessity";
    //   int year = 1987;
    //   int number = 2;
    //   Book book = new Book(title, year);
    //   book.Save();
    //   book.SetStock(number);
    //   int result = book.GetCopiesId();
    //   Assert.AreEqual(1, result);
    // }
    [TestMethod]
    public void Checkout_SetsCheckoutData_Data()
    {
      string title = "Freedom and Necessity";
      int year = 1987;
      int number = 2;
      int patronId = 2;
      DateTime today = DateTime.Now;
      string expectedDueDate = today.AddDays(14).ToString("yyyy-MM-dd");
      Book book = new Book(title, year);
      book.Save();
      book.SetStock(number);
      book.CheckOut(patronId);
      string actualDueDate = book.GetDueDate(patronId).ToString("yyyy-MM-dd");
      Assert.AreEqual(number - 1, book.GetAvailable());
      Assert.AreEqual(expectedDueDate, actualDueDate);
    }
  }
}