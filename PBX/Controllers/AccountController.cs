using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PBX.Models;
using BC = BCrypt.Net.BCrypt;

namespace PBX.Controllers
{
    public class AccountController : BaseController
    {
        private PBXDBEntities _db = new PBXDBEntities();
        // GET: Account/Details/5
        public ActionResult Details(int id=-1)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                if (id < 0) id = user.id;
                int iloscOcen = _db.Ocena.Where(o => o.ocena_dla_id == id).Count();
                double wartoscOcen = iloscOcen > 0 ? _db.Ocena.Where(o => o.ocena_dla_id==id).Sum(o => o.ocena1) : 0;
                double sredniaOcen = iloscOcen > 0 ? wartoscOcen / iloscOcen * 1.0 : 0.0;
                ViewBag.iloscOcen = iloscOcen;
                ViewBag.sredniaOcen = (sredniaOcen * 1.0);
                ViewBag.cal = Int32.Parse(Math.Round(sredniaOcen).ToString()).ToString();
                ViewBag.resz = Int32.Parse((Math.Round(sredniaOcen % 1, 1)*10).ToString()).ToString(); 

                int chatsFound = _db.Chat.Where(c =>
                (c.oglaszajacy_id == id && c.zainteresowany_id == user.id) ||
                (c.zainteresowany_id == id && c.oglaszajacy_id == user.id)
                ).Count();

                int messagesInChat = 0;
                messagesInChat += _db.Wiadomosc.Where(w => w.nadawca_id == id).Count() > 0 ? 1 : 0;
                messagesInChat += _db.Wiadomosc.Where(w => w.nadawca_id == user.id).Count() > 0 ? 1 : 0;

