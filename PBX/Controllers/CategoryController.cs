using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBX.Models;

namespace PBX.Controllers
{
    public class CategoryController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        // GET: Category
        public ActionResult Index()
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Kategoria.ToList());
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Kategoria.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Category/Create
        public ActionResult Create(string parentCategory="")
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                ViewBag.categories = _db.Kategoria.ToList();
                ViewBag.parentCategory = parentCategory != String.Empty ? parentCategory : "";
                return View();
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Kategoria kategoria)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                try
                {
                    if (_db.Kategoria.Where(k => k.nazwa.Equals(kategoria.nazwa)).Count() > 0) throw new IndexOutOfRangeException();
                    if (kategoria.nazwa==null || kategoria.nazwa.Equals(String.Empty)) throw new NullReferenceException();
                    int? nadkategoria_id = _db.Kategoria.
                        Where(k => k.nazwa.Equals(kategoria.Nadkategoria.nazwa)).
                        Select(k => k.id).Count()>0
                        ? _db.Kategoria.
                        Where(k => k.nazwa.Equals(kategoria.Nadkategoria.nazwa)).
                        Select(k => k.id).
                        First()
                        : -1;
                    if (nadkategoria_id<0) throw new FormatException();

                    kategoria.nadkategoria_id = nadkategoria_id>0
                        ? nadkategoria_id
                        : null;
                    kategoria.Nadkategoria = null;
                    if (ModelState.IsValid)
                    {
                        _db.Kategoria.Add(kategoria);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch(IndexOutOfRangeException)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Kategoria o podanej nazwie już istnieje.";
                    return View();
                }
                catch (NullReferenceException)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Nazwa kategorii jest wymagana.";
                    return View();
                }
                catch (FormatException)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Podana nadkategoria nie istnieje.";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Coś poszło nie tak, proszę spróbować później.";
                    return View();
                }
            }
            else return RedirectToAction("Login", "Account");
            
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                ViewBag.categories = _db.Kategoria.ToList();
                return View(_db.Kategoria.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, Kategoria kat)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                try
                {
                    // TODO: Add update logic here
                    if (_db.Kategoria.Where(k => k.nazwa.Equals(kat.nazwa)).Count() > 1) throw new IndexOutOfRangeException();
                    int? nadkategoria_id = _db.Kategoria.
                        Where(k => k.nazwa.Equals(kat.Nadkategoria.nazwa)).
                        Select(k => k.id).Count() > 0
                        ? _db.Kategoria.
                        Where(k => k.nazwa.Equals(kat.Nadkategoria.nazwa)).
                        Select(k => k.id).
                        First()
                        : -1;
                    if (nadkategoria_id < 0) throw new FormatException();
                    Kategoria originalKat = _db.Kategoria.Find(id);
                    if (TryUpdateModel(originalKat, new string[] { "nazwa", "nadkategoria_id" }))
                    {
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else return RedirectToAction("Edit", new { id = kat.id });
                }
                catch (IndexOutOfRangeException)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Kategoria o podanej nazwie już istnieje.";
                    return View();
                }
                catch (FormatException)
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Podana nadkategoria nie istnieje.";
                    return View();
                }
                catch
                {
                    ViewBag.categories = _db.Kategoria.ToList();
                    ViewBag.error = "Coś poszło nie tak, proszę spróbować później.";
                    return View();
                }
            }
            else return RedirectToAction("Login", "Account");
            
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                return View(_db.Kategoria.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Admin admin = SharedSession["admin"] as Admin;
            if (admin != null)
            {
                ViewBag.Admin = admin;
                try
                {
                    DeleteSubcategories(id);
                    _db.Kategoria.Remove(_db.Kategoria.Find(id));
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(_db.Kategoria.Find(id));
                }
            }
            else return RedirectToAction("Login", "Account");
        }

        private void DeleteSubcategories(int id)
        {
            List<Kategoria> subcategoriesFound = _db.Kategoria.Where(k => k.nadkategoria_id==id).ToList();
            if (subcategoriesFound.Count > 0) 
                foreach(Kategoria kat in subcategoriesFound)
                {
                    DeleteSubcategories(kat.id);
                    kat.nadkategoria_id = null;
                    _db.Kategoria.Remove(kat);
                }
        }
    }
}
