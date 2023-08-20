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
        private readonly string[] typy = new string[] { "Oferta sprzedaży", "Oferta wymiany", "Oferta kupna", "Oferta pracy", "Poszukiwanie pracy" };

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
        public ActionResult Reports(string content, string advertisement, string reported_user, string reporting_user)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin == null) return RedirectToAction("Login", "Account");
            
            ViewBag.Admin = admin;

            List<Zgloszenie> results = _db.Zgloszenie.ToList();

            if (content != null && !content.Equals(""))
            {
                results = results.Where(z => z.tresc.ToLower().Contains(content.ToLower())).ToList();
                ViewBag.content = content;
            }

            if (advertisement != null && !advertisement.Equals(""))
            {
                results = results.Where(z => z.Ogloszenie.nazwa.ToLower().Contains(advertisement.ToLower())).ToList();
                ViewBag.advertisement = advertisement;
            }

            if (reported_user != null && !reported_user.Equals(""))
            {
                results = results.Where(z => z.Zgloszony.imie.ToLower().Contains(reported_user.ToLower())).ToList();
                ViewBag.reported_user = reported_user;
            }

            if (reporting_user != null && !reporting_user.Equals(""))
            {
                results = results.Where(z => z.Zglaszajacy.imie.ToLower().Contains(reporting_user.ToLower())).ToList();
                ViewBag.reporting_user = reporting_user;
            }

            return View(results);
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
                List<Zgloszenie> reports = _db.Zgloszenie.Where(z => z.ogloszenie_id==o.id).ToList();
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
        public ActionResult Ads(FormCollection collection, string sortOrder, string typ, string kategoria, int? price_od, int? price_do)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;

                string searchQuery = collection["query"];
                ViewBag.searched = searchQuery == null || searchQuery == "" ? null : searchQuery;
                ViewBag.kategorie = _db.Kategoria.ToList();
                ViewBag.typy = typy.ToList();

                List<Ogloszenie> results = searchQuery == null || searchQuery == "" ?
                _db.Ogloszenie.OrderBy(o => o.dodano).ToList() :
                _db.Ogloszenie.Where(o =>
                    o.nazwa.Contains(searchQuery) ||
                    o.opis.Contains(searchQuery) ||
                    o.Kategoria.nazwa.Contains(searchQuery) ||
                    o.typ.Contains(searchQuery))
                .Where(o => o.aktywne)
                .OrderBy(o => o.dodano).ToList();
                results.Reverse();
                string sort = sortOrder == null || sortOrder == "" ? collection["sort-type"] : sortOrder;
                ViewBag.sort = sort;
                switch (sort)
                {
                    case "od najstarszych":
                        {
                            results.Reverse();
                            break;
                        }
                    case "od najtańszych":
                        {
                            results = results.OrderBy(o => o.cena).ToList();
                            break;
                        }
                    case "od najdroższych":
                        {
                            results = results.OrderBy(o => o.cena).ToList();
                            results.Reverse();
                            break;
                        }
                    case "nazwą alfabetycznie":
                        {
                            results = results.OrderBy(o => o.nazwa).ToList();
                            break;
                        }
                }
                string category = _db.Kategoria.Select(k => k.nazwa).ToList().Contains(collection["category"])
                    ? collection["category"] : "";
                category = kategoria == null || kategoria == "" ? category : kategoria;
                ViewBag.category = category;
                if (category != null && category.Length > 0)
                {
                    int category_id = _db.Kategoria.Where(k => k.nazwa.Equals(category)).Select(k => k.id).First();
                    List<int> categories = new List<int>(category_id);
                    categories.AddRange(findAllSubcategories(category_id));
                    results = results.Where(o => categories.Contains(o.kategoria_id)).ToList();
                }

                string type = collection["ad-type"];
                type = typ != null ? typ : type;
                type = type != null && type.Equals("Dowolny") ? null : type;
                ViewBag.type = type;
                if (type != null && !type.Equals("Wybierz typ"))
                {
                    results = results.Where(o => o.typ.Equals(type)).ToList();
                }

                string location = collection["location"];
                if (location != null && location != "")
                {
                    results = results.Where(o => o.lokalizacja.Equals(location)).ToList();
                    ViewBag.location = location;
                }

                int price_from;
                try
                {
                    price_from = Int32.Parse(collection["price-from"]) >= 0
                        ? Int32.Parse(collection["price-from"])
                        : 0;
                    price_from = price_od.HasValue ? price_od.Value : price_from;
                    ViewBag.price_from = price_from;
                    results = results.Where(o => o.cena >= price_from).ToList();
                }
                catch { }
                if (price_od.HasValue)
                {
                    price_from = price_od.Value;
                    ViewBag.price_from = price_from;
                    results = results.Where(o => o.cena >= price_from).ToList();
                }

                int price_to;
                try
                {
                    price_to = Int32.Parse(collection["price-to"]) >= 0
                        ? Int32.Parse(collection["price-to"])
                        : -1;
                    ViewBag.price_to = price_to > 0 ? price_to.ToString() : " ";
                    results = results.Where(o => o.cena <= price_to).ToList();
                }
                catch { }
                if (price_do.HasValue)
                {
                    price_to = price_do.Value;
                    ViewBag.price_to = price_to;
                    results = results.Where(o => o.cena <= price_to).ToList();
                }

                ViewBag.found = results.Count();
                return View(results);
            }
            else return RedirectToAction("Login", "Account");
        }

        private List<int> findAllSubcategories(int id)
        {
            List<int> categories = new List<int>();
            categories.Add(id);
            foreach (Kategoria k in _db.Kategoria.Where(k => k.nadkategoria_id == id))
            {
                categories.AddRange(findAllSubcategories(k.id));
            }
            return categories;
        }

        // GET: Admin/Users
        public ActionResult Users(int? user_id, string user_name, string phone_number, string email, string joined_from, string joined_to)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin == null) return RedirectToAction("Login", "Account");
            
            ViewBag.Admin = admin;

            List<Uzytkownik> results = _db.Uzytkownik.ToList();

            if (user_id != null && user_id >= 0)
            {
                results = results.Where(u => u.id == user_id).ToList();
                ViewBag.user_id = user_id;
            }

            if (user_name != null && !user_name.Equals(""))
            {
                results = results.Where(u => u.imie.ToLower().Contains(user_name.ToLower())).ToList();
                ViewBag.user_name = user_name;
            }

            if (phone_number != null && !phone_number.Equals(""))
            {
                results = results.Where(u => u.nr_tel.ToLower().Contains(phone_number.ToLower())).ToList();
                ViewBag.phone_number = phone_number;
            }

            if (email != null && !email.Equals(""))
            {
                results = results.Where(u => u.email.ToLower().Contains(email.ToLower())).ToList();
                ViewBag.email = email;
            }

            if (joined_from != null && !joined_from.Equals(""))
            {
                try
                {
                    DateTime dolaczono_od = DateTime.Parse(joined_from);
                    results = results.Where(u => DateTime.Compare(u.dolaczono, dolaczono_od)>=0).ToList();
                    ViewBag.joined_from = joined_from;
                }
                catch{}
            }

            if (joined_to != null && !joined_to.Equals(""))
            {
                try
                {
                    DateTime dolaczono_do = DateTime.Parse(joined_to);
                    results = results.Where(u => DateTime.Compare(u.dolaczono, dolaczono_do) <= 0).ToList();
                    ViewBag.joined_to = joined_to;
                }
                catch { }
            }

            return View(results);
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
        public ActionResult DeletedUsers(int? user_id, string user_name, string phone_number, string email, string joined_from, string joined_to, string deleted_from, string deleted_to)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin == null) return RedirectToAction("Login", "Account");

            ViewBag.Admin = admin;

            List<Usunieci> results = _db.Usunieci.ToList();

            if (user_id != null && user_id >= 0)
            {
                results = results.Where(u => u.id == user_id).ToList();
                ViewBag.user_id = user_id;
            }

            if (user_name != null && !user_name.Equals(""))
            {
                results = results.Where(u => u.imie.ToLower().Contains(user_name.ToLower())).ToList();
                ViewBag.user_name = user_name;
            }

            if (phone_number != null && !phone_number.Equals(""))
            {
                results = results.Where(u => u.nr_tel.ToLower().Contains(phone_number.ToLower())).ToList();
                ViewBag.phone_number = phone_number;
            }

            if (email != null && !email.Equals(""))
            {
                results = results.Where(u => u.email.ToLower().Contains(email.ToLower())).ToList();
                ViewBag.email = email;
            }

            if (joined_from != null && !joined_from.Equals(""))
            {
                try
                {
                    DateTime dolaczono_od = DateTime.Parse(joined_from);
                    results = results.Where(u => DateTime.Compare(u.dolaczono, dolaczono_od) >= 0).ToList();
                    ViewBag.joined_from = joined_from;
                }
                catch { }
            }

            if (joined_to != null && !joined_to.Equals(""))
            {
                try
                {
                    DateTime dolaczono_do = DateTime.Parse(joined_to);
                    results = results.Where(u => DateTime.Compare(u.dolaczono, dolaczono_do) <= 0).ToList();
                    ViewBag.joined_to = joined_to;
                }
                catch { }
            }

            if (deleted_from != null && !deleted_from.Equals(""))
            {
                try
                {
                    DateTime usunieto_od = DateTime.Parse(deleted_from);
                    results = results.Where(u => DateTime.Compare(u.usunieto, usunieto_od) >= 0).ToList();
                    ViewBag.deleted_from = deleted_from;
                }
                catch { }
            }

            if (deleted_to != null && !deleted_to.Equals(""))
            {
                try
                {
                    DateTime usunieto_do = DateTime.Parse(deleted_to);
                    results = results.Where(u => DateTime.Compare(u.dolaczono, usunieto_do) <= 0).ToList();
                    ViewBag.deleted_to = deleted_to;
                }
                catch { }
            }

            return View(results);
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