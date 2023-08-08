using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;

namespace Music.Controllers
{
    public class AccountController : Controller
    {
        MyMusicDBEntities db = new MyMusicDBEntities();
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Register")]
        public ActionResult Register([Bind(Include = "FirstName,LastName,UserName,Password,RePassword,Email")] RegisterViewModel register)
        {

            if (ModelState.IsValid)
            {
                if (db.User.Any(k => k.emailAddress.Trim().ToLower() == register.Email.Trim().ToLower()))
                {
                    ModelState.AddModelError("Email", "a user with this email exists!");
                }
                if (db.User.Any(k => k.username.Trim().ToLower() == register.UserName.Trim().ToLower()))
                {
                    ModelState.AddModelError("UserName", "a user with this username exists!");
                }
                else
                {
                    User u = new User();
                    u.pic = "/pics/ProfilePic/default-avatar-profile-icon-set-collection-anonymous-social-media-default-avatar-profile-icon-set-collection-anonymous-social-176237699 (2).jpg";
                    var id = 1;
                    var q = db.User.OrderByDescending(o => o.Id).ToList();
                    if (q.Count == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = q.First().Id + 1;
                    }
                    u.Id = id;
                    u.emailAddress = register.Email.Trim().ToLower();
                    u.password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5");
                    u.ActiveCode = Guid.NewGuid().ToString();
                    u.IsActive = true;
                    u.RegisterDate = DateTime.Now;
                    u.RoleId = 1;
                    u.username = register.UserName;
                    u.firstName = register.FirstName;
                    u.lastName = register.LastName;
                    db.User.Add(u);
                    db.SaveChanges();
                    return PartialView("RegisterSuccess", u);
                }
            }
            return View(register);
        }
        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login, string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = db.User.SingleOrDefault(s => s.emailAddress == login.Email && s.password == hashPassword);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), login.RememberMe);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "your account is not active!");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "no user found with this information!");
                }
            }
            return View("Login", login);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        [Route("ForgotPassword")]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var user = db.User.SingleOrDefault(s => s.emailAddress == forgot.Email);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        RecoveryPasswordViewModel r = new RecoveryPasswordViewModel();
                        r.id = user.Id;
                        return Redirect("/Account/RecoveryPassword/" + r.id);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "user is not active.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "no user found!");
                }
            }
            return View(forgot);
        }
        public ActionResult RecoveryPassword(int id)
        {
            RecoveryPasswordViewModel recovery = new RecoveryPasswordViewModel();
            recovery.id = id;
            return View(recovery);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RecoveryPassword(RecoveryPasswordViewModel recovery)
        {
            if (ModelState.IsValid)
            {
                var user = db.User.SingleOrDefault(s => s.Id == recovery.id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.SaveChanges();
                return Redirect("/Login?recovery=true");
            }
            return View("RecoveryPassword", recovery);
        }
    }
}