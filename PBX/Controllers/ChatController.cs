using PBX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBX.Controllers
{
    public class ChatController : BaseController
    {

        private PBXDBEntities _db = new PBXDBEntities();
        // GET: Chat
        public ActionResult Index(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Chat chat = _db.Chat.Find(id);
                ViewBag.chat_id = id;
                ViewBag.rozmowca = chat.oglaszajacy_id == user.id ? chat.Zainteresowany.imie : chat.Oglaszajacy.imie;
                ViewBag.rozmowca_id = chat.oglaszajacy_id == user.id ? chat.Zainteresowany.id : chat.Oglaszajacy.id;
                ViewBag.ogloszenie_id = _db.Chat.Where(ch => ch.id == id).First().ogloszenie_id;
                ViewBag.ogloszenie_nazwa = _db.Chat.Where(ch => ch.id == id).First().Ogloszenie.nazwa;
                foreach(Wiadomosc w in _db.Wiadomosc.Where(w => w.chat_id == id).ToList()){
                    if(w.nadawca_id!=user.id) w.przeczytano = true;
                }
                _db.SaveChanges();
                return View(chat);
            }
            else return RedirectToAction("Login", "Account");
        }

        public PartialViewResult GetChatView(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            ViewBag.user = user;
            Chat chat = _db.Chat.Find(id);
            return PartialView("ChatPartial", _db.Wiadomosc.Where(w => w.chat_id == id).ToList());
        }

        // GET: Chat/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Chat/Create/5
        public ActionResult Create(int id)
        {
            try
            {
                Uzytkownik user = SharedSession["user"] as Uzytkownik;
                if (user != null)
                {
                    ViewBag.user = user;
                    //int chat_id = _db.Chat.Last().id + 1;
                    int oglaszajacy_id = _db.Ogloszenie.Find(id).wystawil_id;
                    List<Chat> chatsFound = _db.Chat.Where(ch => ch.ogloszenie_id == id && ch.oglaszajacy_id == oglaszajacy_id && ch.zainteresowany_id == user.id).ToList();
                    if (chatsFound.Count() <= 0)
                    {
                        Chat newChat = new Chat
                        {
                            ogloszenie_id = id,
                            oglaszajacy_id = _db.Ogloszenie.Find(id).wystawil_id,
                            zainteresowany_id = user.id
                        };
                        _db.Chat.Add(newChat);
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Chat", new { id = newChat.id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Chat", new { id = chatsFound.First().id });
                    }
                }
                else return RedirectToAction("Login", "Account");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: Chat/MakeMessage
        [HttpPost]
        public ActionResult MakeMessage(int chat, FormCollection collection)
        {
            try
            {
                Uzytkownik user = SharedSession["user"] as Uzytkownik;
                if (user != null)
                {
                    ViewBag.user = user;
                    _db.Wiadomosc.Add(new Wiadomosc
                    {
                        chat_id = chat,
                        nadawca_id = user.id,
                        wiadomosc = collection["wiadomosc"],
                        wyslano = DateTime.Now
                    });
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Chat", new { id = _db.Chat.Find(chat).id });
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Chat/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Chat/Edit/5
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

        // GET: Chat/Delete/5
        public ActionResult Delete(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                return View(_db.Chat.Find(id));
            }
            else return RedirectToAction("Login", "Account");
        }

        // POST: Chat/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Chat chatToDel)
        {
            try
            {
                // TODO: Add delete logic here
                _db.Wiadomosc.Where(w => w.chat_id == id).ToList().ForEach(w =>
                {
                    _db.Wiadomosc.Remove(w);
                });
                _db.Chat.Remove(_db.Chat.Find(id));
                _db.SaveChanges();
                return RedirectToAction("Messages", "Home");
            }
            catch
            {
                return View(chatToDel);
            }
        }
    }
}
