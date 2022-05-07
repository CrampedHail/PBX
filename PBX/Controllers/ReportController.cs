using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBX.Models;

namespace PBX.Controllers
{
    public class ReportController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        // GET: Report
        public ActionResult Index()
        {

            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                return View();
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {

            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                return View();
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Report/Create
        public ActionResult Create(int id)
        {
            //Parametr to ogłoszenie_id

            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                Zgloszenie z = new Zgloszenie()
                {
                    Ogloszenie_id = id,
                    zglaszajacy_id = user.id,
                    zgloszony_id = _db.Ogloszenie.Find(id).Uzytkownik.id,
                    tresc = ""
                };
                ViewBag.user = user;
                return View(z);
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {

            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Zgloszenie z = new Zgloszenie()
                {
                    Ogloszenie_id = id,
                    zglaszajacy_id = user.id,
                    zgloszony_id = _db.Ogloszenie.Find(id).Uzytkownik.id,
                    tresc = collection["tresc"]
                };
                try
                {
                    
                    // TODO: Add insert logic here
                    if (ModelState.IsValid)
                    {
                        _db.Zgloszenie.Add(z);
                        _db.SaveChanges();
                        return RedirectToAction("Details", "Advertisement", new { id = z.Ogloszenie_id });
                    }
                    else return View(z);
                }
                catch
                {
                    return View(z);
                }
            }
            else return RedirectToAction("Login", "Account");
            
        }

        // GET: Report/Cancel/5
        public ActionResult Cancel(int id)
        {
            //Parametr to Ogloszenie_id
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Zgloszenie zg = _db.Zgloszenie.Where(z => z.Ogloszenie_id==id && z.zglaszajacy_id==user.id).First();
                _db.Zgloszenie.Remove(zg);
                _db.SaveChanges();
                return RedirectToAction("Details", "Advertisement", new { id=id });
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
