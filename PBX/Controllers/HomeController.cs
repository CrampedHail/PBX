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
            return View(_db.Kategoria.Take(8));
        }

        public ActionResult Messages()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user == null) return RedirectToAction("Login", "Account");
            ViewBag.user = user;
            
            List<Chat> chats = _db.Chat.Where(item => item.oglaszajacy_id == user.id || item.zainteresowany_id == user.id)
                .Where(item => item.Wiadomosci.Count() > 0)
                .OrderByDescending(chat => chat.Wiadomosci.OrderByDescending(ch => ch.wyslano).FirstOrDefault().wyslano).ToList();

            ViewBag.chatIsRead = chats
                .Select(chat => 
                    chat.Wiadomosci.OrderByDescending(ch => ch.wyslano).FirstOrDefault().nadawca_id == user.id
                        ? true
                        : chat.Wiadomosci.OrderByDescending(ch => ch.wyslano).FirstOrDefault().przeczytano
                    )
                .ToList<bool>();

            ViewBag.chatIds = chats.Select(ch => ch.id).ToList<int>();
            /*ViewBag.chatIsRead = _db.Wiadomosc.Where(w => user.id==w.Chat.oglaszajacy_id || user.id == w.Chat.zainteresowany_id)
                .OrderByDescending(w => w.wyslano).Select(w => w.nadawca_id==user.id || w.przeczytano).ToList<bool>();*/
            /*ViewBag.chatIds = _db.Wiadomosc.Where(w => user.id == w.Chat.oglaszajacy_id || user.id == w.Chat.zainteresowany_id)
                .OrderByDescending(w => w.wyslano).Select(w => w.chat_id).ToList<int>();*/
            return View(chats);
        }
    }
}
