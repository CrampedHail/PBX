using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PBX.Models;

namespace PBX.Controllers
{
    public class AdvertisementController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        private readonly string[] typy = new string[] { "Oferta sprzedaży", "Oferta wymiany", "Oferta kupna", "Oferta pracy", "Poszukiwanie pracy" };
    // GET: Advertisement
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
        
        // GET: Advertisement/Search
        public ActionResult Search(FormCollection collection)
        {
            string searchQuery = collection["query"];
            ViewBag.searched = searchQuery==null || searchQuery==""? null : searchQuery;
            ViewBag.kategorie = _db.Kategoria.ToList();
            ViewBag.typy = typy.ToList();
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            List<Ogloszenie> results = searchQuery == null || searchQuery == "" ?
                _db.Ogloszenie.OrderBy(o=>o.dodano).ToList() :
                _db.Ogloszenie.Where(o => 
                    o.nazwa.Contains(searchQuery) || 
                    o.opis.Contains(searchQuery) || 
                    o.Kategoria.nazwa.Contains(searchQuery) || 
                    o.typ.Contains(searchQuery))
                .OrderBy(o => o.dodano).ToList();
            results.Reverse();
            string sort = collection["sort-type"];
            ViewBag.sort = sort;    
            switch(sort)
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
            ViewBag.category = category;
            if (category!=null && category.Length > 0)
            {
                results = results.Where(o => o.Kategoria.nazwa.Equals(collection["category"])).ToList();
            }
            string type = collection["ad-type"] != null && collection["ad-type"].Equals("Oferta sprzedaży")
                ? "Oferta sprzedazy"
                : collection["ad-type"];
            ViewBag.type = type;    
            if (type != null && !type.Equals("Wybierz typ"))
            {
                results = results.Where(o => o.typ.Equals(type)).ToList();
            }
            try
            {
                int price_from = Int32.Parse(collection["price-from"]) >= 0
                    ? Int32.Parse(collection["price-from"])
                    : 0;
                ViewBag.price_from = price_from;
                results = results.Where(o => o.cena > price_from).ToList();
            }
            catch { }
            try
            {
                int price_to = Int32.Parse(collection["price-to"]) >= 0
                    ? Int32.Parse(collection["price-to"])
                    : -1;
                ViewBag.price_to = price_to > 0 ? price_to.ToString() : " ";
                results = results.Where(o => o.cena <= price_to).ToList();
            }
            catch { }

            return View(results);
        }

        /*public PartialViewResult SearchResults(FormCollection collection)
        {
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
                .OrderBy(o => o.dodano).ToList();
            results.Reverse();
            string sort = collection["sort-type"];
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
            ViewBag.category = category;
            if (category != null && category.Length > 0)
            {
                results = results.Where(o => o.Kategoria.nazwa.Equals(collection["category"])).ToList();
            }
            string type = collection["ad-type"] != null && collection["ad-type"].Equals("Oferta sprzedaży")
                ? "Oferta sprzedazy"
                : collection["ad-type"];
            ViewBag.type = type;
            if (type != null && !type.Equals("Wybierz typ"))
            {
                results = results.Where(o => o.typ.Equals(type)).ToList();
            }
            try
            {
                int price_from = Int32.Parse(collection["price-from"]) >= 0
                    ? Int32.Parse(collection["price-from"])
                    : 0;
                ViewBag.price_from = price_from;
                results = results.Where(o => o.cena > price_from).ToList();
            }
            catch { }
            try
            {
                int price_to = Int32.Parse(collection["price-to"]) >= 0
                    ? Int32.Parse(collection["price-to"])
                    : -1;
                ViewBag.price_to = price_to > 0 ? price_to.ToString() : " ";
                results = results.Where(o => o.cena <= price_to).ToList();
            }
            catch { }
            return PartialView(results);
        }*/

