using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace Music.Controllers
{
    public class HomeController : Controller
    {
        MyMusicDBEntities db = new MyMusicDBEntities();
        public ActionResult Index()
        {
            var q = db.Song.OrderByDescending(w => w.likes).Take(4);
            var q2 = db.Song.OrderByDescending(w => w.viewCount).Take(4);
            List<MusicViewModel> ml = new List<MusicViewModel>();
            foreach (var item in q)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.pic = item.pic;
                m.name = item.name;
                m.singerName = item.singerName;
                m.viewCount = item.viewCount;
                ml.Add(m);
            }
            foreach (var item in q2)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.pic = item.pic;
                m.name = item.name;
                m.singerName = item.singerName;
                m.viewCount = item.viewCount;
                ml.Add(m);
            }
            ViewBag.DrGenreId = new SelectList(db.Genre, "Id", "Id");
            ViewBag.DrGenreName = new SelectList(db.Genre, "Id", "name");
            ViewBag.DrGenrePic = new SelectList(db.Genre, "Id", "pic");
            return View(ml);
        }
        public ActionResult GenresPartial()
        {
            var q = db.Genre.Take(4).ToList();
            List<GenreViewModel> gl = new List<GenreViewModel>();
            foreach (var item in q)
            {
                GenreViewModel g = new GenreViewModel();
                g.Id = item.Id;
                g.pic = item.pic;
                g.Name = item.name;
                gl.Add(g);
            }
            return PartialView("_contentPartialView", gl);

        }
        public ActionResult SingersPartial()
        {
            var q = db.Singer.Take(15).ToList();
            List<SingerViewModel> sl = new List<SingerViewModel>();
            foreach (var item in q)
            {
                SingerViewModel s = new SingerViewModel();
                s.Id = item.Id;
                s.name = item.name;
                sl.Add(s);
            }
            return PartialView("_SingerPartialView", sl);
        }
        public ActionResult SuggestPartial()
        {
            var q = db.Song.OrderByDescending(o => o.likes).Take(5).ToList();
            List<MusicViewModel> ml = new List<MusicViewModel>();
            foreach (var item in q)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.pic = item.pic;
                m.name = item.name;
                m.singerName = item.singerName;
                m.viewCount = item.viewCount;
                ml.Add(m);
            }
            return PartialView("_SuggestPartialView", ml);
        }
        public ActionResult SongPage(int id)
        {
            var q = db.Song.Where(w => w.Id == id).Single();
            var gpname = db.SongGroup.Where(w => w.Id == q.groupId).Single().name;
            var lnname = db.Language.Where(w => w.Id == q.languageId).Single().name;
            var singername = db.Singer.Where(w => w.Id == q.singerId).Single().name;
            var grename = db.Genre.Where(w => w.Id == q.genreId).Single().name;
            MusicViewModel m = new MusicViewModel();
            m.Id = q.Id;
            m.fileLocation = q.filelocation;
            m.genreId = q.genreId;
            m.groupId = q.groupId;
            m.languageId = q.languageId;
            m.genreName = grename;
            m.groupName = gpname;
            m.languageName = lnname;
            m.name = q.name;
            m.note = q.Note;
            m.pic = q.pic;
            m.releaseDate = q.releaseDate;
            m.singerId = q.singerId;
            m.singerName = singername;
            m.youtubeAddress = q.youtubeAddress;
            m.likes = q.likes;
            m.viewCount = q.viewCount;
            ViewBag.Tags = string.Join(",", db.KeyWords.Where(w => w.songid == id).Select(s => s.name).ToList());
            var comments = db.Comments.Where(w => w.songid == id).ToList();
            var comment1 = comments.Where(w => w.replied == 0).Select(s => s).OrderByDescending(o => o.Id).ToList();
            var comment2 = comments.Where(w => w.replied != 0).Select(s => s).OrderByDescending(o => o.Id).ToList();
            List<CommentViewModel> cmlist = new List<CommentViewModel>();
            foreach (var cm in comment1)
            {
                Karbar user = db.Karbar.Where(s => s.Id == cm.userId).ToList().First();
                CommentViewModel cmModel = new CommentViewModel();
                cmModel.id = cm.Id;
                cmModel.createDate = cm.createDate;
                cmModel.createDateString = cm.createDate.ToString("dddd, dd MMMM yyyy");
                cmModel.text = cm.text;
                cmModel.songid = cm.songid;
                cmModel.userid = cm.userId;
                cmModel.username = user.nameKarbari;
                cmModel.userpic = user.pic;
                cmModel.replied = cm.replied;
                cmlist.Add(cmModel);
            }
            List<CommentViewModel> cmlist2 = new List<CommentViewModel>();
            foreach (var cm in comment2)
            {
                Karbar user = db.Karbar.Where(s => s.Id == cm.userId).ToList().First();
                CommentViewModel cmModel = new CommentViewModel();
                cmModel.id = cm.Id;
                cmModel.createDate = cm.createDate;
                cmModel.createDateString = cm.createDate.ToString("dddd, dd MMMM yyyy");
                cmModel.text = cm.text;
                cmModel.songid = cm.songid;
                cmModel.userid = cm.userId;
                cmModel.username = user.nameKarbari;
                cmModel.userpic = user.pic;
                cmModel.replied = cm.replied;
                cmlist2.Add(cmModel);

            }
            ViewBag.Comments = cmlist.ToList();
            ViewBag.RepliedComments = cmlist2.ToList();
            ViewBag.CommentsCount = cmlist2.ToList().Count + cmlist.ToList().Count;
            return View(m);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ViewCount(int musicid)
        {
            var song = db.Song.Where(w => w.Id == musicid).Single();
            song.viewCount += 1;
            db.SaveChanges();
            return Json(song.viewCount);
        }
        public ActionResult ListPage(int id = 0)
        {
            var q = db.Song.ToList();
            List<MusicViewModel> ml = new List<MusicViewModel>();
            foreach (var item in q)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.pic = item.pic;
                m.name = item.name;
                m.singerName = item.singerName;
                m.viewCount = item.viewCount;
                ml.Add(m);
            }
            ViewBag.Tags = string.Join(",", db.KeyWords.Where(w => w.songid != null).Select(s => s.name).ToList());
            var c = db.Song.Count();
            int take = 8;
            ViewBag.PageCount = (c / take) + 1;
            ViewBag.Id = id;
            return View(ml);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ListPage(int id = 0, string id2 = "", int pageid = 0)
        {
            var songs = db.Song.ToList();
            if (id2.Contains("ListPage"))
            {
                if (id == 1)
                {
                    songs = songs.Take(8).ToList();
                }
                if (id > 1)
                {
                    for (int i = 1; i < id; i++)
                    {
                        songs = songs.Skip(8).ToList();
                    }
                    songs = songs.Take(8).ToList();
                }
            }
            if (id2.Contains("ListPageDate"))
            {
                songs = songs.OrderByDescending(s => s.releaseDate).ToList();
                if (id == 1)
                {
                    songs = songs.Take(8).ToList();
                }
                if (id > 1)
                {
                    for (int i = 1; i < id; i++)
                    {
                        songs = songs.Skip(8).ToList();
                    }
                    songs = songs.Take(8).ToList();
                }
            }
            if (id2.Contains("ListPageViewCount"))
            {
                songs = songs.OrderByDescending(s => s.viewCount).ToList();
                if (id == 1)
                {
                    songs = songs.Take(8).ToList();
                }
                if (id > 1)
                {
                    for (int i = 1; i < id; i++)
                    {
                        songs = songs.Skip(8).ToList();
                    }
                    songs = songs.Take(8).ToList();
                }
            }
            if (id2.Contains("ListPageGenre"))
            {
                songs = songs.Where(s => s.genreId == pageid).ToList();
                if (id == 1)
                {
                    songs = songs.Take(8).ToList();
                }
                if (id > 1)
                {
                    for (int i = 1; i < id; i++)
                    {
                        songs = songs.Skip(8).ToList();
                    }
                    songs = songs.Take(8).ToList();
                }
            }
            var q1 = songs.Join(db.Singer, p => p.singerId, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.genreId,
                m.p.groupId,
                m.p.Id,
                m.p.languageId,
                m.p.singerId,
                m.p.pic,
                m.p.name,
                singername = m.d.name
            });
            List<MusicViewModel> ml = new List<MusicViewModel>();
            foreach (var item in q1)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.name = item.name;
                m.genreId = item.genreId;
                m.groupId = item.groupId;
                m.languageId = item.languageId;
                m.singerName = item.singername;
                m.singerId = item.singerId; m.pic = item.pic;
                ml.Add(m);
            }

            return Json(ml);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Search(string search)
        {
            return View();
        }
    }
}