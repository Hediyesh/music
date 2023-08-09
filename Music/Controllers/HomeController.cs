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
            var comments = db.Comments.Where(w => w.songid == id).ToList();
            var comment1 = comments.Where(w => w.replied == 0).Select(s => s).OrderByDescending(o => o.Id).ToList();
            var comment2 = comments.Where(w => w.replied != 0).Select(s => s).OrderByDescending(o => o.Id).ToList();
            List<CommentViewModel> cmlist = new List<CommentViewModel>();
            foreach (var cm in comment1)
            {
                User user = db.User.Where(s => s.Id == cm.userId).ToList().First();
                CommentViewModel cmModel = new CommentViewModel();
                cmModel.id = cm.Id;
                cmModel.createDate = cm.createDate;
                cmModel.createDateString = cm.createDate.ToString("dddd, dd MMMM yyyy");
                cmModel.text = cm.text;
                cmModel.songid = cm.songid;
                cmModel.userid = cm.userId;
                cmModel.username = user.username;
                cmModel.userpic = user.pic;
                cmModel.replied = cm.replied;
                cmlist.Add(cmModel);
            }
            List<CommentViewModel> cmlist2 = new List<CommentViewModel>();
            foreach (var cm in comment2)
            {
                User user = db.User.Where(s => s.Id == cm.userId).ToList().First();
                CommentViewModel cmModel = new CommentViewModel();
                cmModel.id = cm.Id;
                cmModel.createDate = cm.createDate;
                cmModel.createDateString = cm.createDate.ToString("dddd, dd MMMM yyyy");
                cmModel.text = cm.text;
                cmModel.songid = cm.songid;
                cmModel.userid = cm.userId;
                cmModel.username = user.username;
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
        public ActionResult ListPageGenre(int id = 0)
        {
            ViewBag.Id = id;
            var q = db.Song.Where(w => w.genreId == id).ToList();
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
            var c = q.Count();
            int take = 8;
            ViewBag.PageCount = (c / take) + 1;
            return View("ListPage", ml);
        }
        public ActionResult ListPageDate(int id = 0)
        {
            ViewBag.Id = id;
            var q = db.Song.OrderByDescending(o => o.releaseDate).ToList();
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
            var c = q.Count();
            int take = 8;
            ViewBag.PageCount = (c / take) + 1;
            return View("ListPage", ml);
        }
        public ActionResult ListPageViewCount(int id = 0)
        {
            ViewBag.Id = id;
            var q = db.Song.OrderByDescending(o => o.viewCount).ToList();
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
            var c = q.Count();
            int take = 8;
            ViewBag.PageCount = (c / take) + 1;
            return View("ListPage", ml);
        }
        public ActionResult SongGenres()
        {
            var q = db.Genre.ToList();
            List<GenreViewModel> gl = new List<GenreViewModel>();
            foreach (var item in q)
            {
                GenreViewModel g = new GenreViewModel();
                g.Id = item.Id;
                g.pic = item.pic;
                g.Name = item.name;
                gl.Add(g);
            }
            return View(gl);
        }
        public ActionResult SingerSongsPage(int id)
        {
            var q = db.Singer.Where(w => w.Id == id);
            var q2 = q.Join(db.Song, p => p.Id, d => d.singerId, (p, d) => new { p, d }).Select(m => new
            {
                singerId = m.p.Id,
                singername = m.p.name,
                singergpid = m.p.groupId,
                singergenreid = m.p.genreId,
                singerlanid = m.p.languageId,
                musicid = m.d.Id,
                musicName = m.d.name,
                musicgnre = m.d.genreId,
                musicgpid = m.d.groupId,
                musiclanid = m.d.languageId,
                musicdate = m.d.releaseDate,
                musicfilelocation = m.d.filelocation,
                musicnote = m.d.Note,
                musicpic = m.d.pic,
                musiclikes = m.d.likes,
                musicViews = m.d.viewCount
            });
            var q3 = q2.Join(db.Language, p => p.musiclanid, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.musicdate,
                m.p.musicfilelocation,
                m.p.musicgnre,
                m.p.musicgpid,
                m.p.musicid,
                m.p.musiclanid,
                m.p.musiclikes,
                m.p.musicName,
                m.p.musicnote,
                m.p.musicpic,
                m.p.musicViews,
                m.p.singergenreid,
                m.p.singergpid,
                m.p.singerId,
                m.p.singerlanid,
                m.p.singername,
                musiclanName = m.d.name
            });
            var q4 = q3.Join(db.Genre, p => p.musicgnre, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.musicdate,
                m.p.musicfilelocation,
                m.p.musicgnre,
                m.p.musicgpid,
                m.p.musicid,
                m.p.musiclanid,
                m.p.musiclikes,
                m.p.musicName,
                m.p.musicnote,
                m.p.musicpic,
                m.p.musicViews,
                m.p.singergenreid,
                m.p.singergpid,
                m.p.singerId,
                m.p.singerlanid,
                m.p.singername,
                m.p.musiclanName,
                musicgenreName = m.d.name
            });
            var q5 = q4.Join(db.Genre, p => p.singergenreid, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.musicdate,
                m.p.musicfilelocation,
                m.p.musicgnre,
                m.p.musicgpid,
                m.p.musicid,
                m.p.musiclanid,
                m.p.musiclikes,
                m.p.musicName,
                m.p.musicnote,
                m.p.musicpic,
                m.p.musicViews,
                m.p.singergenreid,
                m.p.singergpid,
                m.p.singerId,
                m.p.singerlanid,
                m.p.singername,
                m.p.musiclanName,
                m.p.musicgenreName,
                singergenreName = m.d.name
            });
            List<SingerMusicsViewModel> sml = new List<SingerMusicsViewModel>();
            foreach (var item in q5)
            {
                SingerMusicsViewModel sm = new SingerMusicsViewModel();
                sm.singername = item.singername;
                sm.singerid = item.singerId;
                sm.singergpid = item.singergpid;
                sm.singergenreid = item.singergenreid;
                sm.musicviews = item.musicViews;
                sm.musicpic = item.musicpic;
                sm.musicnote = item.musicnote;
                sm.musicname = item.musicName;
                sm.musiclikes = item.musiclikes;
                sm.musiclanName = item.musiclanName;
                sm.musiclanid = item.musiclanid;
                sm.musicid = item.musicid;
                sm.musicgpid = item.musicgpid;
                sm.musicgenreid = item.musicgnre;
                sm.musicfilelocation = item.musicfilelocation;
                sm.musicdate = item.musicdate;
                sm.musicgenreName = item.musicgenreName;
                sm.singergenreName = item.singergenreName;
                sml.Add(sm);
            }
            ViewBag.SingerName = q.First().name;
            ViewBag.Singerid = q.First().Id;
            var gid = q.First().genreId;
            var gnre = db.Genre.Where(w => w.Id == gid).ToList().Single();
            ViewBag.SingerGenreName = gnre.name;
            return View(sml);
        }
        public ActionResult SingersPage()
        {
            var q = db.Singer.ToList();
            q = q.Take(8).ToList();
            var q1 = q.Join(db.Language, p => p.languageId, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.genreId,
                m.p.groupId,
                m.p.Id,
                m.p.languageId,
                m.p.name,
                lanName = m.d.name
            });
            var q2 = q1.Join(db.Genre, p => p.genreId, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.genreId,
                m.p.groupId,
                m.p.Id,
                m.p.languageId,
                m.p.name,
                m.p.lanName,
                genreName = m.d.name
            });
            List<SingerViewModel> sl = new List<SingerViewModel>();
            foreach (var item in q2)
            {
                SingerViewModel s = new SingerViewModel();
                s.Id = item.Id;
                s.name = item.name;
                s.gnreid = item.genreId;
                s.gpid = item.groupId;
                s.languageid = item.languageId;
                s.languageName = item.lanName;
                s.genreName = item.genreName;
                sl.Add(s);
            }

            var c = db.Singer.Count();
            int take = 8;
            ViewBag.PageCount = (c / take) + 1;
            ViewBag.SingerCount = c;
            return View(sl);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SingersPage(int id = 0)
        {
            var singers = db.Singer.ToList();
            if (id == 1)
            {
                singers = singers.Take(8).ToList();
            }
            if (id > 1)
            {
                for (int i = 1; i < id; i++)
                {
                    singers = singers.Skip(8).ToList();
                }
                singers = singers.Take(8).ToList();
            }
            var q1 = singers.Join(db.Language, p => p.languageId, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.genreId,
                m.p.groupId,
                m.p.Id,
                m.p.languageId,
                m.p.name,
                lanName = m.d.name
            });
            var q2 = q1.Join(db.Genre, p => p.genreId, d => d.Id, (p, d) => new { p, d }).Select(m => new
            {
                m.p.genreId,
                m.p.groupId,
                m.p.Id,
                m.p.languageId,
                m.p.name,
                m.p.lanName,
                genreName = m.d.name
            });

            List<SingerViewModel> sl = new List<SingerViewModel>();
            foreach (var item in q2)
            {
                SingerViewModel s = new SingerViewModel();
                s.Id = item.Id;
                s.name = item.name;
                s.gnreid = item.genreId;
                s.gpid = item.groupId;
                s.languageid = item.languageId;
                s.languageName = item.lanName;
                s.genreName = item.genreName;
                sl.Add(s);
            }

            return Json(sl);
        }

        public ActionResult Search(string id)
        {
            List<MusicViewModel> ml = new List<MusicViewModel>();
            var sr = db.Song.Where(w => w.name.ToLower().Contains(id.ToLower()) == true || id.ToLower().Contains(w.name.ToLower()) == true).ToList().Distinct();
            foreach (var item in sr)
            {
                MusicViewModel m = new MusicViewModel();
                m.Id = item.Id;
                m.pic = item.pic;
                m.name = item.name;
                m.singerName = item.singerName;
                m.viewCount = item.viewCount;
                ml.Add(m);
            }
            ViewBag.c = ml.Count();
            return View(ml);
        }
    }
}