        // GET: Advertisement/AddFavorite/5
        public ActionResult AddFavorite(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                _db.Ulubiona.Add(new Ulubiona()
                {
                    uzytkownik_id = user.id,
                    ogloszenie_id = id
                });
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Advertisement/RemoveFavorite/5
        public ActionResult RemoveFavorite(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Ulubiona ul = _db.Ulubiona.Where(u => u.ogloszenie_id == id && u.uzytkownik_id == user.id).First();
                _db.Ulubiona.Remove(ul);
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            else return RedirectToAction("Login", "Account");
        }
        // GET: Advertisement
        public ActionResult MyFavorites()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                List<Ogloszenie> myFav = _db.Ulubiona.Where(u => u.uzytkownik_id==user.id).Select(u => u.Ogloszenie).ToList();
                return View(myFav);
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Advertisement/Details/5
        public ActionResult Details(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                ViewBag.fav = _db.Ulubiona.Where(u => u.uzytkownik_id == user.id && u.ogloszenie_id == id).Count();
                ViewBag.reported = _db.Zgloszenie.Where(z => z.Ogloszenie_id == id && z.zglaszajacy_id == user.id).Count() > 0;
                return View(_db.Ogloszenie.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Advertisement/MyAds
        public ActionResult MyAds()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                List<Ogloszenie> ogloszenia = _db.Ogloszenie.Where(o => o.wystawil_id == user.id).ToList();
                ogloszenia.Reverse();
                return View(ogloszenia);
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Advertisement/Create
        public ActionResult Create()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                ViewBag.kategorie = _db.Kategoria.ToList();
                ViewBag.typy = typy;
                return View(new Ogloszenie());
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Advertisement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Ogloszenie o)
        {
            try
            {
                string kategoria = collection["category"];
                o.nazwa = o.nazwa.Trim();
                int katFound = _db.Kategoria.Where(k => k.nazwa.Equals(kategoria)).Select(k => k.id).First();
                int kat_id = katFound > 0 ? katFound : -1;
                if (kat_id != null && kat_id >= 0) o.kategoria_id = kat_id;
                else throw new ArgumentNullException();
                o.opis = o.opis.Trim();
                o.aktywne = true;
                o.dodano = DateTime.Now;
                o.wystawil_id = (SharedSession["user"] as Uzytkownik).id;

                _db.Ogloszenie.Add(o);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Advertisement/Edit/5
        public ActionResult Edit(int id)
        {

            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Ogloszenie o = _db.Ogloszenie.Find(id);
                o.nazwa = o.nazwa.Trim();
                o.opis = o.opis.Trim();
                ViewBag.typy = typy;
                ViewBag.currTyp = typy.Contains(o.typ.Trim())? o.typ.Trim() : typy[0] ;
                ViewBag.kategorie = _db.Kategoria.ToList();
                ViewBag.currKat = o.Kategoria.nazwa.Trim();
                return View(o);
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Advertisement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Ogloszenie o, FormCollection collection)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                var originalOgl = _db.Ogloszenie.Find(id);
                try
                {
                    string kategoria = collection["category"];
                    int katFound = _db.Kategoria.Where(k => k.nazwa.Equals(kategoria)).Select(k => k.id).First();
                    int kat_id = katFound > 0 ? katFound : -1;
                    if (kat_id != null && kat_id >= 0) originalOgl.kategoria_id = kat_id;
                    else throw new ArgumentNullException();
                    originalOgl.typ = collection["type"];
                    o.typ = collection["type"];
                    bool valid = ModelState.IsValid;
                    if (TryUpdateModel(originalOgl, new string[] { "nazwa", "opis", "cena", "negocjacja", "pokaz_tel", "pokaz_email" } ))
                    {
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Edit", new {id=o.id});
                }
                catch
                {
                    return RedirectToAction("Edit", new { id = o.id });
                }
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Advertisement/Delete/5
        public ActionResult Delete(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                return View(_db.Ogloszenie.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Advertisement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Ogloszenie o)
        {
            try
            {
                // TODO: Add delete logic here
                _db.Ogloszenie.Remove(_db.Ogloszenie.Find(id));
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(o);
            }
        }
    }
}
