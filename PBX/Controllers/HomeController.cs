using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBX.Models;

namespace PBX.Controllers
{
    public class HomeController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        public ActionResult Index()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user!=null)
            {
                ViewBag.user = user;
            }
            return View();
        }

        public ActionResult Messages()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                List<Chat> chats = _db.Chat.Where(item => item.oglaszajacy_id == user.id || item.zainteresowany_id == user.id).ToList();
                chats.Reverse();
                return View(chats);
            }
            else return RedirectToAction("Login", "Account");
        }
    }
}