                int ratesFound = _db.Ocena.Where(o => o.ocena_od_id == user.id && o.ocena_dla_id == id).Count();
                ViewBag.mozeWystawicOcene = ratesFound == 0 && chatsFound > 0 && messagesInChat >= 2;
                return id > 0 ? View(_db.Uzytkownik.Find(id)) : View(user);
            }
            else return RedirectToAction("Login", "Account");
        }

        // GET: Account/AddRate/5
        public ActionResult AddRate(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;

                return View(new Ocena()
                {
                    ocena_dla_id = id,
                    ocena_od_id = user.id
                });
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Account/AddRate/5
        [HttpPost]
        public ActionResult AddRate(int id, FormCollection collection)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
                Ocena ocena = new Ocena()
                {
                    ocena_dla_id = id,
                    ocena_od_id = user.id,
                    ocena1 = Double.Parse(collection["ocena"])
            };
                _db.Ocena.Add(ocena);
                _db.SaveChanges();
                return RedirectToAction("Details", "Account", new { id = ocena.ocena_dla_id });
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(Uzytkownik newUser)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            try
            {
                int samePhones = _db.Uzytkownik.Where(u => u.nr_tel.Equals(newUser.nr_tel)).Count();
                samePhones += _db.Usunieci.Where(u => u.nr_tel.Equals(newUser.nr_tel)).Count();
                int sameEmails = _db.Uzytkownik.Where(u => u.email.Equals(newUser.email)).Count();
                sameEmails += _db.Uzytkownik.Where(u => u.email.Equals(newUser.email)).Count();
                if (ModelState.IsValid && samePhones==0 && sameEmails== 0)
                {
                    newUser.imie = newUser.imie.Trim();
                    newUser.dolaczono = DateTime.Today;
                    newUser.haslo = BC.HashPassword(newUser.haslo);
                    _db.Uzytkownik.Add(newUser);
                    _db.SaveChanges();
                    SharedSession["user"] = newUser;
                    SharedSession.Timeout = 20;
                    SendEmail(newUser.email, "Witamy w serwisie PBX!", "Dzień dobry, " + newUser.imie + "\n\nDziękujemy za rejestrację i życzymy miłego użytkowania z serwisu! \n\nPozdrawiamy, Zespół PBX :)");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (sameEmails > 0 && samePhones > 0) ViewBag.Error = "Podany numer telefonu oraz adres email są już zajęte!";
                    else if (sameEmails > 0) ViewBag.Error = "Podany adres email jest już zajęty!";
                    else if (samePhones > 0) ViewBag.Error = "Podany numer telefonu jest już zajęty!";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/ConfirmEmail
        public ActionResult ConfirmEmail(Uzytkownik u)
        {
            string chars = "ABCDEFGHIJKLMNOPRSTUQVWXYZ1234567890";
            string verificationCode = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                verificationCode += chars.ElementAt(random.Next(chars.Length - 1));
            }
            SharedSession.Timeout = 60;
            SharedSession["code"] = verificationCode;
            SendEmail(u.email, "Kod weryfikacyjny do serwisu PBX!", "Dzień dobry, " + u.imie + " \n\nKonto zostanie zarejestrowne zaraz po tym gdy wpiszesz poniższy kod na naszej stronie:  " + verificationCode + " \n\nPozdrawiamy, Zespół PBX :)");
            
            return View(u);
        }

        // Post: Account/ConfirmEmail
        [HttpPost]
        public ActionResult ConfirmEmail(FormCollection collection, Uzytkownik u)
        {
            string sessionCode = SharedSession["code"] as string;
            string code = collection["first"] + collection["second"] + collection["third"] +
                collection["fourth"] + collection["fifth"] + collection["sixth"];
            if (sessionCode==null || sessionCode == String.Empty)
            {
                ViewBag.error = "Sesja wygasła. Spróbuj ponownie.";
                return View();
            }
            else if(code.Equals(sessionCode))
            {
                return Create(u);
            }
            else if (!code.Equals(sessionCode))
            {
                ViewBag.error = "Podano niepoprawny kod weryfikacyjny.";
                return View();
            }
            return View();
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            SharedSession["user"] = null;
            SharedSession["admin"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string password = collection["haslo"];
            string email = collection["email"];
            try
            {
                List<Uzytkownik> usersFound = _db.Uzytkownik.Where(user => user.email.Equals(email)).ToList();
                if (usersFound.Count()==1 && BC.Verify(password, usersFound[0].haslo))
                {
                    usersFound[0].imie = usersFound[0].imie.Trim();
                    SharedSession.Timeout = 20;
                    SharedSession["user"] = usersFound[0];
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    List<Admin> adminAccounts = _db.Admin.Where(a => a.login.Equals(email)).ToList();
                    Admin adminAccount = adminAccounts.Count() > 0 ? adminAccounts.First() : null;
                    bool admin = adminAccount != null ? BC.Verify(password, adminAccount.haslo) : false;
                    if (admin)
                    {
                        SharedSession.Timeout = 60;
                        SharedSession["admin"] = adminAccount;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        List<Usunieci> usunieci = _db.Usunieci.Where(u => u.email.Equals(email)).ToList();
                        Usunieci usuniety = usunieci.Count() > 0 ? usunieci.First() : null;
                        if (usuniety != null)
                        {
                            ViewBag.Error = "Przepraszamy, Twoje konto zostało usunięte.";
                        }
                        else
                        {
                            ViewBag.Error = "Przepraszamy, nie znaleziono konta o wprowadzonych danych.";
                        }
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Przepraszamy, nie udało się zalogować.";
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            if (SharedSession["user"] != null) return View(SharedSession["user"] as Uzytkownik);
            else return RedirectToAction("Index", "Home");
        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; 
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(Uzytkownik editedUser)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            var originalUser = _db.Uzytkownik.Find(editedUser.id);
            editedUser.imie = editedUser.imie != null ? editedUser.imie.Trim() : "";
            editedUser.email = editedUser.email != null ? editedUser.email.Trim() : "";
            editedUser.nr_tel = editedUser.nr_tel != null ? editedUser.nr_tel.Trim() : "";
            int errors = 0;
            if(editedUser.imie.Length<3 || editedUser.imie.Length > 50)
            {
                ViewBag.NameError = "Podane imię ma nieprawidłową długość.";
                errors++;
            }
            if (editedUser.nr_tel.Where(ch => "1234567890".Contains(ch)).Count() != 9)
            {
                ViewBag.PhoneError = "Podany numer telefonu ma nieprawidłową długość.";
                errors++;
            }
            if (!IsValidEmail(editedUser.email))
            {
                ViewBag.EmailError = "Podany adres email jest niepoprawny.";
                errors++;
            }
            if (errors==0)
            {
                try
                {
                    // TODO: Add update logic here
                    if (TryUpdateModel(originalUser, new string[] { "imie", "nr_tel", "email" }))
                    {
                        _db.SaveChanges();
                        SharedSession["user"] = editedUser;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return View(editedUser);
                }
                catch
                {
                    return View(editedUser);
                }
            }
            else
            {
                return View(editedUser);
            }
        }

        // GET: Account/ChangePassword/5
        public ActionResult ChangePassword()
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            return View(user);
        }

        // POST: Account/ChangePassword/5
        [HttpPost]
        public ActionResult ChangePassword(Uzytkownik user, FormCollection collection)
        {
            //Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            var originalUser = _db.Uzytkownik.Find(user.id);
            try
            {
                if(BC.Verify( collection["old_password"], originalUser.haslo) &&
                    collection["new_password"].Equals(collection["confirm_password"]) &&
                    !collection["old_password"].Equals(collection["new_password"]) &&
                    ModelState.IsValid)
                {
                    _db.Uzytkownik.Find(user.id).haslo = BC.HashPassword(collection["new_password"]);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: Account/Edit/5
        public ActionResult ForgotPassword( )
        {
            return View();
        }

        private void SendEmail(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("pbx.serwis.ogloszeniowy@gmail.com", "Serwis Ogłoszeniowy PBX");
            var toAddress = new MailAddress(email);
            const string fromPassword = "Haslo123!";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private string GeneratePassword()
        {
            string chars = "ABCDEFGHIJKLMNOPRSTUWYQXZ1234567890";
            string password = "";
            Random random = new Random();
            for(int i = 0; i < 10; i++)
            {
                password+=chars.ElementAt(random.Next(chars.Length));
            }
            return password;
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            string password = "";
            try
            {
                string name = collection["imie"];
                string phone = collection["nr_tel"];
                string email = collection["email"];
                password = GeneratePassword();
                ViewBag.Error = password;
                List<Uzytkownik> found = _db.Uzytkownik.Where(u => u.nr_tel.Equals(phone)).ToList();
                /*found = found.Where(u => u.imie.Equals(name)).ToList();*/
                found = found.Where(u => u.email.Equals(email)).ToList();
                Uzytkownik userFound = found.First();
                _db.Uzytkownik.Find(userFound.id).haslo = BC.HashPassword(password);
                _db.SaveChanges();

                SendEmail(email, "Przypominamy hasło do serwisu PBX!", 
                    "Dzień dobry, "+name+"\n\nOtrzymaliśmy twoje zgłoszenie o zapomnianym haśle. Twoje nowe hasło to: " + password+ " \n\nPozdrawiamy, Zespół PBX :)");

                return RedirectToAction("Index", "Home");
            }
            catch(ArgumentNullException)
            {
                ViewBag.Error += "Nie znaleziono użytkownika o wprowadzonych danych.";
                return View();
            }
            catch
            {
                ViewBag.Error += "Wystąpił błąd. Proszę spróbować później. ";
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Uzytkownik user = SharedSession["user"] as Uzytkownik;
            if (user != null)
            {
                ViewBag.user = user;
            }
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
