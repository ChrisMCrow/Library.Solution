using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class LibrariansController : Controller
    {
        [HttpGet("/librarians")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
