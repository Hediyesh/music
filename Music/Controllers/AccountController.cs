﻿using System;
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

        #region Login and Register
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
        #endregion

        #region Comment
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Comment(string id, string comment, string subject, int subjectid, int replied)
        {
            var user = db.User.Single(s => s.Id.ToString() == id);
            Comments cm = new Comments();
            CommentViewModel cmModel = new CommentViewModel();
            var cmid = 0;
            if (db.Comments.ToList().Count == 0)
            {
                cmid = 1;
            }
            else
            {
                var maxid = db.Comments.OrderByDescending(o => o.Id).First().Id;
                cmid = maxid + 1;
            }
            cm.Id = cmid;
            cm.text = comment;
            cm.subject = subject;
            cm.subjectid = subjectid;
            cm.createDate = DateTime.Now;
            cm.userId = user.Id;
            cm.replied = replied;
            cm.songid = subjectid;
            db.Comments.Add(cm);
            db.SaveChanges();
            cmModel.id = cm.Id;
            cmModel.createDate = cm.createDate;
            cmModel.createDateString = cm.createDate.ToString();
            cmModel.text = cm.text;
            cmModel.subject = cm.subject;
            cmModel.subjectid = cm.subjectid;
            cmModel.userid = user.Id;
            cmModel.replied = cm.replied;
            cmModel.username = user.username;
            cmModel.userpic = user.pic;
            return Json(cmModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            var comment = db.Comments.Single(s => s.Id == id);
            if (comment.replied == 0 && db.Comments.Any(w => w.replied == comment.Id))
            {
                comment.text = "This comment is deleted.";
            }
            else
            {
                db.Comments.Remove(comment);
            }
            db.SaveChanges();
            bool bl = db.Comments.Any(w => w.replied == comment.Id);
            return Json(bl);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditComment(int commentid, string comment)
        {
            var comment1 = db.Comments.Single(s => s.Id == commentid);
            comment1.text = comment;
            db.SaveChanges();
            var cmModel = new CommentViewModel();
            cmModel.id = comment1.Id;
            cmModel.text = comment1.text;
            cmModel.replied = comment1.replied;
            return Json(cmModel);
        }
        #endregion

        #region Like
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LikeSong(string userid, int musicid)
        {
            var likesong = db.LikedSongs.Where(w => w.userId.ToString() == userid && w.SongId == musicid).ToList().FirstOrDefault();
            if (likesong == null)
            {
                var music = db.Song.Where(w => w.Id == musicid).ToList().First();
                var user = db.User.Where(w => w.Id.ToString() == userid).ToList().First();
                LikedSongs ls = new LikedSongs();
                if (db.LikedSongs.ToList().Count == 0)
                {
                    ls.Id = 1;
                }
                else
                {
                    var max = db.LikedSongs.OrderByDescending(o => o.Id).ToList().First().Id;
                    ls.Id = max + 1;
                }
                ls.userId = user.Id;
                ls.SongId = music.Id;
                db.LikedSongs.Add(ls);
                music.likes = music.likes + 1;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IsLiked(string identityname, int musicid)
        {
            var likesong = db.LikedSongs.Where(w => w.userId.ToString() == identityname && w.SongId == musicid).ToList().FirstOrDefault();
            if (likesong != null)
            {
                //liked = true
                return Json(true);
            }
            //not liked = false
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UnLikeSong(string userid, int musicid)
        {
            var likesong = db.LikedSongs.Where(w => w.userId.ToString() == userid && w.SongId == musicid).ToList().First();
            if (likesong != null)
            {
                var music = db.Song.Where(w => w.Id == musicid).ToList().First();
                db.LikedSongs.Remove(likesong);
                music.likes = music.likes - 1;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IsLikedText(string identityname, int musicid)
        {
            var likeText = db.LikedSongText.Where(w => w.userId.ToString() == identityname && w.SongId == musicid).ToList().FirstOrDefault();
            if (likeText != null)
            {
                //liked = true
                return Json(true);
            }
            //not liked = false
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LikeText(string userid, int musicid)
        {
            var liketext = db.LikedSongText.Where(w => w.userId.ToString() == userid && w.SongId == musicid).ToList().FirstOrDefault();
            if (liketext == null)
            {
                var music = db.Song.Where(w => w.Id == musicid).ToList().First();
                var user = db.User.Where(w => w.Id.ToString() == userid).ToList().First();
                LikedSongText lt = new LikedSongText();
                if (db.LikedSongText.ToList().Count == 0)
                {
                    lt.Id = 1;
                }
                else
                {
                    var max = db.LikedSongText.OrderByDescending(o => o.Id).ToList().First().Id;
                    lt.Id = max + 1;
                }
                lt.userId = user.Id;
                lt.SongId = music.Id;
                db.LikedSongText.Add(lt);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UnLikeText(string userid, int musicid)
        {
            var liketext = db.LikedSongText.Where(w => w.userId.ToString() == userid && w.SongId == musicid).ToList().First();
            if (liketext != null)
            {
                db.LikedSongText.Remove(liketext);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IsLikedSinger(string identityname, int singerid)
        {
            var likesinger = db.LikedSingers.Where(w => w.userId.ToString() == identityname && w.SingerId == singerid).ToList().FirstOrDefault();
            if (likesinger != null)
            {
                //liked = true
                return Json(true);
            }
            //not liked = false
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LikeSinger(string userid, int singerid)
        {
            var likesinger = db.LikedSingers.Where(w => w.userId.ToString() == userid && w.SingerId == singerid).ToList().FirstOrDefault();
            if (likesinger == null)
            {
                var singer = db.Singer.Where(w => w.Id == singerid).ToList().First();
                var user = db.User.Where(w => w.Id.ToString() == userid).ToList().First();
                LikedSingers ls = new LikedSingers();
                if (db.LikedSingers.ToList().Count == 0)
                {
                    ls.Id = 1;
                }
                else
                {
                    var max = db.LikedSingers.OrderByDescending(o => o.Id).ToList().First().Id;
                    ls.Id = max + 1;
                }
                ls.userId = user.Id;
                ls.SingerId = singer.Id;
                db.LikedSingers.Add(ls);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UnLikeSinger(string userid, int singerid)
        {
            var likesinger = db.LikedSingers.Where(w => w.userId.ToString() == userid && w.SingerId == singerid).ToList().FirstOrDefault();
            if (likesinger != null)
            {
                db.LikedSingers.Remove(likesinger);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        #endregion

        #region UserPanel
        [Authorize]
        public ActionResult UserPage(string id)
        {
            if (User.Identity.Name == id)
            {
                var user = db.User.Where(w => w.Id.ToString() == id).Single();
                var likedSongs = db.LikedSongs.Where(w => w.userId == user.Id).ToList();
                var q1 = likedSongs.Join(db.Song, p => p.SongId, d => d.Id, (p, d) => new { p, d }).Select(m => new
                {
                    likeid = m.p.Id,
                    m.p.SongId,
                    m.p.userId,
                    songname = m.d.name,
                    m.d.singerId
                });
                var q2 = q1.Join(db.Singer, p => p.singerId, d => d.Id, (p, d) => new { p, d }).Select(m => new
                {
                    m.p.likeid,
                    m.p.songname,
                    m.p.singerId,
                    m.p.SongId,
                    m.p.userId,
                    singername = m.d.name
                });
                List<LikedSongsViewModel> lslist = new List<LikedSongsViewModel>();
                foreach (var item in q2)
                {
                    LikedSongsViewModel ls = new LikedSongsViewModel();
                    ls.likeid = item.likeid;
                    ls.singername = item.singername;
                    ls.songid = item.SongId;
                    ls.userid = item.userId;
                    ls.songname = item.songname;
                    ls.singerid = item.singerId;
                    lslist.Add(ls);
                }
                ViewBag.likedSongs = lslist.ToList();
                ViewBag.likedSongsCount = lslist.ToList().Count;
                var likedTexts = db.LikedSongText.Where(w => w.userId == user.Id).ToList();
                var q3 = likedTexts.Join(db.Song, p => p.SongId, d => d.Id, (p, d) => new { p, d }).Select(m => new
                {
                    likeid = m.p.Id,
                    m.p.SongId,
                    m.p.userId,
                    songname = m.d.name,
                    m.d.singerId
                });
                var q4 = q3.Join(db.Singer, p => p.singerId, d => d.Id, (p, d) => new { p, d }).Select(m => new
                {
                    m.p.likeid,
                    m.p.songname,
                    m.p.singerId,
                    m.p.SongId,
                    m.p.userId,
                    singername = m.d.name
                });
                List<LikedSongsViewModel> lstextlist = new List<LikedSongsViewModel>();
                foreach (var item in q4)
                {
                    LikedSongsViewModel lst = new LikedSongsViewModel();
                    lst.likeid = item.likeid;
                    lst.singername = item.singername;
                    lst.songid = item.SongId;
                    lst.userid = item.userId;
                    lst.songname = item.songname;
                    lst.singerid = item.singerId;
                    lstextlist.Add(lst);
                }
                ViewBag.likedTexts = lstextlist.ToList();
                ViewBag.likedTextsCount = lstextlist.ToList().Count;
                var likedSingers = db.LikedSingers.Where(w => w.userId == user.Id).ToList();
                var q5 = likedSingers.Join(db.Singer, p => p.SingerId, d => d.Id, (p, d) => new { p, d }).Select(m => new
                {
                    likeid = m.p.Id,
                    m.p.SingerId,
                    m.p.userId,
                    singername = m.d.name
                });
                List<LikedSongsViewModel> lsingerlist = new List<LikedSongsViewModel>();
                foreach (var item in q5)
                {
                    LikedSongsViewModel lsinger = new LikedSongsViewModel();
                    lsinger.likeid = item.likeid;
                    lsinger.singername = item.singername;
                    lsinger.userid = item.userId;
                    lsinger.singerid = item.SingerId;
                    lsingerlist.Add(lsinger);
                }
                ViewBag.likedSingers = lsingerlist.ToList();
                ViewBag.likedSingersCount = lsingerlist.ToList().Count;
                var comments = db.Comments.Where(w => w.userId.ToString() == id).ToList();
                List<CommentViewModel> comli = new List<CommentViewModel>();
                foreach (var item in comments)
                {
                    CommentViewModel cm = new CommentViewModel();
                    cm.id = item.Id;
                    cm.subject = item.subject;
                    cm.subjectid = item.subjectid;
                    cm.replied = item.replied;
                    cm.text = item.text;
                    cm.userid = item.userId;
                    cm.createDate = item.createDate;
                    cm.createDateString = item.createDate.ToString();
                    comli.Add(cm);
                }
                ViewBag.Comments = comli.OrderByDescending(o => o.createDate).Reverse().ToList();
                ViewBag.CommentsCount = comli.ToList().Count;
                return View(user);

            }
            return View("NotAuthenticated");
        }
        [Authorize]
        public ActionResult ChangeUserInformation(int id)
        {
            if (User.Identity.Name == id.ToString())
            {
                var user = db.User.Where(w => w.Id == id).Single();
                UserViewModel us = new UserViewModel();
                us.id = user.Id;
                us.bio = user.bio;
                us.firstName = user.firstName;
                us.lastName = user.lastName;
                us.userName = user.username;
                ViewBag.userid = us.id;
                return View(us);

            }
            return View("NotAuthenticated");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeUserInformation(UserViewModel us)
        {
            if (ModelState.IsValid)
            {
                var user = db.User.Where(w => w.Id == us.id).Single();
                user.firstName = us.firstName;
                user.lastName = us.lastName;
                user.bio = us.bio;
                user.username = us.userName;
                db.SaveChanges();
                return RedirectToAction("UserPage/" + us.id.ToString());

            }
            else
            {
                return View(us);
            }
        }


        #endregion
    }
}