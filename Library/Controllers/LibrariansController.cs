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

        [HttpPost("/librarians/return/lookup")]
        public ActionResult LookupCheckout(string patronId)
        {
            List<Checkout> patronCheckouts = Patron.Find(int.Parse(patronId)).GetCurrentCheckouts();
            return View("Details", patronCheckouts);
        }

        [HttpPost("/librarians/return")]
        public ActionResult ReturnBook(string checkoutId)
        {
            Checkout checkoutToReturn = Checkout.Find(int.Parse(checkoutId));
            checkoutToReturn.CheckIn();
            Book bookReturned = Book.Find(checkoutToReturn.BookId);
            bookReturned.BookReturn();
            return View("Details", checkoutToReturn);
        }

        [HttpGet("/librarians/cardholder")]
        public ActionResult CardHolders()
        {
            return View();
        }

        [HttpPost("/librarians/cardholder/add")]
        public ActionResult AddPatron(string lastName, string firstName)
        {
            Patron newPatron = new Patron(lastName, firstName);
            newPatron.Save();
            return View("CardHolders", newPatron);
        }

        [HttpPost("/librarians/cardholder/lookup")]
        public ActionResult LookupPatron(string patronId)
        {
            Patron foundPatron = Patron.Find(int.Parse(patronId));
            return View("CardHolders", foundPatron);
        }

        [HttpPost("/librarians/cardholder/update/{id}")]
        public ActionResult UpdatePatron(string lastName, string firstName, int id)
        {
            Patron foundPatron = Patron.Find(id);
            foundPatron.Update(lastName, firstName);
            return View("CardHolders", foundPatron);
        }


    }
}
