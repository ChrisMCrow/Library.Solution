using System.Collections.Generic;
using System;
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

        [HttpPost("/librarians/login")]
        public ActionResult LogInRedirect(string libraryNumber)
        {
            return RedirectToAction("Details");
        }

        [HttpGet("/librarians/dashboard")]
        public ActionResult Details()
        {
            return View();
        }

        [HttpPost("/librarians/checkout")]
        public ActionResult CreateCheckout(string bookId, string patronId, string dueDate)
        {
            DateTime checkoutDT = DateTime.Now;
            DateTime dueDT = DateTime.Now.AddDays(int.Parse(dueDate));
            Checkout newCheckout = new Checkout(int.Parse(bookId), int.Parse(patronId), checkoutDT, dueDT);
            newCheckout.Save();
            Book thisBook = Book.Find(int.Parse(bookId));
            Console.WriteLine(thisBook.Title);
            thisBook.BookCheckout();
            Console.WriteLine(thisBook.CurrentCount);
            return View("Details", newCheckout);
        }
    }
}
