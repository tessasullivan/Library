using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;

namespace Library.Controllers
{
  public class StaffController : Controller
  {

    [HttpGet("/staff")]
    public ActionResult Index()
    {
      return View();
    }


  }
}