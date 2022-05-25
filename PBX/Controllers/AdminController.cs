using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBX.Models;

namespace PBX.Controllers
{
    public class AdminController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                ViewBag.reports = _db.Zgloszenie.Count();
                ViewBag.ads = _db.Ogloszenie.Count();
                ViewBag.users = _db.Uzytkownik.Count();
                ViewBag.deleted = _db.Usunieci.Count();
                ViewBag.categories = _db.Kategoria.Count();
                return View();
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Reports
        public ActionResult Reports()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Zgloszenie.ToList());
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/DeleteReport
        public ActionResult DeleteReport(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Zgloszenie z = _db.Zgloszenie.Find(id);
                _db.Zgloszenie.Remove(z);
                _db.SaveChanges();
                return RedirectToAction("Reports");
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/AdDetails
        public ActionResult AdDetails(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Ogloszenie o = _db.Ogloszenie.Find(id);
                ViewBag.userId = o.Uzytkownik.id;
                ViewBag.userName = o.Uzytkownik.imie;
                return View(o);
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/DeleteAd
        public ActionResult DeleteAd(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Ogloszenie o = _db.Ogloszenie.Find(id);
                List<Chat> chats = _db.Chat.Where(ch => ch.ogloszenie_id == o.id).ToList();
                foreach(Chat chat in chats)
                {
                    List<Wiadomosc> messages = _db.Wiadomosc.Where(w => w.chat_id == chat.id).ToList();
                    foreach(Wiadomosc message in messages)
                    {
                        _db.Wiadomosc.Remove(message);
                    }
                    _db.Chat.Remove(chat);
                }
                List<Zgloszenie> reports = _db.Zgloszenie.Where(z => z.Ogloszenie_id==o.id).ToList();
                foreach(Zgloszenie z in reports)
                {
                    _db.Zgloszenie.Remove(z);
                }
                _db.Ogloszenie.Remove(o);
                _db.SaveChanges();
                return RedirectToAction("Ads");
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Ads
        public ActionResult Ads()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Ogloszenie.ToList());
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Users
        public ActionResult Users()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Uzytkownik.ToList());
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/UserDetails
        public ActionResult UserDetails(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Uzytkownik u = _db.Uzytkownik.Find(id);
                return View(u);
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/DeleteUser
        public ActionResult DeleteUser(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Uzytkownik u = _db.Uzytkownik.Find(id);
                List<Chat> chats = _db.Chat.Where(ch => ch.zainteresowany_id == u.id || ch.Ogloszenie.wystawil_id==u.id).ToList();
                foreach (Chat chat in chats)
                {
                    List<Wiadomosc> messages = _db.Wiadomosc.Where(w => w.chat_id == chat.id).ToList();
                    foreach (Wiadomosc message in messages)
                    {
                        _db.Wiadomosc.Remove(message);
                    }
                    _db.Chat.Remove(chat);
                }
                List<Zgloszenie> reports = _db.Zgloszenie.Where(z => z.zglaszajacy_id == u.id || z.zgloszony_id == u.id).ToList();
                foreach (Zgloszenie z in reports)
                {
                    _db.Zgloszenie.Remove(z);
                }
                List<Ocena> rates = _db.Ocena.Where(o => o.ocena_dla_id == u.id || o.ocena_od_id == u.id).ToList();
                foreach (Ocena o in rates)
                {
                    _db.Ocena.Remove(o);
                }
                List<Ulubiona> favs = _db.Ulubiona.Where(ul => ul.uzytkownik_id == u.id || ul.Ogloszenie.wystawil_id==u.id).ToList();
                foreach (Ulubiona f in favs)
                {
                    _db.Ulubiona.Remove(f);
                }
                List<Ogloszenie> ads = _db.Ogloszenie.Where(o => o.wystawil_id == u.id).ToList();
                foreach (Ogloszenie o in ads)
                {
                    _db.Ogloszenie.Remove(o);
                }
                _db.Uzytkownik.Remove(u);
                _db.Usunieci.Add(new Usunieci
                {
                    id = u.id,
                    imie = u.imie,
                    nr_tel = u.nr_tel,
                    email = u.email,
                    haslo = u.haslo,
                    dolaczono = u.dolaczono,
                    usunieto = DateTime.Now
                });
                _db.SaveChanges();
                return RedirectToAction("Users");
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Users
        public ActionResult DeletedUsers()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Usunieci.ToList());
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Admin/DeleteUser
        public ActionResult RestoreUser(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                Usunieci u = _db.Usunieci.Find(id);
                _db.Usunieci.Remove(u);
                _db.Uzytkownik.Add(new Uzytkownik
                {
                    id = u.id,
                    imie = u.imie,
                    nr_tel = u.nr_tel,
                    email = u.email,
                    haslo = u.haslo,
                    dolaczono = u.dolaczono
                });
                _db.SaveChanges();
                return RedirectToAction("Users");
            }
            else return RedirectToAction("Login", "Account");
        }
    }
}