using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class PatronsController : Controller
    {
        [HttpGet("/patrons")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost("/patrons/details")]
        public ActionResult LogInRedirect(string libraryNumber)
        {
            Patron newPatron = Patron.Find(int.Parse(libraryNumber));
            newPatron.HasOverDue();
            foreach (var checkout in newPatron.GetCurrentCheckouts())
            {
                checkout.IsReturned();
            }
            if (newPatron == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new {id = newPatron.Id});
            }
        }

        [HttpGet("/patrons/{id}")]
        public ActionResult Details(int id)
        {
            return View(Patron.Find(id));
        }

        [HttpGet("/patrons/{id}/history")]
        public ActionResult History(int id)
        {
            return View(Patron.Find(id));
        }
    }
}
