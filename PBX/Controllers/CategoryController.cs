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
            return View(_db.Kategoria.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            return View(_db.Kategoria.Find(id));
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ViewBag.categories = _db.Kategoria.Select(k => k.nazwa).ToList();
            ViewBag.categories_id = _db.Kategoria.Select(k => k.id).ToList();
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_db.Kategoria.Find(id));
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_db.Kategoria.Find(id));
        }

        // POST: Category/Delete/5
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
