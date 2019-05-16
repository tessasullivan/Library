using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;

namespace Library.Controllers
{
  public class StaffBooksController : Controller
  {

    [HttpGet("/staff/books/ShowAll")]
    public ActionResult ShowAll()
    {
        return View();
    }

    [HttpGet("/staff/books/{id}")]
    public ActionResult Show(int id)
    {
      Book thisBook = Book.Find(id);
      return View(thisBook);
    }

    [HttpPost("/staff/books")]
    public ActionResult Create(string title, int year, string first1, string last1, string first2, string last2, string first3, string last3)
    {
      // Check if combo of book & year exists, if so, error
      // Otherwise add book and save it
      // Check if author exists, if not, then create them, if so, then get the author id
      // If author field is blank, skip it

      // Run book.SetBookAuthor for each author
      // Run book.SetStock

      // Show book

      // Otherwise add book and save it
      Book newBook = new Book(title, year);
      newBook.Save();

      // Check if author exists, if not, then create them, if so, then get the author id
      Author author1 = new Author(first1, last1);
      






      return View("Show");
    }
  }
}