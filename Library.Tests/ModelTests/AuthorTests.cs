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
  }
}
    