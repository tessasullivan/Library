using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.TestTools
{
  [TestClass]
  public class AuthorTest : IDisposable
  {
    public void Dispose()
    {
      Author.ClearAll();
    }
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;convert zero datetime=True";
    }
  
    [TestMethod]
    public void Find_ReturnsCorrectAuthor_Author()
    {
      string first = "William";
      string last = "Shakespeare";
      Author author = new Author(first, last);
      author.Save();
      Author foundAuthor = Author.Find(author.GetId());
      Assert.AreEqual(author, foundAuthor);
    }

    [TestMethod]
    public void GetAll_ReturnsAllAuthors_AuthorsList()
    {
      string first = "William";
      string last = "Shakespeare";
      Author author = new Author(first, last);
      author.Save();
      string firstTwo = "Christopher";
      string lastTwo = "Marlowe";
      Author authorTwo = new Author(firstTwo, lastTwo);
      authorTwo.Save();
      List<Author> expected = new List<Author>{author, authorTwo};
      List<Author> result = Author.GetAll();
      CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Delete_DeletesAuthor_EmptyList()
    {
      string first = "William";
      string last = "Shakespeare";
      Author author = new Author(first, last);
      author.Save();
      author.Delete();
      List<Author> expected = new List<Author>{};
      List<Author> result = Author.GetAll();
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Edit_EditsAuthor_Author()
    {
      string first = "William";
      string last = "Shakespeare";
      Author author = new Author(first, last);
      author.Save();

      string newFirst = "Charles";
      string newLast = "Dickens";
      Author expectedResult = new Author(newFirst, newLast);
      author.Edit(newFirst, newLast);
      Author actualResult = new Author(author.GetFirst(), author.GetLast());
      Assert.AreEqual(expectedResult, actualResult);
    }
    // Set up author and 2 books, set up link between them, test list of given 2 books with list returned by method
    
    [TestMethod]
    public void GetBooks_ReturnsBooksBySingleAuthor_BookList()
    {
      string first = "William";
      string last = "Shakespeare";
      Author author = new Author(first, last);
      author.Save();

      string title1 = "Hamlet";
      int year1 = 1987;
      Book book1 = new Book(title1, year1);
      book1.Save();
      book1.SetBookAuthor(author.GetId());

      string title2 = "The Tempest";
      int year2 = 1988;
      Book book2 = new Book(title2, year2);
      book2.Save();
      book2.SetBookAuthor(author.GetId());

      List<Book> expectedResult = new List<Book> {book1, book2};
      List<Book> actualResult = author.GetBooks();

      CollectionAssert.AreEqual(expectedResult, actualResult);  
    }        
  }
}
    