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