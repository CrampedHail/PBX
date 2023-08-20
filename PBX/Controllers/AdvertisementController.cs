using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PBX.Models;

namespace PBX.Controllers
{
    public class AdvertisementController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        private EmailSender emailSender = new EmailSender();
        private readonly string[] typy = new string[] { "Oferta sprzedaży", "Oferta wymiany", "Oferta kupna", "Oferta pracy", "Poszukiwanie pracy" };
        private readonly string[] statusyZamowien = new string[]
        { "Nowe zamówienie", "Zamówienie przygotowane", "Zamówienie wysłane", "W doręczeniu", "Zamówienie doręczone", "Zamówienie zakończone"};
        private readonly SelectListItem[] statusy = new SelectListItem[] 
        { 
            new SelectListItem() { Text="Nowe zamówienie", Value= "Nowe zamówienie" },
            new SelectListItem() { Text="Zamówienie przygotowane", Value= "Zamówienie przygotowane" },
            new SelectListItem() { Text="Zamówienie wysłane", Value= "Zamówienie wysłane" },
            new SelectListItem() { Text="W doręczeniu", Value= "W doręczeniu" },
            new SelectListItem() { Text="Zamówienie doręczone", Value= "Zamówienie doręczone" },
            new SelectListItem() { Text="Zamówienie odebrane", Value= "Zamówienie odebrane" },
            new SelectListItem() { Text="Zamówienie zakończone", Value= "Zamówienie zakończone" },
        };
    // GET: Advertisement
    public ActionResult Index()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            return View();
        }
        
        // GET: Advertisement/Search
        public ActionResult Search(FormCollection collection, int? page, string sortOrder, string query, string typ, string kategoria, int? price_od, int? price_do)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null) ViewBag.user = user;

            string searchQuery = collection["query"];
            if (ContainsVulgarism(searchQuery)) searchQuery = "";
            ViewBag.searched = searchQuery==null || searchQuery==""? null : searchQuery;
            ViewBag.kategorie = _db.Kategoria.ToList();
            ViewBag.typy = typy.ToList();

            List<Ogloszenie> results = searchQuery == null || searchQuery == "" ?
                _db.Ogloszenie.OrderBy(o=>o.dodano).ToList() :
                _db.Ogloszenie.Where(o => 
                    o.nazwa.Contains(searchQuery) || 
                    o.opis.Contains(searchQuery) || 
                    o.Kategoria.nazwa.Contains(searchQuery) || 
                    o.typ.Contains(searchQuery))
                .Where(o => o.aktywne)
                .OrderBy(o => o.dodano).ToList();
            results.Reverse();
            string sort = sortOrder==null || sortOrder=="" ? collection["sort-type"]: sortOrder;
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
            category = kategoria == null || kategoria == "" ? category : kategoria;
            ViewBag.category = category;
            if (category!=null && category.Length > 0)
            {
                int category_id = _db.Kategoria.Where(k => k.nazwa.Equals(category)).Select(k => k.id).First();
                List<int> categories = new List<int>(category_id);
                categories.AddRange(findAllSubcategories(category_id));
                results = results.Where(o => categories.Contains(o.kategoria_id)).ToList();
            }

            string type = collection["ad-type"];
            type = typ != null ? typ : type;
            type = type != null && type.Equals("Oferta sprzedazy") ? "Oferta sprzedaży" : type;
            type = type != null && type.Equals("Dowolny") ? null : type;
            ViewBag.type = type;    
            if (type != null && !type.Equals("Wybierz typ"))
            {
                type = type.Equals("Oferta sprzedaży")
                    ? "Oferta sprzedazy"
                    : collection["ad-type"];
                results = results.Where(o => o.typ.Equals(type)).ToList();
            }

            string location = collection["location"];
            if(location!=null && location != "")
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
            if(price_od.HasValue)
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

            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            int pageSize = 10;
            var res = from r in results select r;
            return View(res.ToPagedList(pageNumber, pageSize));
        }

        private List<int> findAllSubcategories(int id)
        {
            List<int> categories = new List<int>();
            categories.Add(id);
            foreach (Kategoria k in _db.Kategoria.Where(k => k.nadkategoria_id==id))
            {
                categories.AddRange(findAllSubcategories(k.id));
            }
            return categories;
        }


        // GET: Advertisement/AddFavorite/5
        public ActionResult AddFavorite(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            _db.Ulubiona.Add(new Ulubiona()
            {
                uzytkownik_id = user.id,
                ogloszenie_id = id
            });
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Advertisement/RemoveFavorite/5
        public ActionResult RemoveFavorite(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            Ulubiona ul = _db.Ulubiona.Where(u => u.ogloszenie_id == id && u.uzytkownik_id == user.id).First();
            _db.Ulubiona.Remove(ul);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }
        // GET: Advertisement
        public ActionResult MyFavorites()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            List<Ogloszenie> myFav = _db.Ulubiona.Where(u => u.uzytkownik_id==user.id).Select(u => u.Ogloszenie).ToList();
            return View(myFav);
        }

        // GET: Advertisement/Details/5
        public ActionResult Details(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            ViewBag.fav = _db.Ulubiona.Where(u => u.uzytkownik_id == user.id && u.ogloszenie_id == id).Count();
            ViewBag.reported = _db.Zgloszenie.Where(z => z.ogloszenie_id == id && z.zglaszajacy_id == user.id).Count() > 0;
            ViewBag.applied = _db.Aplikacja.Where(a => a.ogloszenie_id == id && a.uzytkownik_id == user.id).Count() > 0;
            ViewBag.addedToCart = _db.Koszyk.Where(k => k.ogloszenie_id == id && k.uzytkownik_id == user.id).Count() > 0;
            ViewBag.ordered = _db.Zamowienie.Where(z => z.ogloszenie_id == id && z.uzytkownik_id == user.id).Count() > 0;
            return View(_db.Ogloszenie.Find(id));
        }

        // GET: Advertisement/MyAds
        public ActionResult MyAds()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            List<Ogloszenie> ogloszenia = _db.Ogloszenie.Where(o => o.wystawil_id == user.id).ToList();
            ogloszenia.Reverse();
            return View(ogloszenia);
        }

        // GET: Advertisement/Create
        public ActionResult Create()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            ViewBag.kategorie = _db.Kategoria.ToList();
            ViewBag.typy = typy;
            List<string> locations = new List<string>();
            ViewBag.locations = _db.Ogloszenie
                .Where(o => o.wystawil_id == user.id)
                .Select(o => o.lokalizacja)
                .ToHashSet()
                .Take(3).ToList();
            return View();
        }

        // POST: Advertisement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase UploadedPicture, Ogloszenie o)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            ViewBag.kategorie = _db.Kategoria.ToList();
            ViewBag.typy = typy;

            if(o.cena<0) ModelState.AddModelError(nameof(o.cena), "Cena/Wynagrodzenie nie może być liczbą ujemną!");
            if (o.cena > 9999999) ModelState.AddModelError(nameof(o.cena), "Pole Cena/Wynagrodzenie jest za duże!");

            if (o.nazwa == null || o.nazwa.Length <= 0) ModelState.AddModelError(nameof(o.nazwa), "Pole z nazwą ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.nazwa)) ModelState.AddModelError(nameof(o.nazwa), "W podanej nazwie znajduje się nieładne słowo!");

            if (o.opis == null || o.opis.Length <= 0) ModelState.AddModelError(nameof(o.opis), "Pole z opisem ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.opis)) ModelState.AddModelError(nameof(o.opis), "W podanym opisie znajduje się nieładne słowo!");

            if (collection["category"] == null || collection["category"].Length <= 0) ModelState.AddModelError(nameof(o.kategoria_id), "Pole z kategorią ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(collection["category"])) ModelState.AddModelError(nameof(o.kategoria_id), "W podanej kategorii znajduje się nieładne słowo!");

            if (o.lokalizacja == null || o.lokalizacja.Length <= 0) ModelState.AddModelError(nameof(o.lokalizacja), "Pole z lokalizacją ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.lokalizacja)) ModelState.AddModelError(nameof(o.lokalizacja), "W podanej lokalizacji znajduje się nieładne słowo!");

            if (!ModelState.IsValid) return View(o);

            int katFound;
            try
            {
                string kategoria = collection["category"];
                katFound = _db.Kategoria.Where(k => k.nazwa.Equals(kategoria)).Select(k => k.id).First();
            }
            catch
            {
                ModelState.AddModelError(nameof(o.kategoria_id), "Podano nieprawidłową kategorię ogłoszenia.");
                return RedirectToAction("Create");
            }

            bool pictureIsNullOrTooSmall = UploadedPicture == null || UploadedPicture.ContentLength <= 0;
            bool pictureIsTooBig = UploadedPicture == null ? false : UploadedPicture.ContentLength >= 8388608;
            string fileExtension = UploadedPicture == null ? ".png" : Path.GetExtension(UploadedPicture.FileName);
            bool pictureWrongFileType = !fileExtension.Equals(".png") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".jpeg");
            if (pictureIsTooBig)
            {
                ModelState.AddModelError(nameof(o.zdjecie), "Wrzucone zdjęcie jest za duże! (Przyjmujemy zdjęcia do 8MB)");
                return View();
            }
            if (pictureWrongFileType)
            {
                ModelState.AddModelError(nameof(o.zdjecie), "Wrzucone zdjęcie ma złe rozszerzenie! (Przyjmujemy pliki .png, .jpg i .jpeg)");
                return View();
            }
            if (!pictureIsNullOrTooSmall)
            {
                using (Stream inputStream = UploadedPicture.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    o.zdjecie = memoryStream.ToArray();
                }
            }
            else
            {
                Image img = Image.FromFile(@"C:\Users\Paweł Brandt\Documents\GitHub\PBX\PBX\Images\no_image.png");
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    o.zdjecie = ms.ToArray();
                }
            }
            if (o.lokalizacja == null || o.lokalizacja.Length <= 1) o.lokalizacja = "Polska";

            o.nazwa = o.nazwa.Trim();
            o.opis = o.opis.Replace("\u200B", "").Trim();
            o.kategoria_id = katFound;
            o.aktywne = true;
            o.dodano = DateTime.Now;
            o.wystawil_id = user.id;

            try
            {
                _db.Ogloszenie.Add(o);
                _db.SaveChanges();
                return RedirectToAction("Created", new { id = o.id });
            }
            catch(Exception e)
            {
                ViewBag.user = user;
                ViewBag.kategorie = _db.Kategoria.ToList();
                ViewBag.typy = typy;
                return View(o);
            }
        }

        private bool ContainsVulgarism(string text)
        {
            if(string.IsNullOrEmpty(text)) return false;
            string[] words = System.IO.File.ReadAllLines(@"C:\Users\Paweł Brandt\Documents\GitHub\PBX\PBX\Content\ForbiddenWords\wulgaryzmy.txt");
            foreach(string word in words)
            {
                if(text.Contains(word)) return true;
            }
            return false;
        }

        public ActionResult Created(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            return View(id);
        }

        // GET: Advertisement/Edit/5
        public ActionResult Edit(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

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

        // POST: Advertisement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Ogloszenie o, FormCollection collection, HttpPostedFileBase UploadedPicture)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            ViewBag.typy = typy;
            ViewBag.currTyp = typy.Contains(collection["type"]) ? collection["type"].Trim() : typy[0];
            ViewBag.kategorie = _db.Kategoria.ToList();
            ViewBag.currKat = collection["category"].Trim();
            var originalAd = _db.Ogloszenie.Find(id);

            if (o.cena < 0) ModelState.AddModelError(nameof(o.cena), "Pole Cena/Wynagrodzenie nie może być liczbą ujemną!");
            if (o.cena > 9999999) ModelState.AddModelError(nameof(o.cena), "Pole Cena/Wynagrodzenie jest za duże!");

            if (o.nazwa == null || o.nazwa.Length <= 0) ModelState.AddModelError(nameof(o.nazwa), "Pole z nazwą ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.nazwa)) ModelState.AddModelError(nameof(o.nazwa), "W podanej nazwie znajduje się nieładne słowo!");

            if (o.opis == null || o.opis.Length <= 0) ModelState.AddModelError(nameof(o.opis), "Pole z opisem ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.opis)) ModelState.AddModelError(nameof(o.opis), "W podanym opisie znajduje się nieładne słowo!");

            if (collection["category"] == null || collection["category"].Length <= 0) ModelState.AddModelError(nameof(o.kategoria_id), "Pole z kategorią ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(collection["category"])) ModelState.AddModelError(nameof(o.kategoria_id), "W podanej kategorii znajduje się nieładne słowo!");

            if (o.lokalizacja == null || o.lokalizacja.Length <= 0) ModelState.AddModelError(nameof(o.lokalizacja), "Pole z lokalizacją ogłoszenia jest wymagane!");
            else if (ContainsVulgarism(o.lokalizacja)) ModelState.AddModelError(nameof(o.lokalizacja), "W podanej lokalizacji znajduje się nieładne słowo!");

            if (!ModelState.IsValid) return View(originalAd);

            try
            {
                int katFound;
                try
                {
                    string kategoria = collection["category"];
                    katFound = _db.Kategoria.Where(k => k.nazwa.Equals(kategoria)).Select(k => k.id).First();
                }
                catch
                {
                    ModelState.AddModelError(nameof(o.kategoria_id), "Podano nieprawidłową kategorię ogłoszenia.");
                    return View(o);
                }

                bool originalPictureIsTooSmalOrNull = originalAd.zdjecie == null || originalAd.zdjecie.Length <= 0;
                bool pictureIsNullOrTooSmall = UploadedPicture == null || UploadedPicture.ContentLength <= 0;
                bool pictureIsTooBig = UploadedPicture == null ? false : UploadedPicture.ContentLength >= 8388608;
                string fileExtension = UploadedPicture == null ? ".png" : Path.GetExtension(UploadedPicture.FileName);
                bool pictureWrongFileType = !fileExtension.Equals(".png") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".jpeg");
                if (pictureIsTooBig)
                {
                    ModelState.AddModelError(nameof(o.zdjecie), "Wrzucone zdjęcie jest za duże! (Przyjmujemy zdjęcia do 8MB)");
                    return View();
                }
                if (pictureWrongFileType)
                {
                    ModelState.AddModelError(nameof(o.zdjecie), "Wrzucone zdjęcie ma złe rozszerzenie! (Przyjmujemy pliki .png, .jpg i .jpeg)");
                    return View();
                }
                if (!pictureIsNullOrTooSmall)
                {
                    using (Stream inputStream = UploadedPicture.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        o.zdjecie = memoryStream.ToArray();
                        originalAd.zdjecie = o.zdjecie;
                    }
                }
                else if (originalPictureIsTooSmalOrNull)
                {
                    Image img = Image.FromFile(@"C:\Users\Paweł Brandt\Documents\GitHub\PBX\PBX\Images\no_image.png");
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        o.zdjecie = ms.ToArray();
                        originalAd.zdjecie = o.zdjecie;
                    }
                }
                if (o.lokalizacja == null || o.lokalizacja.Length <= 1) o.lokalizacja = "Polska";

                originalAd.typ = collection["type"];
                o.typ = collection["type"];
                bool valid = ModelState.IsValid;
                if (TryUpdateModel(originalAd, new string[] { "nazwa", "opis", "cena", "negocjacja", "pokaz_tel", "pokaz_email", "lokalizacja", "zdjecie", "auto_przedluzanie" } ))
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

        // GET: Advertisement/Delete/5
        public ActionResult Delete(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.user = user;
            return View(_db.Ogloszenie.Find(id));
        }

        // POST: Advertisement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Ogloszenie o)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            try
            {
                o = _db.Ogloszenie.Find(id);
                _db.Wiadomosc.RemoveRange(_db.Wiadomosc.Where(w => w.Chat.ogloszenie_id==o.id));
                _db.Chat.RemoveRange(_db.Chat.Where(ch => ch.ogloszenie_id==o.id));
                _db.Aplikacja.RemoveRange(_db.Aplikacja.Where(a => a.ogloszenie_id == o.id));
                _db.Zamowienie.RemoveRange(_db.Zamowienie.Where(z => z.ogloszenie_id == o.id));
                _db.Zgloszenie.RemoveRange(_db.Zgloszenie.Where(z => z.ogloszenie_id == o.id));
                _db.Ulubiona.RemoveRange(_db.Ulubiona.Where(u => u.ogloszenie_id == o.id));
                _db.Koszyk.RemoveRange(_db.Koszyk.Where(k => k.ogloszenie_id == o.id));
                _db.Ogloszenie.Remove(o);
                _db.SaveChanges();
                return RedirectToAction("MyAds");
            }
            catch
            {
                return View(o);
            }
        }

        public ActionResult Apply(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            if (_db.Aplikacja.Where(a => a.uzytkownik_id==user.id && a.ogloszenie_id==id).Count()>0)
                return RedirectToAction("Details", new { id = id});

            Ogloszenie o = _db.Ogloszenie.Where(og => og.id == id).FirstOrDefault();

            if (!o.typ.Equals("Oferta pracy")) return RedirectToAction("Index", "Home");

            return View(o);
        }

        [HttpPost]
        public ActionResult Apply(Ogloszenie o, HttpPostedFileBase UploadedCv)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            if (_db.Aplikacja.Where(a => a.uzytkownik_id == user.id && a.ogloszenie_id == o.id).Count() > 0)
                return RedirectToAction("Details", new { id = o.id });

            if (!o.typ.Equals("Oferta pracy")) return RedirectToAction("Index", "Home");

            if(UploadedCv==null)
            {
                ViewBag.Error = "Prosimy o wrzucenie pliku pdf z CV.";
                return View(o);
            }

            if (UploadedCv.ContentLength <= 0)
            {
                ViewBag.Error = "Podany plik z CV jest za mały.";
                return View(o);
            }

            if (UploadedCv.ContentLength >= 8388608)
            {
                ViewBag.Error = "Wrzucone zdjęcie jest za duże! (Przyjmujemy zdjęcia do 8MB)";
                return View(o);
            }

            if(!Path.GetExtension(UploadedCv.FileName).Equals(".pdf"))
            {
                ViewBag.Error = "Wrzucone zdjęcie ma złe rozszerzenie! (Przyjmujemy pliki .png, .jpg i .jpeg)";
                return View(o);
            }


            Aplikacja aplikacja = new Aplikacja();
            using (Stream inputStream = UploadedCv.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                aplikacja.cv = memoryStream.ToArray();
            }
            aplikacja.uzytkownik_id = user.id;
            aplikacja.ogloszenie_id = o.id;
            _db.Aplikacja.Add(aplikacja);
            _db.SaveChanges();

            return RedirectToAction("Details", new{ id=o.id });
        }

        public ActionResult WithdrawApplication(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Ogloszenie o = _db.Ogloszenie.Where(og => og.id == id).FirstOrDefault();
            if (!o.typ.Equals("Oferta pracy")) return RedirectToAction("Index", "Home");

            Aplikacja aplikacja = _db.Aplikacja.Where(a => a.ogloszenie_id == id && a.uzytkownik_id == user.id).First();
            if(aplikacja==null) return RedirectToAction("Index", "Home");

            _db.Aplikacja.Remove(aplikacja);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id=id });
        }

        public ActionResult Applications(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Ogloszenie o = _db.Ogloszenie.Where(og => og.id == id).FirstOrDefault();
            if (!o.typ.Equals("Oferta pracy")) return RedirectToAction("Index", "Home");

            List<Aplikacja> applications = _db.Aplikacja.Where(a => a.ogloszenie_id == o.id).ToList();
            return View(applications); 
        }
        public ActionResult DownloadCv(int ogloszenie_id, int aplikujacy_id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Aplikacja application = _db.Aplikacja
                .Where(a => a.ogloszenie_id == ogloszenie_id && a.uzytkownik_id == aplikujacy_id).First();
            if(application == null) return RedirectToAction("Applications", new { id=ogloszenie_id });

            String ogloszenie_name = application.Ogloszenie.nazwa;

            return File(application.cv, "application/pdf", application.Aplikujacy.imie+"-aplikacja na ogłoszenie pracy "+ogloszenie_name+".pdf");

        }

        public ActionResult AddToCart(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Ogloszenie o = _db.Ogloszenie.Where(ogl => ogl.id == id).First();
            if(o==null) return RedirectToAction("Details", new { id = o.id });

            if (_db.Koszyk.Where(k => k.ogloszenie_id == o.id && k.uzytkownik_id == user.id).Count() > 0)
                return RedirectToAction("Details", new { id = o.id });

            Koszyk koszyk = new Koszyk();
            koszyk.ogloszenie_id = o.id;
            koszyk.uzytkownik_id = user.id;
            _db.Koszyk.Add(koszyk);
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = o.id });
        }

        public ActionResult RemoveFromCart(int ogloszenie_id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            if (_db.Koszyk.Where(k => k.ogloszenie_id == ogloszenie_id && k.uzytkownik_id == user.id).Count() <= 0)
                return RedirectToAction("Details", "Advertisement", new { id=ogloszenie_id } );

            Koszyk koszyk = _db.Koszyk.Where(k => k.ogloszenie_id == ogloszenie_id && k.uzytkownik_id == user.id).First();
            _db.Koszyk.Remove(koszyk);
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = ogloszenie_id });
        }

        public ActionResult GetMyCart()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            return View(_db.Koszyk.Where(k => k.uzytkownik_id==user.id).ToList());
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            List<Koszyk> orders = _db.Koszyk.Where(_k => _k.uzytkownik_id==user.id).ToList();
            if (orders == null || orders.Count<=0) return RedirectToAction("GetMyCart");

            orders = orders.Where(o => o.Ogloszenie.typ.Equals("Oferta sprzedaży")).ToList();
            string message = "Dzień dobry "+user.imie+", oto twoje zamówienie:";
            double orderPriceSum = 0;
            foreach (Koszyk k in orders)
            {
                Ogloszenie ogloszenie = _db.Ogloszenie.Where(o => o.id == k.ogloszenie_id).First();
                if (ogloszenie == null) continue;
                Uzytkownik uzytkownik = _db.Uzytkownik.Where(u => u.id == k.uzytkownik_id).First();
                if (uzytkownik == null) continue;
                Zamowienie z = new Zamowienie();
                z.ogloszenie_id = k.ogloszenie_id;
                z.uzytkownik_id = k.uzytkownik_id;
                z.data = DateTime.Now;
                z.status = statusyZamowien[0];
                _db.Zamowienie.Add(z);
                _db.Koszyk.Remove(k);
                message += "</br>" + ogloszenie.nazwa+ " - " + ogloszenie.cena + "zł";
                orderPriceSum += ogloszenie.cena;
                emailSender.SendEmail(ogloszenie.Uzytkownik.email, 
                    "Nowe zamówienie!",
                    "Witaj "+ogloszenie.Uzytkownik.imie+"! Właśnie przyszło do Ciebie nowe zamówienie od użytkownika "+uzytkownik.imie+" dotyczące ogłoszenia \""+ogloszenie.nazwa+"\".</br>Zajrzyj do nas po więcej szczegółów.");
            }
            _db.SaveChanges();

            message += "</br><strong>Suma: "+orderPriceSum+"zł</strong></br></br><small style=\" font-size:10px; \">Jeśli zamówienie nie było złożone przez Ciebie prosimy o kontakt z administratorem serwisu.</small>";
            emailSender.SendEmail(user.email, "Dziękujemy za złożenie zamówienia!",
                message);

            return View();
        }

        public ActionResult Bought()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            return View(_db.Zamowienie.Where(z => z.uzytkownik_id == user.id).ToList());
        }

        public ActionResult Sold()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            ViewBag.statusy = statusy;

            return View(_db.Zamowienie.Where(z => z.Ogloszenie.wystawil_id == user.id).ToList());
        }
        public ActionResult ChangeOrderStatus(int id, string status)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Zamowienie zamowienie = _db.Zamowienie.Where(z => z.id == id).First();
            if (TryUpdateModel(zamowienie, new string[] { "status" }))
            {
                _db.SaveChanges();
                emailSender.SendEmail(zamowienie.Uzytkownik.email,
                    "Nowy status zamówienia!",
                    "<strong>To już niedługo!</strong></br>Twoje zamówienie dotyczące ogłoszenia \"" + zamowienie.Ogloszenie.nazwa + "\" właśnie zmieniło status na: " + zamowienie.status + ".");
            }

            return RedirectToAction("Sold");
        }
        public ActionResult OrderReceived(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Zamowienie zamowienie = _db.Zamowienie.Where(z => z.id == id).First();
            zamowienie.status = "Zamówienie odebrane";
            if (TryUpdateModel(zamowienie, new string[] { "status" }))
            {
                _db.SaveChanges();
                emailSender.SendEmail(zamowienie.Ogloszenie.Uzytkownik.email,
                    "Kupujący odebrał zamówione produkty!",
                    "<strong>Zakończ zamówienie!</strong></br>Zamówienie dotyczące Twojego ogłoszenia \"" + zamowienie.Ogloszenie.nazwa + "\" właśnie zostało odebrane.");
            }

            return RedirectToAction("Bought");
        }
        public ActionResult FinaliseOrder(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Zamowienie zamowienie = _db.Zamowienie.Where(z => z.id == id).First();
            zamowienie.status = "Zamówienie zakończone";
            if (TryUpdateModel(zamowienie, new string[] { "status" }))
            {
                Ogloszenie o = _db.Ogloszenie.Find(zamowienie.ogloszenie_id);
                o.aktywne = o.auto_przedluzanie;
                _db.SaveChanges();
                emailSender.SendEmail(zamowienie.Uzytkownik.email,
                    "Sprzedający zakończył zamówienie!",
                    "<strong>Wszystko gotowe!</strong></br>Twoje zamówienie dotyczące ogłoszenia \"" + zamowienie.Ogloszenie.nazwa + "\" właśnie zostało zakończone.");
            }

            return RedirectToAction("Sold");
        }
        public ActionResult ActivateAd(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;

            Ogloszenie o = _db.Ogloszenie.Find(id);
            o.aktywne = true;
            if (TryUpdateModel(o, new string[] { "aktywne" }))
            {
                _db.SaveChanges();
            }

            return RedirectToAction("MyAds");
        }
    }
}
