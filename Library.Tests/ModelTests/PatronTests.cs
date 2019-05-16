using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.TestTools
{
  [TestClass]
  public class PatronTest : IDisposable
  {
    public void Dispose()
    {
      Patron.ClearAll();
    }
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;convert zero datetime=True";
    }

    [TestMethod]
    public void Find_ReturnsCorrectPatron_Patron()
    {
        string first = "Paige";
        string last = "Turner";
        Patron patron = new Patron(first, last);
        patron.Save();
        Patron foundPatron = Patron.Find(patron.GetId());
        Assert.AreEqual(patron, foundPatron);
    }
    [TestMethod]
    public void Delete_DeletesPatron_EmptyList()
    {
        string first = "Paige";
        string last = "Turner";
        Patron patron = new Patron(first, last);
        patron.Save();
        patron.Delete();
        List<Patron> expected = new List<Patron>{};
        List<Patron> result = Patron.GetAll();
        CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void GetCheckouts_ReturnsListOfCheckedOutBooks_BookList()
    {
        string title = "Freedom and Necessity";
        int year = 1987;
        int number = 2;
        Book book = new Book(title, year);
        book.Save();
        book.SetStock(number);
      
        string first = "Paige";
        string last = "Turner";
        Patron patron = new Patron(first, last);
        patron.Save();

        book.CheckOut(patron.GetId());

        List<Book> expected = new List<Book> {book};
        List<Book> actual = patron.GetCheckouts();
        CollectionAssert.AreEqual(expected, actual);
    }
  }
}