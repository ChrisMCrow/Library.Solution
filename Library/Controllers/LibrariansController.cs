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


// CHECKOUT
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


//CARDHOLDERS
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

        [HttpGet("/librarians/books")]
        public ActionResult Books()
        {
            return View();
        }

        [HttpPost("/librarians/books/add")]
        public ActionResult AddBook(string title, string author1, string author2, string cost, string totalCount)
        {
            float costFloat = float.Parse(cost);
            int totalInt = int.Parse(totalCount);
            Book newBook = new Book(title, totalInt, costFloat);
            newBook.Save();
            if (author1 != null)
            {
                int author1Id = int.Parse(author1);
                newBook.AddAuthor(author1Id);
            }
            if (author2 != null)
            {
                int author2Id = int.Parse(author2);
                newBook.AddAuthor(author2Id);
            }
            return RedirectToAction("Books");
        }

        [HttpGet("/librarians/books/lookup")]
        public ActionResult LookupBook(int id)
        {
            Book foundBook = Book.Find(id);
            return View("Books", foundBook);
        }


        [HttpPost("/librarians/books/lookup")]
        public ActionResult LookupBook(string bookId)
        {
            Book foundBook = Book.Find(int.Parse(bookId));
            return View("Books", foundBook);
        }

        [HttpPost("/librarians/books/update/{id}")]
        public ActionResult UpdateBook(string title, string author, string cost, string totalCount, int id)
        {
            Book foundBook = Book.Find(id);
            float costFloat = float.Parse(cost);
            int totalInt = int.Parse(totalCount);
            foundBook.Update(title, totalInt, costFloat);
            if (author != null)
            {
                int authorId = int.Parse(author);
                foundBook.AddAuthor(authorId);
            }
            return View("Books", foundBook);
        }

        [HttpGet("/librarians/books/{bookId}/delete/{authorId}")]
        public ActionResult RemoveBookAuthor(int bookId, int authorId)
        {
            Author.RemoveBookAuthor(bookId, authorId);
            return RedirectToAction("LookupBook", new{id = bookId});
        }
    }
}
